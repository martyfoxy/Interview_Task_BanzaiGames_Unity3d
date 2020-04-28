using Assets.Scripts.StateMachine;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Абстрактный класс любого менеджера, использующего машину состояний
    /// </summary>
    public abstract class AbstractStateMachineManager : BaseManager
    {
        public State CurrentState;

        /// <summary>
        /// Сменить текущее состояние
        /// </summary>
        /// <param name="nextState">Следующее состояние</param>
        public void ChangeState(TankState nextState)
        {
            CurrentState = nextState;
        }
    }
}
