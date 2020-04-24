using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    /// <summary>
    /// Класс скриптвого объекта для противника
    /// </summary>
    [CreateAssetMenu(fileName = "NewEnemyDescription", menuName = "Game Data/New Enemy", order = 2)]
    public class EnemyScriptableObject : ScriptableObject
    {
        [SerializeField]
        private GameObject usedPrefab;

        [SerializeField]
        private string enemyName;

        [SerializeField]
        [Range(50, 200)]
        private int health;

        [SerializeField]
        [Range(1, 10)]
        private int damage;

        [SerializeField]
        [Range(0f, 1f)]
        private float defence;

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