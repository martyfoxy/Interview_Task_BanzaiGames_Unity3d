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
        [SerializeField]
        [Tooltip("Количество возможных пуль на сцене")]
        private IntReference BulletCount;
        [SerializeField]
        [Tooltip("Максимальная координата пули, после которой она скрывается")]
        private FloatReference BulletMaximumPosition;
        [SerializeField]
        [Tooltip("Префаб пули")]
        private GameObject BulletPrefab;

        [Header("Ссылки на объекты событий")]
        [SerializeField]
        [Tooltip("Ссылка на событие изменения выбранного оружия")]
        private GameEventScriptableObject WeaponChangedEvent;
        [SerializeField]
        [Tooltip("Ссылка на событие создания игрока")]
        private GameEventScriptableObject PlayerSpawnedEvent;

        [Header("Ссылки на объекты с событиями ввода")]
        [SerializeField]
        [Tooltip("Ссылка на событие нажатия кнопки следующего оружия")]
        private GameEventScriptableObject NextWeaponButtonEvent;
        [SerializeField]
        [Tooltip("Ссылка на событие нажатия кнопки предыдущего оружия ")]
        private GameEventScriptableObject PreviosWeaponButtonEvent;
        [SerializeField]
        [Tooltip("Ссылка на событие нажатия кнопки выстрела")]
        private GameEventScriptableObject FireButtonEvent;

        /// <summary>
        /// Текущее выбранное оружие
        /// </summary>
        public WeaponScriptableObject CurrentWeapon => _weaponData[_currentWeaponIndex];

        private List<WeaponScriptableObject> _weaponData = new List<WeaponScriptableObject>();
        private int _currentWeaponIndex;
        private List<BulletCore> _bulletPool = new List<BulletCore>();
        private Transform _tankTransform;

        public void OnAwake()
        {
            UpdateManager.Register(this);

            _weaponData.AddRange(Resources.LoadAll<WeaponScriptableObject>("ScriptableObjects/Weapons").ToList());

            if (_weaponData.Count > 0)
                _currentWeaponIndex = 0;
            else
                throw new Exception("Не загружено ни одного описания оружия");

            //Object pooling
            for (int i = 0; i < BulletCount; i++)
            {
                GameObject bullet = Instantiate(BulletPrefab);
                bullet.SetActive(false);
                _bulletPool.Add(bullet.GetComponent<BulletCore>());
            }

            //Подписываемся на нажатия
            NextWeaponButtonEvent.OnGameEvent += NextWeaponHandler;
            PreviosWeaponButtonEvent.OnGameEvent += PreviousWeaponHandler;
            FireButtonEvent.OnGameEvent += FireBulletHandler;
            PlayerSpawnedEvent.OnTransformEvent += PlayerTransformHandler;
        }

        private void OnDisable()
        {
            //Отписываемся
            NextWeaponButtonEvent.OnGameEvent -= NextWeaponHandler;
            PreviosWeaponButtonEvent.OnGameEvent -= PreviousWeaponHandler;
            FireButtonEvent.OnGameEvent -= FireBulletHandler;
            PlayerSpawnedEvent.OnTransformEvent -= PlayerTransformHandler;
        }
        public void OnFixedUpdate()
        {
            //Если позиция пули выходит за пределы уровня, то скрываем
            for (int i = 0; i < BulletCount; i++)
            {
                if (_bulletPool[i].gameObject.activeInHierarchy)
                {
                    var tr = _bulletPool[i].transform;

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

            WeaponChangedEvent.Invoke();
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

            WeaponChangedEvent.Invoke();
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
        /// Обработчик события создания игрока, в качестве аргумента получаего его Transform
        /// </summary>
        /// <param name="args"></param>
        private void PlayerTransformHandler(TransformArgs args)
        {
            _tankTransform = args.TransformArg;
        }

        /// <summary>
        /// Обработчик нажатия на кнопку выстрела
        /// </summary>
        private void FireBulletHandler()
        {
            if(_tankTransform != null)
            {
                var newBullet = GetAvailableBullet();
                if (newBullet != null)
                {
                    newBullet.WeaponDescription = CurrentWeapon;
                    newBullet.transform.position = _tankTransform.position;
                    newBullet.transform.rotation = _tankTransform.rotation;
                    newBullet.gameObject.SetActive(true);
                }
                else
                    Debug.LogError("Слишком много пуль на сцене");
            }
        }
    }
}
