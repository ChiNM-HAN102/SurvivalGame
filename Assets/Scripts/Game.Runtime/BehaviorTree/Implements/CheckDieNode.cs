using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "CheckDieNode", menuName = "BehaviorTree/Node/Condition/CheckDieNode")]
    public class CheckDieNode : ActionNode
    {
        protected override void OnStart()
        {
            
        }

        protected override void OnStop()
        {
            
        }

        protected override NodeState OnUpdate(float deltaTime)
        {
            if (this.tree.Owner.UnitState.Current == State.DIE)
            {
                return NodeState.Success;
            }

            return NodeState.Failure;
        }
    }
}