using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Events
{
    /// <summary>
    /// Класс слушателя игровых событий
    /// </summary>
    public class GameEventListener : MonoBehaviour
    {
        [Tooltip("Игровое событие, на которое нужно подписаться")]
        public GameEventScriptableObject Event;

        [Tooltip("Обработчик события")]
        public UnityEvent EventHandler;

        private void OnEnable()
        {
            //Подписываем на событие
            Event.Subscribe(this);
        }

        private void OnDisable()
        {
            //Отписываемся от события
            Event.Unsubscribe(this);
        }

        /// <summary>
        /// Метод вызывает обработчик события
        /// </summary>
        public void OnEventInvoked()
        {
            EventHandler.Invoke();
        }
    }
}
