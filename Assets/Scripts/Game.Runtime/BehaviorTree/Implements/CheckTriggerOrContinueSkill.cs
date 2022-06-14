using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "CheckTriggerOrContinueSkill", menuName = "BehaviorTree/Node/Condition/CheckTriggerOrContinueSkill")]
    public class CheckTriggerOrContinueSkill : ActionNode
    {
        public SkillType skillType;
        public State state;
        

        protected override NodeState OnUpdate(float deltaTime)
        {
            var skill = this.Tree.Owner.Skills.GetSkill(this.skillType);
            if ((skill == null || !skill.CanUse) && this.Tree.Owner.UnitState.Current != state)
            {
                CurrentNodeState = NodeState.Failure;
                return CurrentNodeState;
            }

            CurrentNodeState = NodeState.Success;
            return CurrentNodeState;
        }
    }
}