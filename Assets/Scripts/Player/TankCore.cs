using UnityEngine;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.ScriptableObjects.Variables;
using Assets.Scripts.Enemy;

namespace Assets.Scripts.Player
{
    /// <summary>
    /// Класс для логики танка, которым управляет игрок
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class TankCore : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Слои, которые наносят урон")]
        private LayerMask EnemyLayer;

        [SerializeField]
        [Tooltip("Ссылка на переменную хранящую текущее здоровье")]
        private FloatReference CurrentHealthVariable;

        [SerializeField]
        [Tooltip("Ссылка на объект события убийства игрока")]
        private GameEventScriptableObject PlayerKilledEvent;

        //Используемый объект описания танка
        private TankScriptableObject _tankDescription;

        //Ссылки на колеса танка
        [SerializeField]
        private GameObject[] _rightWheels;
        [SerializeField]
        private GameObject[] _leftWheels;

        //Ссылки на компоненты танка
        private Rigidbody _rigidBody;
        private BoxCollider _boxCollider;

        //Локальные переменные
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
                    PlayerKilledEvent.Invoke();
            }
        }

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _boxCollider = GetComponent<BoxCollider>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            var otherLayer = collision.collider.gameObject.layer;

            if ((EnemyLayer & 1 << otherLayer) == 1 << otherLayer)
            {
                EnemyCore enemy = collision.gameObject.GetComponent<EnemyCore>();
                Harm(enemy.GetDescription());
            }
        }

        /// <summary>
        /// Задать описание танка
        /// </summary>
        /// <param name="tankDesc">Описание танка</param>
        public void SetDescription(TankScriptableObject tankDesc)
        {
            _tankDescription = tankDesc;

            CurrentHealth = _tankDescription.Health;
        }

        public TankScriptableObject GetDescription()
        {
            return _tankDescription;
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
            for (int i = 0; i < _rightWheels.Length; i++)
            {
                var tr = _rightWheels[i].transform;
                tr.Rotate(tr.rotation.x + vertInputValue * 5f, 0f, 0f);
            }
            for (int i = 0; i < _leftWheels.Length; i++)
            {
                var tr = _leftWheels[i].transform;
                tr.Rotate(tr.rotation.x + vertInputValue * 5f, 0f, 0f);
            }
        }

        /// <summary>
        /// Повернуть танк в направлении заданном вводом
        /// </summary>
        /// <param name="horInputValue"></param>
        public void RotateTank(float horInputValue)
        {
            Quaternion rotationQuat = Quaternion.Euler(0f, horInputValue * 45f * Time.fixedDeltaTime, 0f);

            _rigidBody.MoveRotation(_rigidBody.rotation * rotationQuat);

            //Поворачиваем колеса
            for (int i = 0; i < _rightWheels.Length; i++)
            {
                var tr = _rightWheels[i].transform;
                tr.Rotate(tr.rotation.x - horInputValue * 5f, 0f, 0f);
            }
            for (int i = 0; i < _leftWheels.Length; i++)
            {
                var tr = _leftWheels[i].transform;
                tr.Rotate(tr.rotation.x + horInputValue * 5f, 0f, 0f);
            }
        }

        /// <summary>
        /// Метод обработки удара врага
        /// </summary>
        /// <param name="enemyDesc">Описание врага, который нанес урон</param>
        private void Harm(EnemyScriptableObject enemyDesc)
        {
            CurrentHealth = CurrentHealthVariable.Value - enemyDesc.Damage * _tankDescription.Defence;
        }
    }
}
