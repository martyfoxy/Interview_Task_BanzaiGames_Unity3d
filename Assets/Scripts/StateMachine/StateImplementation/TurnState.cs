using Assets.Scripts.Managers;
using Assets.Scripts.Player;

namespace Assets.Scripts.StateMachine.StateImplementation
{
    /// <summary>
    /// Реализация состояния поворота танка
    /// </summary>
    public class TurnState : TankState
    {
        private TankControllingManager _controllingManager;
        private TankCore _player;

        public TurnState(TankControllingManager controllingManager) : base(controllingManager)
        {
            _controllingManager = controllingManager;
            _player = ManagerContainer.Get<SpawnManager>().TankReference;
        }

        public override void Stay()
        {
            //Переходим в состояние покоя
            StateManager.ChangeState(new IdleState(_controllingManager));
        }

        public override void Move()
        {
            //Переходим в состояние движения
            StateManager.ChangeState(new MoveState(_controllingManager));
        }

        public override void Turn()
        {
            //Поворачиваем танк
            _player?.RotateTank(_controllingManager.HorizontalInput.Value);
        }
    }
}
