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
        private TankCore _player;

        public IdleState(TankControllingManager controllingManager) : base(controllingManager)
        {
            _controllingManager = controllingManager;
            _player = ManagerContainer.Get<SpawnManager>().TankReference;
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

        public override void Fire()
        {
            //Стреляем
            _player?.Fire();
        }
    }
}
