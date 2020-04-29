using Assets.Scripts.Interface;
using Assets.Scripts.Player;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.ScriptableObjects.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Класс менеджера оружия
    /// </summary>
    [CreateAssetMenu(fileName = "WeaponManager", menuName = "Game Manager/Weapon Manager")]
    public class WeaponManager : BaseManager, IAwake, IFixedUpdate
    {
        [Header("Настройки пуль")]
        [Tooltip("Количество возможных пуль на сцене")]
        public IntReference BulletCount;

        [Tooltip("Максимальная координата пули, после которой она скрывается")]
        public FloatReference BulletMaximumPosition;

        [Tooltip("Префаб пули")]
        public GameObject BulletPrefab;

        [Header("Ссылки на объекты событий")]
        [Tooltip("Ссылка на событие изменения выбранного оружия")]
        public GameEventScriptableObject WeaponChangedEvent;

        [Header("Ссылки на объекты с событиями ввода")]
        [Tooltip("Ссылка на событие нажатия кнопки следующего оружия")]
        public GameEventScriptableObject NextWeaponButtonEvent;
        [Tooltip("Ссылка на событие нажатия кнопки предыдущего оружия ")]
        public GameEventScriptableObject PreviosWeaponButtonEvent;
        [Tooltip("Ссылка на событие нажатия кнопки выстрела")]
        public GameEventScriptableObject FireButtonEvent;

        //Список скриптовых объектов с описанием видов оружия
        public List<WeaponScriptableObject> WeaponData => _weaponData;
        private List<WeaponScriptableObject> _weaponData = new List<WeaponScriptableObject>();

        //Текущее выбранное оружие
        public WeaponScriptableObject CurrentWeapon => _weaponData[_currentWeaponIndex];
        private int _currentWeaponIndex;

        //Object pool
        private List<BulletCore> _bulletPool = new List<BulletCore>();

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
            FireButtonEvent.OnGameEvent += FireBulletHandler;

            //Object pooling
            for (int i = 0; i < BulletCount; i++)
            {
                GameObject bullet = Instantiate(BulletPrefab);
                bullet.SetActive(false);
                _bulletPool.Add(bullet.GetComponent<BulletCore>());
            }
        }

        private void OnDisable()
        {
            //Отписываемся
            NextWeaponButtonEvent.OnGameEvent -= NextWeaponHandler;
            PreviosWeaponButtonEvent.OnGameEvent -= PreviousWeaponHandler;
        }

        public void OnFixedUpdate()
        {
            for (int i = 0; i < BulletCount; i++)
            {
                if (_bulletPool[i].gameObject.activeInHierarchy)
                {
                    var tr = _bulletPool[i].transform;

                    //Если позиция пули выходит за пределы уровня, то скрываем
                    if (Mathf.Abs(tr.position.x) > BulletMaximumPosition || Mathf.Abs(tr.position.z) > BulletMaximumPosition)
                        _bulletPool[i].gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки следующего оружия
        /// </summary>
        private void NextWeaponHandler()
        {
            if(_currentWeaponIndex + 1 <= _weaponData.Count - 1)
                _currentWeaponIndex++;
            else
                _currentWeaponIndex = 0;

            WeaponChangedEvent?.Invoke();
        }

        /// <summary>
        /// Обработчик нажатия кнопки предыдущего оружия
        /// </summary>
        private void PreviousWeaponHandler()
        {
            if (_currentWeaponIndex - 1 >= 0)
                _currentWeaponIndex--;
            else
                _currentWeaponIndex = _weaponData.Count - 1;

            WeaponChangedEvent?.Invoke();
        }

        /// <summary>
        /// Вытащить из пула неактивную пулю
        /// </summary>
        /// <returns>Неактивная пуля</returns>
        private BulletCore GetAvailableBullet()
        {
            for (int i = 0; i < BulletCount; i++)
            {
                if (_bulletPool[i].gameObject.activeInHierarchy == false)
                    return _bulletPool[i];
            }

            return null;
        }

        /// <summary>
        /// Обработчик нажатия на кнопку выстрела
        /// </summary>
        private void FireBulletHandler()
        {
            var player = ManagerContainer.Get<SpawnManager>().TankReference;
            if(player != null)
            {
                var newBullet = GetAvailableBullet();
                if (newBullet != null)
                {
                    newBullet.SetDescription(CurrentWeapon);
                    newBullet.transform.position = player.transform.position;
                    newBullet.transform.rotation = player.transform.rotation;
                    newBullet.gameObject.SetActive(true);
                }
                else
                    Debug.LogError("Слишком много пуль на сцене");
            }
        }
    }
}
