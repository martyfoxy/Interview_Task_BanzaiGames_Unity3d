using Assets.Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    //TODO: Избавиться и сделать менеджер перемещения пуль
    public class BulletCore : MonoBehaviour
    {
        //Описание оружия из которого была выстрелена пуля
        private WeaponScriptableObject _weaponDescription;

        //Список компонентов
        private Rigidbody _rigidBody;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _rigidBody.AddForce(transform.forward * 1000f, ForceMode.Acceleration);
        }

        /// <summary>
        /// Задать описание оружия
        /// </summary>
        /// <param name="weaponDesc">Описание оружия</param>
        public void SetDescription(WeaponScriptableObject weaponDesc)
        {
            _weaponDescription = weaponDesc;
        }
    }
}
