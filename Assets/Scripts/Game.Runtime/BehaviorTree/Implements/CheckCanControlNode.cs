using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "CheckCanControlNode", menuName = "BehaviorTree/Node/Condition/CheckCanControlNode")]
    public class CheckCanControlNode : ActionNode
    {
        private Unit owner;
        
        protected override void OnStart()
        {
            this.owner = this.Tree.Owner;
        }

        protected override NodeState OnUpdate(float deltaTime)
        {
            if (this.owner.UnitState.CanUseSkill())
            {
                CurrentNodeState = NodeState.Success;
                return CurrentNodeState;
            }

            CurrentNodeState = NodeState.Failure;
            return CurrentNodeState;
        }
    }
}