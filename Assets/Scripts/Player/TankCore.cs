using UnityEngine;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.ScriptableObjects.Variables;
using Assets.Scripts.Enemy;
using Assets.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Player
{
    /// <summary>
    /// Класс для логики танка, которым управляет игрок
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(MeshRenderer))]
    public class TankCore : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Слои, которые наносят урон")]
        private LayerMask HarmLayer;

        [Header("Ссылки на события и переменные")]
        [SerializeField]
        [Tooltip("Ссылка на переменную хранящую текущее здоровье")]
        private FloatReference CurrentHealthVariable;

        [SerializeField]
        [Tooltip("Ссылка на объект события убийства игрока")]
        private GameEventScriptableObject PlayerKilledEvent;

        [SerializeField]
        [Tooltip("Ссылка на объект события убийства врага")]
        private GameEventScriptableObject EnemyKilledEvent;

        [SerializeField]
        [Tooltip("Ссылка на объект смены текущего оружия")]
        private GameEventScriptableObject WeaponChangedEvent;

        [Header("Ссылки на внутренние объекты и компоненты")]
        [SerializeField]
        [Tooltip("Ссылки на внутренние объекты правых колес танка")]
        private GameObject[] RightWheels;

        [SerializeField]
        [Tooltip("Ссылки на внутренние объекты левых колес танка")]
        private GameObject[] LeftWheels;

        [SerializeField]
        [Tooltip("Ссылка на внутренний объект обычной пушки")]
        private GameObject Canon;

        [SerializeField]
        [Tooltip("Ссылка на внутренний объект пулемета")]
        public GameObject MachineGun;

        [SerializeField]
        [Tooltip("Ссылки на все MeshRenderer тела танка")]
        private MeshRenderer[] BodyMeshRenderers;

        /// <summary>
        /// Описание танка
        /// </summary>
        public TankScriptableObject TankDescription
        {
            get
            {
                return _tankDescription;
            }
            set
            {
                _tankDescription = value;

                CurrentHealth = value.Health;

                for (int i = 0; i < BodyMeshRenderers.Length; i++)
                    BodyMeshRenderers[i].material.color = value.TankColor;
            }
        }

        /// <summary>
        /// Текущее здоровье
        /// </summary>
        public float CurrentHealth
        {
            get
            {
                return CurrentHealthVariable;
            }
            private set
            {
                CurrentHealthVariable.Variable.SetValue(value);

                if(CurrentHealthVariable <= 0f)
                {
                    gameObject.SetActive(false);
                    PlayerKilledEvent.Invoke();
                }
            }
        }

        private TankScriptableObject _tankDescription;
        private Rigidbody _rigidBody;
        private Dictionary<EnemyCore, Coroutine> _takenDamageFrom = new Dictionary<EnemyCore, Coroutine>();

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            WeaponChangedEvent.OnGameEvent += WeaponChangedHandler;
            EnemyKilledEvent.OnTransformEvent += EnemyTransformHandler;
        }

        private void OnDisable()
        {
            WeaponChangedEvent.OnGameEvent -= WeaponChangedHandler;
            EnemyKilledEvent.OnTransformEvent -= EnemyTransformHandler;
        }

        private void OnCollisionEnter(Collision collision)
        {
            var otherLayer = collision.collider.gameObject.layer;

            if ((HarmLayer & 1 << otherLayer) == 1 << otherLayer)
            {
                EnemyCore enemy = collision.gameObject.GetComponent<EnemyCore>();
                if (!_takenDamageFrom.ContainsKey(enemy))
                {
                    var newCoroutine = StartCoroutine(DamagerPlayer(enemy.EnemyDescription, 1f));
                    _takenDamageFrom.Add(enemy, newCoroutine);
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            var otherLayer = collision.collider.gameObject.layer;

            if ((HarmLayer & 1 << otherLayer) == 1 << otherLayer)
            {
                EnemyCore enemy = collision.gameObject.GetComponent<EnemyCore>();
                if (_takenDamageFrom.ContainsKey(enemy))
                {
                    StopCoroutine(_takenDamageFrom[enemy]);
                    _takenDamageFrom.Remove(enemy);
                }
            }
        }

        /// <summary>
        /// Обработчик события убийства врага, принимает на вход Transform
        /// Это обходное решение проблемы, что при убийстве врага, не срабатывает OnCollisionEnter и урон продолжает наноситься
        /// </summary>
        /// <param name="args">Аргумент события</param>
        private void EnemyTransformHandler(TransformArgs args)
        {
            var enemy = args.TransformArg.gameObject.GetComponent<EnemyCore>();

            Coroutine cor;
            _takenDamageFrom.TryGetValue(enemy, out cor);

            if(cor != null)
            {
                StopCoroutine(cor);
                _takenDamageFrom.Remove(enemy);
            }
        }

        /// <summary>
        /// Корутина, которая наносит игроку урон каждые несколько секунд
        /// </summary>
        /// <param name="desc">Описание врага</param>
        /// <param name="seconds">Сколько секунд происходит между атаками</param>
        /// <returns></returns>
        IEnumerator DamagerPlayer(EnemyScriptableObject desc, float seconds)
        {
            while(true)
            {
                Harm(desc);
                yield return new WaitForSeconds(seconds);
            }
        }

        /// <summary>
        /// Передвинуть танк в направлении заданном вводом
        /// </summary>
        /// <param name="vertInputValue">Значение ввода</param>
        public void MoveTank(float vertInputValue)
        {
            Vector3 movementVector = transform.forward * _tankDescription.Speed * vertInputValue * Time.fixedDeltaTime;

            _rigidBody.MovePosition(_rigidBody.position + movementVector);

            //Вращаем колеса
            for (int i = 0; i < RightWheels.Length; i++)
            {
                var tr = RightWheels[i].transform;
                tr.Rotate(tr.rotation.x + vertInputValue * 5f, 0f, 0f);
            }
            for (int i = 0; i < LeftWheels.Length; i++)
            {
                var tr = LeftWheels[i].transform;
                tr.Rotate(tr.rotation.x + vertInputValue * 5f, 0f, 0f);
            }
        }

        /// <summary>
        /// Повернуть танк в направлении заданном вводом
        /// </summary>
        /// <param name="horInputValue"></param>
        public void RotateTank(float horInputValue)
        {
            Quaternion rotationQuat = Quaternion.Euler(0f, horInputValue * 65f * Time.fixedDeltaTime, 0f);

            _rigidBody.MoveRotation(_rigidBody.rotation * rotationQuat);

            //Поворачиваем колеса
            for (int i = 0; i < RightWheels.Length; i++)
            {
                var tr = RightWheels[i].transform;
                tr.Rotate(tr.rotation.x - horInputValue * 5f, 0f, 0f);
            }
            for (int i = 0; i < LeftWheels.Length; i++)
            {
                var tr = LeftWheels[i].transform;
                tr.Rotate(tr.rotation.x + horInputValue * 5f, 0f, 0f);
            }
        }

        /// <summary>
        /// Нанести урон игроку
        /// </summary>
        /// <param name="enemyDesc">Описание врага, который нанес урон</param>
        private void Harm(EnemyScriptableObject enemyDesc)
        {
            CurrentHealth = CurrentHealthVariable.Value - enemyDesc.Damage * _tankDescription.Defence;
        }

        /// <summary>
        /// Обработчик события смены оружия
        /// </summary>
        private void WeaponChangedHandler()
        {
            //TODO: Не нравится обращение к контейнеру
            var weapon = ManagerContainer.Get<WeaponManager>().CurrentWeapon;

            switch(weapon.WeaponType)
            {
                case WeaponTypeEnum.Canon:
                    {
                        Canon.SetActive(true);
                        MachineGun.SetActive(false);
                        break;
                    }
                case WeaponTypeEnum.MachineGun:
                    {
                        Canon.SetActive(false);
                        MachineGun.SetActive(true);
                        break;
                    }
            }
        }
    }
}
