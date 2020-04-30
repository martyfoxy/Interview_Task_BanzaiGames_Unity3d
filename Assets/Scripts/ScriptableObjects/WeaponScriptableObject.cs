using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    /// <summary>
    /// Класс скриптового объекта для оружия игрока
    /// </summary>
    [CreateAssetMenu(fileName = "NewWeaponDescription", menuName = "Game Data/New Weapon", order = 1)]
    public class WeaponScriptableObject : ScriptableObject
    {
        [Tooltip("Цвет оружия")]
        [SerializeField]
        private Color weaponColor;

        [Tooltip("Имя оружия")]
        [SerializeField]
        private string weaponName;

        [Tooltip("Наносимый урон")]
        [SerializeField]
        [Range(10, 50)]
        private float damage;

        [Tooltip("Тип оружия")]
        [SerializeField]
        private WeaponTypeEnum weaponType;

        #region Публичные свойства
        public Color WeaponColor => weaponColor;
        /// <summary>
        /// Название оружия
        /// </summary>
        public string WeaponName => weaponName;

        /// <summary>
        /// Сила урона
        /// </summary>
        public float Damage => damage;

        /// <summary>
        /// Тип оружия
        /// </summary>
        public WeaponTypeEnum WeaponType => weaponType;
        #endregion
    }

    /// <summary>
    /// Перечисление типов оружия
    /// </summary>
    public enum WeaponTypeEnum
    {
        Canon,
        MachineGun
    }
}