using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "CheckNearToTarget", menuName = "BehaviorTree/Node/Condition/CheckNearToTarget")]
    public class CheckNearToTarget: ActionNode
    {
        protected override NodeState OnUpdate(float deltaTime)
        {
            var meleeDetect = this.Tree.Owner.Stats.GetStat<MeleeDetectRange>(RPGStatType.MeleeDetectRange);
            var meleeDetectValue = meleeDetect == null ? 0 : meleeDetect.StatValue;
            
            var targetPosition = this.Tree.Owner.target.transform.position;

            if (Vector2.Distance(targetPosition, this.Tree.Owner.transform.position) > meleeDetectValue)
            {
                CurrentNodeState = NodeState.Failure;
                return CurrentNodeState;
            }

            CurrentNodeState = NodeState.Success;
            return CurrentNodeState;
        }
    }
}