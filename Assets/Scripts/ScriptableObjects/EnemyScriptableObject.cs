using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    /// <summary>
    /// Класс скриптвого объекта для противника
    /// </summary>
    [CreateAssetMenu(fileName = "NewEnemyDescription", menuName = "Game Character/New Enemy", order = 1)]
    public class EnemyScriptableObject : ScriptableObject
    {
        [Tooltip("Используемый префаб противника")]
        [SerializeField]
        private GameObject usedPrefab;

        [Tooltip("Имя противника")]
        [SerializeField]
        private string enemyName;

        [Tooltip("Начальное здоровье")]
        [SerializeField]
        [Range(50, 200)]
        private int health;

        [Tooltip("Наносимый урон")]
        [SerializeField]
        [Range(1, 10)]
        private int damage;

        [Tooltip("Уровень защиты")]
        [SerializeField]
        [Range(0f, 1f)]
        private float defence;

        [Tooltip("Скорость перемещения")]
        [SerializeField]
        [Range(0.5f, 5f)]
        private float speed;

        #region Публичные свойства
        /// <summary>
        /// Префаб противника
        /// </summary>
        public GameObject UsedPrefab
        {
            get => usedPrefab;
        }

        /// <summary>
        /// Имя противника
        /// </summary>
        public string EnemyName
        {
            get => enemyName;
        }

        /// <summary>
        /// Здоровье
        /// </summary>
        public int Health
        {
            get => health;
        }

        /// <summary>
        /// Сила атаки
        /// </summary>
        public int Damage
        {
            get => damage;
        }

        /// <summary>
        /// Защита (от 0 до 1)
        /// </summary>
        public float Defence
        {
            get => defence;
        }

        /// <summary>
        /// Скорость
        /// </summary>
        public float Speed
        {
            get => speed;
        }
        #endregion
    }
}