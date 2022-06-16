using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "CheckCanControlNode", menuName = "BehaviorTree/Node/Condition/CheckCanControlNode")]
    public class CheckCanControlNode : ActionNode
    {
        private Unit _owner;
        
        protected override void OnStart()
        {
            this._owner = this.Tree.Owner;
        }

        protected override NodeState OnUpdate(float deltaTime)
        {
            if (this._owner.UnitState.CanUseSkill())
            {
                CurrentNodeState = NodeState.Success;
                return CurrentNodeState;
            }

            CurrentNodeState = NodeState.Failure;
            return CurrentNodeState;
        }
    }
}