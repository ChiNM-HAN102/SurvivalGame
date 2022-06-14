using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "Idle", menuName = "BehaviorTree/Node/Action/Idle")]
    public class Idle : ActionNode
    {
        public string idleAnim;
        

        protected override NodeState OnUpdate(float deltaTime)
        {
            Debug.Log(this.ToString());
            this.Tree.Owner.AnimController.DoAnim(this.idleAnim, State.IDLE);

            CurrentNodeState = NodeState.Success;
            return CurrentNodeState;
        }
    }
}