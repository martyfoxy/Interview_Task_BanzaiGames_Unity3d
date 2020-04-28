using Assets.Scripts.Managers;
using Assets.Scripts.Player;

namespace Assets.Scripts.StateMachine.StateImplementation
{
    /// <summary>
    /// Реализация состояния выстрела
    /// </summary>
    public class FireState : TankState
    {
        private TankControllingManager _controllingManager;
        private TankCore _player;

        public FireState(TankControllingManager controllingManager) : base(controllingManager)
        {
            _controllingManager = controllingManager;
            _player = ManagerContainer.Get<SpawnManager>().TankReference;
        }
    }
}
