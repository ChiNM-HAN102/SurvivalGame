using BehaviorDesigner.Runtime.Tasks;

namespace Game.Runtime
{
    public class IdleTask : Action
    {
        private Unit _owner;
        
        public override void OnStart()
        {
            base.OnStart();

            this._owner = Owner.GetComponent<Unit>();
        }

        public override TaskStatus OnUpdate()
        {
            this._owner.AnimController.Idle();
            return TaskStatus.Success;
        }
    }
}