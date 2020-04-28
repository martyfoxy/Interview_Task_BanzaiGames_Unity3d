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
    [CreateAssetMenu(fileName = "SpawnManager", menuName = "Game Manager/Spawn Manager")]
    public class SpawnManager : BaseManager, IAwake
    {
        [Header("Ссылки на объекты с событиями")]
        [Tooltip("Ссылки на объект события создания игрока")]
        public GameEventScriptableObject PlayerSpawnedEvent;

        //Ссылка на танк
        public TankCore TankReference => _tankReference;
        private TankCore _tankReference;

        //Ссылки на созданных противников
        public List<EnemyCore> EnemiesReference => _enemyReferences;
        private List<EnemyCore> _enemyReferences = new List<EnemyCore>();

        //Списки скриптовых объектов с описанием создаваемых персонажей
        public List<EnemyScriptableObject> EnemiedData => _enemiesData;
        private List<EnemyScriptableObject> _enemiesData = new List<EnemyScriptableObject>();

        public List<TankScriptableObject> TanksData => TanksData;
        private List<TankScriptableObject> _tanksData = new List<TankScriptableObject>();

        public void OnAwake()
        {
            //Грузим ресурсы скриптовых объектов
            _enemiesData.AddRange(Resources.LoadAll<EnemyScriptableObject>("ScriptableObjects/Enemies").ToList());
            _tanksData.AddRange(Resources.LoadAll<TankScriptableObject>("ScriptableObjects/Tanks").ToList());
        }

        /// <summary>
        /// Тестовый метод. Создает на сцене танк со случайным описанием
        /// </summary>
        public void SpawnAnyTank()
        {
            var randomDesc = _tanksData[Random.Range(0, _tanksData.Count - 1)];

            SpawnPlayerTank(randomDesc);
        }

        /// <summary>
        /// Создать на сцене танк согласно описанию
        /// </summary>
        /// <param name="description">Скриптовый объект с описанием</param>
        public TankCore SpawnPlayerTank(TankScriptableObject description)
        {
            var descriptionInstance = Instantiate(description);

            var go = Instantiate(description.UsedPrefab, new Vector3(), Quaternion.identity) as GameObject;
            var tankCore = go.GetComponent<TankCore>();
            tankCore.SetDescription(descriptionInstance);

            _tankReference = tankCore;

            PlayerSpawnedEvent.Invoke();

            return tankCore;
        }

        /// <summary>
        /// Создать на сцене противника согласно описанию
        /// </summary>
        /// <param name="description">Скриптовый объект с описанием</param>
        public EnemyCore SpawnEnemy(EnemyScriptableObject description)
        {
            var descriptionInstance = Instantiate(description);

            var go = Instantiate(description.UsedPrefab, new Vector3(), Quaternion.identity) as GameObject;
            var enemyCore = go.GetComponent<EnemyCore>();
            enemyCore.SetDescription(descriptionInstance);

            _enemyReferences.Add(enemyCore);

            return enemyCore;
        }
    }
}
