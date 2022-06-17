using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Game.Runtime
{
    public class MoveToTargetTask: Action
    {
        private Unit _owner;

        public override void OnStart()
        {
            base.OnStart();

            this._owner = this.Owner.GetComponent<Unit>();
        }

        public override TaskStatus OnUpdate()
        {
            if (this._owner.Target == null)
            {
                return TaskStatus.Failure;
            }

            if (this._owner.UnitState.Current == State.ATTACK)
            {
                return TaskStatus.Success;
            }

            var transform = this._owner.transform;
            var targetPosition = this._owner.Target.transform.position;
            
            if (targetPosition.x > transform.position.x && !this._owner.GetFaceRight() || 
                targetPosition.x < transform.position.x && this._owner.GetFaceRight())
            {
                this._owner.Flip();
            }
            
            var meleeDetect = this._owner.Stats.GetStat<MeleeDetectRange>(RPGStatType.MeleeDetectRange);
            var meleeDetectValue = meleeDetect == null ? 0 : meleeDetect.StatValue;
            
            if (Vector2.Distance(targetPosition, transform.position) > meleeDetectValue)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition,
                    this._owner.Stats.GetStat<MoveSpeed>(RPGStatType.MoveSpeed).StatValue * Time.deltaTime);
                
                this._owner.AnimController.Move();
                
                return TaskStatus.Running;
            }
            
            return TaskStatus.Success;
        }
    }
}