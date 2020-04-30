using Assets.Scripts.Interface;
using Assets.Scripts.ScriptableObjects;
using System;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Класс менеджера для управления врагами
    /// </summary>
    [CreateAssetMenu(fileName = "EnemyControllingManager", menuName = "Game Manager/Enemy Controlling Manager")]
    public class EnemyControllingManager : BaseManager, IAwake, IFixedUpdate
    {
        [Header("Ссылки на объекты с собятиями")]
        [SerializeField]
        [Tooltip("Ссылка на объект события создания игрока")]
        private GameEventScriptableObject PlayerSpawnedEvent;

        private Transform _target;

        public void OnAwake()
        {
            UpdateManager.Register(this);

            PlayerSpawnedEvent.OnTransformEvent += PlayerSpawnedHandler;
        }

        private void OnDisable()
        {
            PlayerSpawnedEvent.OnTransformEvent -= PlayerSpawnedHandler;
        }

        public void OnFixedUpdate()
        {
            //TODO: Не нравится обращение к контейнеру каждый кадр
            if(_target != null)
            {
                ManagerContainer.Get<SpawnManager>().SpawnedEnemies.ForEach(x =>
                {
                    if (x.gameObject.activeInHierarchy)
                        x.SetDestination(_target);
                });
            }
        }

        /// <summary>
        /// Обработчик события создания игрока, в качестве аргумента получает его Transform
        /// </summary>
        private void PlayerSpawnedHandler(TransformArgs args)
        {
            _target = args.TransformArg;
        }
    }
}
