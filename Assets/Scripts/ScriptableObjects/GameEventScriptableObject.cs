using Assets.Scripts.Events;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    /// <summary>
    /// Класс скриптового объекта для игровых событий
    /// </summary>
    [CreateAssetMenu(fileName = "NewGameEvent", menuName = "Game Data/New Game Event")]
    public class GameEventScriptableObject : ScriptableObject
    {
        //Храним всех подписчиков события
        private List<GameEventListener> _listeners = new List<GameEventListener>();

        public delegate void GameEventHandler();
        public event GameEventHandler OnGameEvent;

        /// <summary>
        /// Вызвать обработчик события у всех слушателей
        /// </summary>
        public void Invoke()
        {
            //Вызываем начиная с самого последнего подписавшегося
            for (int i = _listeners.Count - 1; i >= 0; i--)
                _listeners[i].OnEventInvoked();

            OnGameEvent?.Invoke();
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