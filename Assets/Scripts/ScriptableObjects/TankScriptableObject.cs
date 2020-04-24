using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    /// <summary>
    /// Класс скриптового объекта танка
    /// </summary>
    [CreateAssetMenu(fileName = "NewTankDescription", menuName = "Game Data/New Tank", order = 0)]
    public class TankScriptableObject : ScriptableObject
    {
        [SerializeField]
        private GameObject usedPrefab;

        [SerializeField]
        private string tankName;

        [SerializeField]
        [Range(100, 200)]
        private int health;

        [SerializeField]
        [Range(0f, 1f)]
        private float defence;

        [SerializeField]
        [Range(1f, 10f)]
        private float speed;

        #region Публичные свойства
        /// <summary>
        /// Префаб танка
        /// </summary>
        public GameObject UsedPrefab
        {
            get => usedPrefab;
        }

        /// <summary>
        /// Имя танка
        /// </summary>
        public string TankName
        {
            get => tankName;
        }

        /// <summary>
        /// Здоровье танка
        /// </summary>
        public int Health
        {
            get => health;
        }

        /// <summary>
        /// Защита танка (от 0 до 1)
        /// </summary>
        public float Defence
        {
            get => defence;
        }

        /// <summary>
        /// Скорость танка
        /// </summary>
        public float Speed
        {
            get => speed;
        }
        #endregion
    }
}