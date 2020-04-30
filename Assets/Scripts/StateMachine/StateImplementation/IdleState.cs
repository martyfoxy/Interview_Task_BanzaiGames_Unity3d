using Assets.Scripts.Managers;
using Assets.Scripts.Player;

namespace Assets.Scripts.StateMachine.StateImplementation
{
    /// <summary>
    /// Реализация состояния покоя танка
    /// </summary>
    public class IdleState : TankState
    {
        private TankControllingManager _controllingManager;

        public IdleState(TankControllingManager controllingManager) : base(controllingManager)
        {
            _controllingManager = controllingManager;
        }

        public override void Stay()
        {
            //Ничего не делаем
        }

        public override void Turn()
        {
            //Переходим в состояние вращения
            StateManager.ChangeState(new TurnState(_controllingManager));
        }

        public override void Move()
        {
            //Переходим в состояние движения
            StateManager.ChangeState(new MoveState(_controllingManager));
        }
    }
}
