using Assets.Scripts.Managers;
using Assets.Scripts.Player;

namespace Assets.Scripts.StateMachine.StateImplementation
{
    /// <summary>
    /// Реализация состояния движения танка
    /// </summary>
    public class MoveState : TankState
    {
        private TankControllingManager _controllingManager;
        private TankCore _player;

        public MoveState(TankControllingManager controllingManager) : base(controllingManager)
        {
            _controllingManager = controllingManager;
            _player = ManagerContainer.Get<SpawnManager>().SpawnedTank;
        }

        public override void Stay()
        {
            //Переходим в состояние покоя
            StateManager.ChangeState(new IdleState(_controllingManager));
        }

        public override void Move()
        {
            //Двигаем танк
            _player?.MoveTank(_controllingManager.VerticalInput.Value);
        }

        public override void Turn()
        {
            //Переходим в состояние вращения
            StateManager.ChangeState(new TurnState(_controllingManager));
        }
    }
}
