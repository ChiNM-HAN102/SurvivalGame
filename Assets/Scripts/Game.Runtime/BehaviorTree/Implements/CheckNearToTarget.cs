using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "CheckNearToTarget", menuName = "BehaviorTree/Node/Condition/CheckNearToTarget")]
    public class CheckNearToTarget: ActionNode
    {
        protected override void OnStart()
        {
            
        }

        protected override void OnStop()
        {
            
        }

        protected override NodeState OnUpdate(float deltaTime)
        {
            var meleeDetect = this.tree.Owner.Stats.GetStat<MeleeDetectRange>(RPGStatType.MeleeDetectRange);
            var meleeDetectValue = meleeDetect == null ? 0 : meleeDetect.StatValue;
            
            var targetPosition = this.tree.Owner.target.transform.position;

            if (Vector2.Distance(targetPosition, this.tree.Owner.transform.position) > meleeDetectValue)
            {
                return NodeState.Failure;
            }
            
            return NodeState.Success;
        }
    }
}