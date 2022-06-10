using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "CheckCanControlNode", menuName = "BehaviorTree/Node/Condition/CheckCanControlNode")]
    public class CheckCanControlNode : ActionNode
    {
        private Unit owner;
        
        protected override void OnStart()
        {
            this.owner = this.tree.owner;
        }

        protected override void OnStop()
        {
           
        }

        protected override NodeState OnUpdate(float deltaTime)
        {
            if (this.owner.UnitState.CanUseSkill())
            {
                return NodeState.Failure;
            }

            return NodeState.Success;
        }
    }
}