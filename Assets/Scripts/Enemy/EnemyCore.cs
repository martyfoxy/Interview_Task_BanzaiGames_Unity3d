using Assets.Scripts.Player;
using Assets.Scripts.ScriptableObjects;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    /// <summary>
    /// Класс для логики противника
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(MeshRenderer))]
    public class EnemyCore : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Слои, которые наносят урон")]
        private LayerMask BulletLayer;

        [SerializeField]
        [Tooltip("Ссылка на объект с событием убийства врага")]
        private GameEventScriptableObject EnemyKilledEvent;

        //Используемое описание врага
        private EnemyScriptableObject _enemyDescription;

        //Ссылки на компоненты врага
        private Rigidbody _rigidBody;
        private MeshRenderer _meshRenderer;
        private TextMeshPro _hpBarTextMesh;

        //Локальные переменные
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
        private float _currentHealth;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _meshRenderer = GetComponent<MeshRenderer>();
            _hpBarTextMesh = GetComponentInChildren<TextMeshPro>();
        }

        private void OnDisable()
        {
            //При убийства гасим силу
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.angularVelocity = Vector3.zero;
        }

        private void OnCollisionEnter(Collision collision)
        {
            var otherLayer = collision.collider.gameObject.layer;

            if ((BulletLayer & 1 << otherLayer) == 1 << otherLayer)
            {
                BulletCore bullet = collision.gameObject.GetComponent<BulletCore>();
                Harm(bullet.GetDescription());
            }
        }

        /// <summary>
        /// Задать описание врага
        /// </summary>
        /// <param name="enemyDesc">Описание врага</param>
        public void SetDescription(EnemyScriptableObject enemyDesc)
        {
            _enemyDescription = enemyDesc;

            _meshRenderer.material.color = enemyDesc.EnemyColor;
            CurrentHealth = enemyDesc.Health;
        }

        /// <summary>
        /// Получить описание врага
        /// </summary>
        /// <returns>Описание врага</returns>
        public EnemyScriptableObject GetDescription()
        {
            return _enemyDescription;
        }

        /// <summary>
        /// Метод обработки попадения пули
        /// </summary>
        /// <param name="weaponDesc">Описание оружия, которым нанесли урон</param>
        private void Harm(WeaponScriptableObject weaponDesc)
        {
            CurrentHealth = _currentHealth - weaponDesc.Damage * _enemyDescription.Defence;
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
            }
        }
    }
}
