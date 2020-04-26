using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Events
{
    /// <summary>
    /// Скриптовый объект для игровых событий
    /// </summary>
    [CreateAssetMenu(fileName = "NewGameEvent", menuName = "Game Data/New Game Event", order = 2)]
    public class GameEventScriptableObject : ScriptableObject
    {
        //Храним всех подписчиков события
        private List<GameEventListener> _listeners = new List<GameEventListener>();

        /// <summary>
        /// Вызвать обработчик события у всех слушателей
        /// </summary>
        public void Invoke()
        {
            //Вызываем начиная с самого последнего подписавшегося
            for (int i = _listeners.Count - 1; i >= 0; i++)
                _listeners[i].OnEventInvoked();
        }

        /// <summary>
        /// Подписать слушателя на данное событие
        /// </summary>
        /// <param name="listener">Слушатель</param>
        public void Subscribe(GameEventListener listener)
        {
            if (!_listeners.Contains(listener))
                _listeners.Add(listener);
        }

        /// <summary>
        /// Отписать слушателя от данного события
        /// </summary>
        /// <param name="listener">Слушатель</param>
        public void Unsubscribe(GameEventListener listener)
        {
            if (_listeners.Contains(listener))
                _listeners.Remove(listener);
        }
    }
}