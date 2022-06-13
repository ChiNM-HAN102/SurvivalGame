using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "MoveToTargetNode", menuName = "BehaviorTree/Node/Action/MoveToTargetNode")]
    public class MoveToTargetNode : ActionNode
    {
        private Unit owner;

        public string moveName;
        public string idleName;
        
        protected override void OnStart()
        {
            this.owner = this.Tree.Owner;
        }
        

        protected override NodeState OnUpdate(float deltaTime)
        {
            if (this.owner.target == null)
            {
                CurrentNodeState = NodeState.Failure;
                return CurrentNodeState;
            }

            var transform = this.owner.transform;
            var targetPosition = this.owner.target.transform.position;
            
            if (targetPosition.x > transform.position.x && !this.owner.GetFaceRight() || 
                targetPosition.x < transform.position.x && this.owner.GetFaceRight())
            {
                this.owner.Flip();
            }
            
            var meleeDetect = this.owner.Stats.GetStat<MeleeDetectRange>(RPGStatType.MeleeDetectRange);
            var meleeDetectValue = meleeDetect == null ? 0 : meleeDetect.StatValue;
            
            if (Vector2.Distance(targetPosition, transform.position) > meleeDetectValue)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition,
                    this.owner.Stats.GetStat<MoveSpeed>(RPGStatType.MoveSpeed).StatValue * deltaTime);
                
                this.owner.AnimController.DoAnim(this.moveName, State.MOVE);

                CurrentNodeState = NodeState.Running;
                return CurrentNodeState;
            }

            CurrentNodeState = NodeState.Success;
            return CurrentNodeState;
        }
    }
}