using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "CheckFarToTarget", menuName = "BehaviorTree/Node/Condition/CheckFarToTarget")]
    public class CheckFarToTarget : ActionNode
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
                return NodeState.Success;
            }
            
            return NodeState.Failure;
        }
    }
}
