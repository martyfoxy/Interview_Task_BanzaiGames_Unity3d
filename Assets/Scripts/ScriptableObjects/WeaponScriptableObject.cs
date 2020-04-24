using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    /// <summary>
    /// Класс скриптового объекта для оружия игрока
    /// </summary>
    [CreateAssetMenu(fileName = "NewWeaponDescription", menuName = "Game Data/New Weapon", order = 1)]
    public class WeaponScriptableObject : ScriptableObject
    {
        [SerializeField]
        private string weaponName;

        [SerializeField]
        [Range(10, 50)]
        private int damage;

        /*[SerializeField]
        [Range(1f, 5f)]
        private float hitRange;*/

        [SerializeField]
        [Range(0f, 3f)]
        private float fireDelay;

        [SerializeField]
        private WeaponTypeEnum weaponType;

        [SerializeField]
        [Range(0f, 1f)]
        private float recoil;

        #region Публичные свойства
        /// <summary>
        /// Название оружия
        /// </summary>
        public string WeaponName
        {
            get => weaponName;
        }

        /// <summary>
        /// Сила урона
        /// </summary>
        public int Damage
        {
            get => damage;
        }

        /// <summary>
        /// Задержка между выстрелами
        /// </summary>
        public float FireDelay
        {
            get => fireDelay;
        }

        /// <summary>
        /// Тип оружия
        /// </summary>
        public WeaponTypeEnum WeaponType
        {
            get => weaponType;
        }

        /// <summary>
        /// Сила отдачи
        /// </summary>
        public float Recoil
        {
            get => recoil;
        }
        #endregion
    }

    /// <summary>
    /// Перечисление возможных типов оружия игрока
    /// </summary>
    public enum WeaponTypeEnum
    {
        TankGun,
        MachineGun
    };
}