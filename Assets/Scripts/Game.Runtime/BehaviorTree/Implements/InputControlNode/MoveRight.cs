using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "MoveRight", menuName = "BehaviorTree/Node/InputControl/MoveRight")]
    public class MoveRight : InputControlNode
    {
        protected override NodeState OnUpdate(float deltaTime)
        {
            var owner = this.Tree.Owner;
            if (this.controlType == this.Tree.Owner.CurrentControlType)
            {
                if (!owner.GetFaceRight())
                {
                    owner.Flip();
                }
                
                var moveVector = new Vector3(1, 0, 0);
                owner.transform.position = owner.transform.position + moveVector * (owner.Stats.GetStat<MoveSpeed>(RPGStatType.MoveSpeed).StatValue * deltaTime);
                owner.AnimController.Move();
                CurrentNodeState = NodeState.Success;

                return CurrentNodeState;
            }
            
            CurrentNodeState = NodeState.Failure;
            return CurrentNodeState;
        }
    }
}