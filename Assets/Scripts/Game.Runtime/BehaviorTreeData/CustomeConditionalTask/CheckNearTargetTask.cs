using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Game.Runtime
{
    public class CheckNearTargetTask : Conditional
    {
        private Unit _owner;

        public override void OnStart()
        {
            this._owner = Owner.GetComponent<Unit>();
        }

        public override TaskStatus OnUpdate()
        {
            if (this._owner.UnitState.Current == State.ATTACK)
            {
                return TaskStatus.Success;
            }
            
            var meleeDetect = this._owner.Stats.GetStat<MeleeDetectRange>(RPGStatType.MeleeDetectRange);
            var meleeDetectValue = meleeDetect == null ? 0 : meleeDetect.StatValue;
            
            var targetPosition = this._owner.Target.transform.position;

            if (Vector2.Distance(targetPosition, this._owner.transform.position) > meleeDetectValue 
                || (!this._owner.GetFaceRight() && (targetPosition.x - this._owner.transform.position.x) > 0)
                || (this._owner.GetFaceRight() && (targetPosition.x - this._owner.transform.position.x) < 0))
            {
                return TaskStatus.Failure;
            }

            return TaskStatus.Success;
        }
    }
}