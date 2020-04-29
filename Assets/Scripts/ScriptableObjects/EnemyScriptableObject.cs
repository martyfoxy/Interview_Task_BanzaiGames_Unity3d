using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    /// <summary>
    /// Класс скриптвого объекта для противника
    /// </summary>
    [CreateAssetMenu(fileName = "NewEnemyDescription", menuName = "Game Character/New Enemy", order = 1)]
    public class EnemyScriptableObject : ScriptableObject
    {
        [Tooltip("Цвет противника")]
        [SerializeField]
        private Color enemyColor;

        [Tooltip("Имя противника")]
        [SerializeField]
        private string enemyName;

        [Tooltip("Начальное здоровье")]
        [SerializeField]
        [Range(50, 200)]
        private float health;

        [Tooltip("Наносимый урон")]
        [SerializeField]
        [Range(1, 10)]
        private float damage;

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
        public Color EnemyColor => enemyColor;
        

        /// <summary>
        /// Имя противника
        /// </summary>
        public string EnemyName => enemyName;
        

        /// <summary>
        /// Здоровье
        /// </summary>
        public float Health => health;
        

        /// <summary>
        /// Сила атаки
        /// </summary>
        public float Damage => damage;
        

        /// <summary>
        /// Защита (от 0 до 1)
        /// </summary>
        public float Defence => defence;
        

        /// <summary>
        /// Скорость
        /// </summary>
        public float Speed => speed;
        #endregion
    }
}