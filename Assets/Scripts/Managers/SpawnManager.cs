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
        [Tooltip("Ссылка на объект события убийства игрока")]
        public GameEventScriptableObject PlayerKilledEvent;
        [Tooltip("Ссылка на объект события убийства врага")]
        public GameEventScriptableObject EnemyKilledEvent;

        [Header("Используемые префабы")]
        [SerializeField]
        [Tooltip("Префаб врага")]
        private GameObject EnemyPrefab;
        [SerializeField]
        [Tooltip("Префаб танка")]
        private GameObject TankPrefab;

        /// <summary>
        /// Ссылка на танк
        /// </summary>
        public TankCore SpawnedTank => _tankPool;
        
        /// <summary>
        /// Ссылки на созданных противников
        /// </summary>
        public List<EnemyCore> SpawnedEnemies => _enemyPool;

        private TankCore _tankPool;
        private List<EnemyCore> _enemyPool = new List<EnemyCore>();
        private List<EnemyScriptableObject> _enemiesData = new List<EnemyScriptableObject>();
        private List<TankScriptableObject> _tanksData = new List<TankScriptableObject>();

        public void OnAwake()
        {
            //Грузим ресурсы скриптовых объектов
            _enemiesData.AddRange(Resources.LoadAll<EnemyScriptableObject>("ScriptableObjects/Enemies").ToList());
            _tanksData.AddRange(Resources.LoadAll<TankScriptableObject>("ScriptableObjects/Tanks").ToList());

            //Object pooling
            for (int i = 0; i < EnemyCount; i++)
            {
                GameObject enemy = Instantiate(EnemyPrefab);
                enemy.SetActive(false);
                _enemyPool.Add(enemy.GetComponent<EnemyCore>());
            }

            GameObject tank = Instantiate(TankPrefab);
            tank.SetActive(false);
            _tankPool = tank.GetComponent<TankCore>();

            //Подписываемся на события
            EnemyKilledEvent.OnGameEvent += SpawnRandomEnemy;
            PlayerKilledEvent.OnGameEvent += SpawnRandomTank;
        }

        private void OnDisable()
        {
            EnemyKilledEvent.OnGameEvent -= SpawnRandomEnemy;
            PlayerKilledEvent.OnGameEvent -= SpawnRandomTank;
        }

        /// <summary>
        /// Создает на сцене танк со случайным описанием
        /// </summary>
        public void SpawnRandomTank()
        {
            var randomDesc = _tanksData[Random.Range(0, _tanksData.Count)];

            SpawnPlayerTank(randomDesc);
        }

        /// <summary>
        /// Создать на сцене противника со случайным описанием
        /// </summary>
        public void SpawnRandomEnemy()
        {
            //Берем случайное описание врага
            var randomDesc = _enemiesData[Random.Range(0, _enemiesData.Count)];

            //Копируем описание
            var descriptionInstance = Instantiate(randomDesc);

            EnemyCore newEnemy = GetAvailableEnemy();
            if (newEnemy != null)
            {
                newEnemy.transform.position = new Vector3(Random.Range(-MaximumSpawnPosition, MaximumSpawnPosition), 1.5f, Random.Range(-MaximumSpawnPosition, MaximumSpawnPosition));
                newEnemy.EnemyDescription = descriptionInstance;
                newEnemy.gameObject.SetActive(true);
            }
            else
                Debug.LogError("Слишком много врагов на сцене");
        }

        /// <summary>
        /// Создать на сцене танк согласно описанию
        /// </summary>
        /// <param name="description">Скриптовый объект с описанием</param>
        private void SpawnPlayerTank(TankScriptableObject description)
        {
            //Копируем описание
            var descriptionInstance = Instantiate(description);

            TankCore newTank = _tankPool;
            if (_tankPool.gameObject.activeInHierarchy == false)
            {
                newTank.transform.position = new Vector3(Random.Range(-MaximumSpawnPosition, MaximumSpawnPosition), 1.5f, Random.Range(-MaximumSpawnPosition, MaximumSpawnPosition));
                newTank.TankDescription = descriptionInstance;
                newTank.gameObject.SetActive(true);

                PlayerSpawnedEvent.Invoke();
                PlayerSpawnedEvent.TransformInvoke(newTank.transform);
            }
            else
                Debug.Log("Танк уже создан на сцене");
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
