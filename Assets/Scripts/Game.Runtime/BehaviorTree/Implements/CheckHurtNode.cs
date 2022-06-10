using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "CheckHurtNode", menuName = "BehaviorTree/Node/Condition/CheckHurtNode")]
    public class CheckHurtNode : ActionNode
    {
        protected override void OnStart()
        {
            
        }

        protected override void OnStop()
        {
            
        }

        protected override NodeState OnUpdate(float deltaTime)
        {
            if (this.tree.Owner.UnitState.Current == State.HURT)
            {
                return NodeState.Success;
            }

            return NodeState.Failure;
        }
    }
}