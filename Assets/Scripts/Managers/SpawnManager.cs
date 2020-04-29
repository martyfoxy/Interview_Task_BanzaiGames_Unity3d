using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Enemy;
using Assets.Scripts.Interface;
using Assets.Scripts.Player;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.ScriptableObjects.Variables;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Класс менеджера создания объектов на сцене
    /// </summary>
    [CreateAssetMenu(fileName = "SpawnManager", menuName = "Game Manager/Spawn Manager")]
    public class SpawnManager : BaseManager, IAwake
    {
        [Header("Настройки создания")]
        [Tooltip("Максимальное количество врагов на сцене")]
        public IntReference EnemyCount;

        [Tooltip("Максимальная позиция по X и Z, где могут создаваться враги")]
        public FloatReference MaximumSpawnPosition;

        [Header("Ссылки на объекты с событиями")]
        [Tooltip("Ссылка на объект события создания игрока")]
        public GameEventScriptableObject PlayerSpawnedEvent;

        [Tooltip("Ссылка на объект события убийства врага")]
        public GameEventScriptableObject EnemyKilledEvent;

        //Ссылка на танк
        public TankCore TankReference => _tankReference;
        private TankCore _tankReference;

        //Префаб противника
        private GameObject _enemyPrefab;

        //Ссылки на созданных противников
        private List<EnemyCore> _enemyPool = new List<EnemyCore>();

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

            //Грузим префабы
            _enemyPrefab = Resources.Load<GameObject>("Prefabs/EnemyPrefab");

            //Object pooling
            for (int i = 0; i < EnemyCount; i++)
            {
                GameObject enemy = Instantiate(_enemyPrefab);
                enemy.SetActive(false);
                _enemyPool.Add(enemy.GetComponent<EnemyCore>());
            }

            //Подписываемся на событие убийства врага, чтобы пересоздать его
            EnemyKilledEvent.OnGameEvent += SpawnRandomEnemy;
        }

        private void OnDisable()
        {
            EnemyKilledEvent.OnGameEvent -= SpawnRandomEnemy;
        }

        /// <summary>
        /// Тестовый метод. Создает на сцене танк со случайным описанием
        /// </summary>
        public void SpawnAnyTank()
        {
            var randomDesc = _tanksData[Random.Range(0, _tanksData.Count)];

            SpawnPlayerTank(randomDesc);
        }

        /// <summary>
        /// Создать на сцене противника согласно описанию
        /// </summary>
        /// <param name="description">Скриптовый объект с описанием</param>
        public void SpawnRandomEnemy()
        {
            //Берем случайное описание врага
            var randomDesc = _enemiesData[Random.Range(0, _enemiesData.Count)];

            //Копируем описание
            var descriptionInstance = Instantiate(randomDesc);

            EnemyCore newEnemy = GetAvailableEnemy();
            if (newEnemy != null)
            {
                newEnemy.SetDescription(descriptionInstance);
                newEnemy.transform.position = new Vector3(Random.Range(-MaximumSpawnPosition, MaximumSpawnPosition), 1.5f, Random.Range(-MaximumSpawnPosition, MaximumSpawnPosition));
                newEnemy.gameObject.SetActive(true);
            }
            else
                Debug.LogError("Слишком много врагов на сцене");
        }

        /// <summary>
        /// Создать на сцене танк согласно описанию
        /// </summary>
        /// <param name="description">Скриптовый объект с описанием</param>
        private TankCore SpawnPlayerTank(TankScriptableObject description)
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
        /// Вытащить из пула неактивного врага
        /// </summary>
        /// <returns></returns>
        private EnemyCore GetAvailableEnemy()
        {
            for (int i = 0; i < EnemyCount; i++)
            {
                if (_enemyPool[i].gameObject.activeInHierarchy == false)
                    return _enemyPool[i];
            }

            return null;
        }
    }
}
