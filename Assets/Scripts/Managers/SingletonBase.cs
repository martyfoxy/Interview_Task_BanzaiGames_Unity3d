using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Абстрактный класс всех классов, которым необходим синглтон
    /// </summary>
    /// <typeparam name="T">Класс</typeparam>
    abstract public class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static bool _shuttingDown = false;

        public static T Instance
        {
            get
            {
                if(_instance == null)
                {
                    //На всякий случай делаем проверку, чтобы не было обращения в момент выхода или уничтожения
                    if(_shuttingDown)
                    {
                        Debug.LogError("Синглтон класса " + typeof(T) + " уже уничтожен");
                        return null;
                    }

                    //Пытаемся найти на сцене экземпляр класса
                    _instance = FindObjectOfType<T>();

                    if(_instance == null)
                    {
                        //Создаем экземпляр сами
                        var go = new GameObject();
                        _instance = go.AddComponent<T>();
                        go.name = typeof(T).ToString() + "(Singleton)";

                        Debug.Log("Синглтон класса " + typeof(T).ToString() + " создан!");

                        //При смене сцены не удаляем экземпляр
                        DontDestroyOnLoad(go);
                    }
                }

                return _instance;
            }
        }

        private void OnApplicationQuit()
        {
            _shuttingDown = true;
        }

        private void OnDestroy()
        {
            _shuttingDown = true;
        }
    }
}
