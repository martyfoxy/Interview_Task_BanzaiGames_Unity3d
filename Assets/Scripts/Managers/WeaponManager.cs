using Assets.Scripts.Interface;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.ScriptableObjects.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Класс менеджера оружия
    /// </summary>
    [CreateAssetMenu(fileName = "WeaponManager", menuName = "Game Manager/Weapon Manager")]
    public class WeaponManager : BaseManager, IAwake
    {
        [Header("Настройки пуль")]
        [Tooltip("Количество возможных пуль на сцене")]
        public IntReference BulletCount;

        [Header("Ссылки на объекты событий")]
        [Tooltip("Ссылка на событие изменения выбранного оружия")]
        public GameEventScriptableObject WeaponChangedEvent;

        [Header("Ссылки на объекты с событиями ввода")]
        [Tooltip("Ссылка на событие нажатия кнопки следующего оружия")]
        public GameEventScriptableObject NextWeaponButtonEvent;
        [Tooltip("Ссылка на событие нажатия кнопки предыдущего оружия ")]
        public GameEventScriptableObject PreviosWeaponButtonEvent;

        //Список скриптовых объектов с описанием видов оружия
        public List<WeaponScriptableObject> WeaponData => _weaponData;
        private List<WeaponScriptableObject> _weaponData = new List<WeaponScriptableObject>();

        //Текущее выбранное оружие
        public WeaponScriptableObject CurrentWeapon => _weaponData[_currentWeaponIndex];
        private int _currentWeaponIndex;

        //Object pool
        private List<GameObject> _objectPool = new List<GameObject>();

        public void OnAwake()
        {
            UpdateManager.Register(this);

            _weaponData.AddRange(Resources.LoadAll<WeaponScriptableObject>("ScriptableObjects/Weapons").ToList());

            if (_weaponData.Count > 0)
                _currentWeaponIndex = 0;
            else
                throw new Exception("Не загружено ни одного описания оружия");

            //Подписываемся на нажатия
            NextWeaponButtonEvent.OnGameEvent += NextWeaponHandler;
            PreviosWeaponButtonEvent.OnGameEvent += PreviousWeaponHandler;
        }

        private void OnDisable()
        {
            //Отписываемся
            NextWeaponButtonEvent.OnGameEvent -= NextWeaponHandler;
            PreviosWeaponButtonEvent.OnGameEvent -= PreviousWeaponHandler;
        }

        /// <summary>
        /// Обработчик нажатия кнопки следующего оружия
        /// </summary>
        private void NextWeaponHandler()
        {
            if(_currentWeaponIndex + 1 <= _weaponData.Count - 1)
            {
                _currentWeaponIndex++;

                WeaponChangedEvent?.Invoke();
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки предыдущего оружия
        /// </summary>
        private void PreviousWeaponHandler()
        {
            if (_currentWeaponIndex - 1 >= 0)
            {
                _currentWeaponIndex--;

                WeaponChangedEvent?.Invoke();
            }
        }


    }
}
