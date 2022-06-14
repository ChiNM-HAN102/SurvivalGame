using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "Idle", menuName = "BehaviorTree/Node/Action/Idle")]
    public class Idle : ActionNode
    {
        public string idleAnim;
        

        protected override NodeState OnUpdate(float deltaTime)
        {
            this.Tree.Owner.AnimController.Idle();
            CurrentNodeState = NodeState.Success;
            return CurrentNodeState;
        }
    }
}