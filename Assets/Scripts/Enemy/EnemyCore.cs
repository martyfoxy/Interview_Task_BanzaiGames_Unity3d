using Assets.Scripts.Player;
using Assets.Scripts.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Enemy
{
    /// <summary>
    /// Класс для логики противника
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyCore : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Слои, которые наносят урон")]
        private LayerMask BulletLayer;

        [SerializeField]
        [Tooltip("Ссылка на объект с событием убийства врага")]
        private GameEventScriptableObject EnemyKilledEvent;

        /// <summary>
        /// Описание врага
        /// </summary>
        public EnemyScriptableObject EnemyDescription
        {
            get
            {
                return _enemyDescription;
            }
            set
            {
                _enemyDescription = value;

                _meshRenderer.material.color = value.EnemyColor;
                CurrentHealth = value.Health;
            }
        }

        /// <summary>
        /// Текущее здоровье
        /// </summary>
        public float CurrentHealth
        {
            get
            {
                return _currentHealth;
            }
            private set
            {
                _currentHealth = value;
                OnHealthChanged();
            }
        }

        private EnemyScriptableObject _enemyDescription;
        private Rigidbody _rigidBody;
        private MeshRenderer _meshRenderer;
        private TextMeshPro _hpBarTextMesh;
        private NavMeshAgent _navMeshAgent;
        private float _currentHealth;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _meshRenderer = GetComponent<MeshRenderer>();
            _hpBarTextMesh = GetComponentInChildren<TextMeshPro>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void OnDisable()
        {
            //При убийства гасим действующие силы
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.angularVelocity = Vector3.zero;
        }

        private void OnCollisionEnter(Collision collision)
        {
            var otherLayer = collision.collider.gameObject.layer;

            if ((BulletLayer & 1 << otherLayer) == 1 << otherLayer)
            {
                BulletCore bullet = collision.gameObject.GetComponent<BulletCore>();
                Harm(bullet.WeaponDescription);
            }
        }

        /// <summary>
        /// Задать цель для преследования 
        /// </summary>
        /// <param name="target">Трансформ</param>
        public void SetDestination(Transform target)
        {
            _navMeshAgent.SetDestination(target.position);
        }

        /// <summary>
        /// Метод обработки попадения пули
        /// </summary>
        /// <param name="weaponDesc">Описание оружия, которым нанесли урон</param>
        private void Harm(WeaponScriptableObject weaponDesc)
        {
            CurrentHealth = _currentHealth - weaponDesc.Damage * _enemyDescription.Defence;

            //Гасим действующие силы
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.angularVelocity = Vector3.zero;
        }

        /// <summary>
        /// Обработчик изменения здоровья
        /// </summary>
        private void OnHealthChanged()
        {
            _hpBarTextMesh.text = _currentHealth.ToString();

            if(_currentHealth <= 0)
            {
                gameObject.SetActive(false);
                EnemyKilledEvent.Invoke();
                EnemyKilledEvent.TransformInvoke(transform);
            }
        }
    }
}
