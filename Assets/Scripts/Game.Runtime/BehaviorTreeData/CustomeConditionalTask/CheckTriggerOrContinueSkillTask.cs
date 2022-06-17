using BehaviorDesigner.Runtime.Tasks;

namespace Game.Runtime
{
    public class CheckTriggerOrContinueSkillTask : Conditional
    {
        public SkillType skillType;
        public State state;

        private Unit _owner;

        public override void OnStart()
        {
            base.OnStart();

            this._owner = Owner.GetComponent<Unit>();
        }

        public override TaskStatus OnUpdate()
        {
            var skill = this._owner.Skills.GetSkill(this.skillType);
            if ((skill == null || !skill.CanUse) && this._owner.UnitState.Current != state)
            {
                return TaskStatus.Failure;
            }
            
            return TaskStatus.Success;
        }
    }
}