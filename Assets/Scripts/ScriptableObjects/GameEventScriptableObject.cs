using Assets.Scripts.Events;
using Assets.Scripts.Player;
using System;
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
        public delegate void TransformEventHandler(TransformArgs args);

        public event GameEventHandler OnGameEvent;
        public event TransformEventHandler OnTransformEvent;

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
        /// Вызвать событие и передать Transform в качестве параметра
        /// </summary>
        /// <param name="arg">Transform</param>
        public void TransformInvoke(Transform arg)
        {
            OnTransformEvent?.Invoke(new TransformArgs(arg));
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

    /// <summary>
    /// Передаваемый аргумент события 
    /// </summary>
    public class TransformArgs : EventArgs
    {
        public TransformArgs(Transform arg)
        {
            TransformArg = arg;
        }

        public Transform TransformArg {get;private set; }
    }
}