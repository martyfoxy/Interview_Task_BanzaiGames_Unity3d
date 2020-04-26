using System;
using System.Collections.Generic;
using Assets.Scripts.Enemy;
using Assets.Scripts.Player;
using Assets.Scripts.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Класс менеджера создания объектов на сцене
    /// </summary>
    public class SpawnManager : SingletonBase<SpawnManager>
    {
        private TankCore _tankReference;
        private List<EnemyCore> _enemyReferences = new List<EnemyCore>();

        /// <summary>
        /// Создать на сцене танк согласно описанию
        /// </summary>
        /// <param name="description">Скриптовый объект с описанием</param>
        public void SpawnPlayerTank(TankScriptableObject description)
        {
            var go = Instantiate(description.UsedPrefab, new Vector3(), Quaternion.identity) as GameObject;
            var tankScript = go.GetComponent<TankCore>();
            tankScript.TankDescription = description;

            _tankReference = tankScript;
        }

        /// <summary>
        /// Создать на сцене противника согласно описанию
        /// </summary>
        /// <param name="description">Скриптовый объект с описанием</param>
        public void SpawnEnemy(EnemyScriptableObject description)
        {

        }
    }
}
