using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "Idle", menuName = "BehaviorTree/Node/Action/Idle")]
    public class Idle : ActionNode
    {
        public string idleAnim;
        
        protected override void OnStart()
        {
           
        }

        protected override void OnStop()
        {
           
        }

        protected override NodeState OnUpdate(float deltaTime)
        {
            this.Tree.Owner.AnimController.DoAnim(this.idleAnim, State.IDLE);

            return NodeState.Success;
        }
    }
}