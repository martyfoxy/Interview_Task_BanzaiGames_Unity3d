using Assets.Scripts.Interface;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.StateMachine;
using Assets.Scripts.StateMachine.StateImplementation;
using Assets.Scripts.ScriptableObjects.Variables;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Класс менеджера для управления танком
    /// </summary>
    [CreateAssetMenu(fileName = "TankControllingManager", menuName = "Game Manager/Tank Controlling Manager")]
    public class TankControllingManager : AbstractStateMachineManager, IAwake, IFixedUpdate
    {
        [Header("Сссылки на переменные ввода")]
        [Tooltip("Ссылка на объект переменной хранящее значение ввода стрелками влево-вправо")]
        public FloatReference HorizontalInput;
        [Tooltip("Ссылка на объект переменной хранящее значение ввода стрелками вперед-назад")]
        public FloatReference VerticalInput;

        [Header("Ссылки на объекты с событиями ввода")]
        [Tooltip("Ссылка на объект события нажатия кнопки следующего оружия")]
        public GameEventScriptableObject NextWeaponButtonPressed;
        [Tooltip("Ссылка на объект события нажатия кнопки предыдущего оружия")]
        public GameEventScriptableObject PreviousButtonPressed;

        [Header("Ссылки на объекты с прочими событиями")]
        public GameEventScriptableObject PlayerSpawned;

        public void OnAwake()
        {
            UpdateManager.Register(this);

            //Подписываемся
            PlayerSpawned.OnGameEvent += PlayerSpawnedHandler;
        }

        private void OnDisable()
        {
            //Отписываемся
            PlayerSpawned.OnGameEvent -= PlayerSpawnedHandler;
        }

        /// <summary>
        /// Обработчик события создания танка
        /// </summary>
        private void PlayerSpawnedHandler()
        {
            //Изначально мы находимся в состоянии покоя
            ChangeState(new IdleState(this));
        }

        public void OnFixedUpdate()
        {
            if (Mathf.Abs(VerticalInput.Value) > 0.3f)
                (CurrentState as TankState).Move();
            else if (Mathf.Abs(HorizontalInput.Value) > 0.2f)
                (CurrentState as TankState).Turn();
        }
    }
}
