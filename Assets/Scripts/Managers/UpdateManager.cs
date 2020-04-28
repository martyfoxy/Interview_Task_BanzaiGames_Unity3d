using Assets.Scripts.Interface;
using Assets.Scripts.Managers.Components;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Менеджер вызывающий методы жизненного цикла MonoBehaviour для всех зарегистрированных менеджеров
    /// </summary>
    [CreateAssetMenu(fileName = "UpdateManager", menuName = "Game Manager/Update Manager", order = 0)]
    public class UpdateManager : BaseManager, IAwake
    {
        private List<IUpdate> _updateNeedy = new List<IUpdate>();
        private List<IFixedUpdate> _fixedUpdateNeedy = new List<IFixedUpdate>();

        /// <summary>
        /// Во время Awake создаем фасадный компонент, выполняющий MonoBehaviour методы
        /// </summary>
        public void OnAwake()
        {
            GameObject.Find("Bootstrap").AddComponent<UpdateManagerComponent>().Setup(this);
        }

        /// <summary>
        /// Зарегистрировать менеджер для выполнения методов жизненного цикла MonoBehaviour
        /// </summary>
        /// <param name="updatable">Менеджер</param>
        public static void Register(BaseManager updatable)
        {
            //Это статический метод, поэтому сначала вытаскиваем из контейнера экземпляр менеджера
            var updManager = ManagerContainer.Get<UpdateManager>();

            if (updatable is IUpdate)
            {
                if(!updManager._updateNeedy.Contains(updatable as IUpdate))
                    updManager._updateNeedy.Add(updatable as IUpdate);
            }

            if (updatable is IFixedUpdate)
            {
                if (!updManager._fixedUpdateNeedy.Contains(updatable as IFixedUpdate))
                    updManager._fixedUpdateNeedy.Add(updatable as IFixedUpdate);
            }
        }

        /// <summary>
        /// Удалить менеджер из списка
        /// </summary>
        /// <param name="updatable">Менеджер</param>
        public static void Remove(BaseManager updatable)
        {
            //Это статический метод, поэтому сначала вытаскиваем из контейнера экземпляр менеджера
            var updManager = ManagerContainer.Get<UpdateManager>();

            if(updatable is IUpdate)
            {
                if(updManager._updateNeedy.Contains(updatable as IUpdate))
                    updManager._updateNeedy.Remove(updatable as IUpdate);
            }
            
            if (updatable is IFixedUpdate)
            {
                if(updManager._fixedUpdateNeedy.Contains(updatable as IFixedUpdate))
                    updManager._fixedUpdateNeedy.Remove(updatable as IFixedUpdate);
            }
        }

        /// <summary>
        /// Вызвать метод OnUpdate у всех добавленных объектов реализующих IUpdate
        /// </summary>
        public void DoUpdate()
        {
            for (int i = 0; i < _updateNeedy.Count; i++)
                _updateNeedy[i].OnUpdate();
        }

        /// <summary>
        /// Вызвать метод OnFixedUpdate у всех добавленных объектов реализующих IFixedUpdate
        /// </summary>
        public void DoFixedUpdate()
        {
            for (int i = 0; i < _fixedUpdateNeedy.Count; i++)
                _fixedUpdateNeedy[i].OnFixedUpdate();
        }
    }
}
