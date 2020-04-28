using Assets.Scripts.Managers;

namespace Assets.Scripts.StateMachine
{
    /// <summary>
    /// Абстрактый класс для всех состояний танка
    /// </summary>
    public abstract class TankState : State
    {
        public TankState(TankControllingManager controllingManager) : base(controllingManager)
        {
        }

        public virtual void Stay()
        { }

        public virtual void Move()
        { }

        public virtual void Turn()
        { }
    }
}
