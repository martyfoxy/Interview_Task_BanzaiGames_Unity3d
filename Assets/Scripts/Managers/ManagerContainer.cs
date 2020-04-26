using Assets.Scripts.Interface;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Контейнер всех менеджеров
    /// </summary>
    public class ManagerContainer : SingletonBase<ManagerContainer>
    {
        //Словарь всех менеджеров
        private Dictionary<Type, object> _managers = new Dictionary<Type, object>();

        /// <summary>
        /// Добавить менеджер в список
        /// </summary>
        /// <param name="manager">Менеджер</param>
        public static void AddManager(BaseManager manager)
        {
            //Создаем экземпляр скриптового объекта менеджера
            var managerInstance = Instantiate(manager);

            Instance._managers.Add(managerInstance.GetType(), managerInstance);

            Debug.Log("Менеджер " + managerInstance.ToString() + " добавлен в контейнер менеджеров");

            //Вызываем Awake у добавленного менеджера
            if(managerInstance is IAwake)
            {
                (managerInstance as IAwake).OnAwake();
            }
        }

        /// <summary>
        /// Получить менеджер по его типу
        /// </summary>
        /// <typeparam name="T">Тип менеджера</typeparam>
        /// <returns>Менеджер</returns>
        public static T Get<T>()
        {
            object res;
            Instance._managers.TryGetValue(typeof(T), out res);
            return (T)res;
        }
    }
}
