using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    /// <summary>
    /// Класс скриптового объекта танка
    /// </summary>
    [CreateAssetMenu(fileName = "NewTankDescription", menuName = "Game Data/New Tank")]
    public class TankScriptableObject : ScriptableObject
    {
        [Tooltip("Цвет танка")]
        [SerializeField]
        private Color tankColor;

        [Tooltip("Имя танка")]
        [SerializeField]
        private string tankName;

        [Tooltip("Начальное здоровье танка")]
        [SerializeField]
        [Range(100, 200)]
        private float health;

        [Tooltip("Уровень защиты")]
        [SerializeField]
        [Range(0f, 1f)]
        private float defence;

        [Tooltip("Скорость перемещения")]
        [SerializeField]
        [Range(1f, 10f)]
        private float speed;

        #region Публичные свойства
        /// <summary>
        /// Цвет танка
        /// </summary>
        public Color TankColor  => tankColor;
        
        /// <summary>
        /// Имя танка
        /// </summary>
        public string TankName => tankName;
        
        /// <summary>
        /// Здоровье танка
        /// </summary>
        public float Health  => health;
        
        /// <summary>
        /// Защита танка (от 0 до 1)
        /// </summary>
        public float Defence  => defence;
        
        /// <summary>
        /// Скорость танка
        /// </summary>
        public float Speed => speed;
        #endregion
    }
}