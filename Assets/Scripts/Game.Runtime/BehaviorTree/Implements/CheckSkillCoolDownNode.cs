using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "CheckSkillCoolDownNode", menuName = "BehaviorTree/Node/Condition/CheckSkillCoolDownNode")]
    public class CheckSkillCoolDownNode : ActionNode
    {
        public SkillType skillType;
        
        protected override void OnStart()
        {
            
        }

        protected override void OnStop()
        {
            
        }

        protected override NodeState OnUpdate(float deltaTime)
        {
            var skill = this.Tree.Owner.Skills.GetSkill(this.skillType);
            if (skill == null || !skill.CanUse)
            {
                CurrentNodeState = NodeState.Failure;
                return CurrentNodeState;
            }

            CurrentNodeState = NodeState.Success;
            return CurrentNodeState;
        }
    }
}