using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "CheckFarToTarget", menuName = "BehaviorTree/Node/Condition/CheckFarToTarget")]
    public class CheckFarToTarget : ActionNode
    {

        protected override NodeState OnUpdate(float deltaTime)
        {
            var meleeDetect = this.Tree.Owner.Stats.GetStat<MeleeDetectRange>(RPGStatType.MeleeDetectRange);
            var meleeDetectValue = meleeDetect == null ? 0 : meleeDetect.StatValue;
            
            var targetPosition = this.Tree.Owner.target.transform.position;

            if (Vector2.Distance(targetPosition, this.Tree.Owner.transform.position) > meleeDetectValue)
            {
                CurrentNodeState = NodeState.Success;
                return CurrentNodeState;
            }

            CurrentNodeState = NodeState.Failure;
            return CurrentNodeState;
        }
    }
}
