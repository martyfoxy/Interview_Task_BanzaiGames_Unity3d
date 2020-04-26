using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Enemy;
using Assets.Scripts.Interface;
using Assets.Scripts.Player;
using Assets.Scripts.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Класс менеджера создания объектов на сцене
    /// </summary>
    [CreateAssetMenu(fileName = "SpawnManager", menuName = "Game Manager/Spawn Manager", order = 1)]
    public class SpawnManager : BaseManager, IAwake
    {
        //Списки ссылок на созданные объекты
        private TankCore _tankReference;
        private List<EnemyCore> _enemyReferences = new List<EnemyCore>();

        //Списки скриптовых объектов с описанием создаваемых объектов
        private List<EnemyScriptableObject> _enemiesData = new List<EnemyScriptableObject>();
        private List<TankScriptableObject> _tanksData = new List<TankScriptableObject>();

        public void OnAwake()
        {
            //Грузим ресурсы скриптовых объектов
            _enemiesData.AddRange(Resources.LoadAll<EnemyScriptableObject>("ScriptableObjects/Enemies").ToList());
            _tanksData.AddRange(Resources.LoadAll<TankScriptableObject>("ScriptableObjects/Tanks").ToList());
        }

        /// <summary>
        /// Создать на сцене танк согласно описанию
        /// </summary>
        /// <param name="description">Скриптовый объект с описанием</param>
        public void SpawnPlayerTank(TankScriptableObject description)
        {
            var descriptionInstance = Instantiate(description);

            var go = Instantiate(description.UsedPrefab, new Vector3(), Quaternion.identity) as GameObject;
            var tankScript = go.GetComponent<TankCore>();
            tankScript.TankDescription = descriptionInstance;

            _tankReference = tankScript;
        }

        /// <summary>
        /// Создать на сцене противника согласно описанию
        /// </summary>
        /// <param name="description">Скриптовый объект с описанием</param>
        public void SpawnEnemy(EnemyScriptableObject description)
        {
            var descriptionInstance = Instantiate(description);

            var go = Instantiate(description.UsedPrefab, new Vector3(), Quaternion.identity) as GameObject;
            var enemyScript = go.GetComponent<EnemyCore>();
            enemyScript.EnemyDescription = descriptionInstance;

            _enemyReferences.Add(enemyScript);
        }
    }
}
