using Assets.Scripts.Managers;

namespace Assets.Scripts.StateMachine
{
    /// <summary>
    /// Абстрактный класс для всех состояний
    /// </summary>
    public abstract class State
    {
        protected AbstractStateMachineManager StateManager;

        public State(AbstractStateMachineManager stateManager)
        {
            StateManager = stateManager;
        }
    }
}
