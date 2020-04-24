using System;
using Assets.Scripts.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Класс главного менеджера игры
    /// </summary>
    public class GameManager : SingletonBase<GameManager>
    {
        #region Публичные свойства
        public List<EnemyScriptableObject> EnemiesData => _enemiesData;
        public List<TankScriptableObject> TanksData => _tanksData;
        public List<WeaponScriptableObject> WeaponsData => _weaponsData;
        #endregion

        #region Приватные поля
        private List<EnemyScriptableObject> _enemiesData = new List<EnemyScriptableObject>();
        private List<TankScriptableObject> _tanksData = new List<TankScriptableObject>();
        private List<WeaponScriptableObject> _weaponsData = new List<WeaponScriptableObject>();
        #endregion

        void Start()
        {
            //Загружаем скриптовые объекты и сохраняем их для дальнейшего использования
            _enemiesData.AddRange(Resources.LoadAll<EnemyScriptableObject>("ScriptableObjects/Enemies").ToList());
            _tanksData.AddRange(Resources.LoadAll<TankScriptableObject>("ScriptableObjects/Tanks").ToList());
            _weaponsData.AddRange(Resources.LoadAll<WeaponScriptableObject>("ScriptableObjects/Weapons").ToList());

            Initialize();
        }

        /// <summary>
        /// Метод инициализации игры
        /// </summary>
        private void Initialize()
        {
            var tankData = _tanksData[0];

            SpawnManager.Instance.SpawnPlayerTank(tankData);
        }
    }
}