using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "MoveLeft", menuName = "BehaviorTree/Node/InputControl/MoveLeft")]
    public class MoveLeft : InputControlNode
    {

        protected override NodeState OnUpdate(float deltaTime)
        {
            var owner = this.Tree.Owner;
            if (this.controlType == this.Tree.Owner.CurrentControlType)
            {
                if (owner.GetFaceRight())
                {
                    owner.Flip();
                }
                
                var moveVector = new Vector2(-1, 0);
                owner.transform.position = owner.transform.position + (Vector3)moveVector * (owner.Stats.GetStat<MoveSpeed>(RPGStatType.MoveSpeed).StatValue * deltaTime);
                owner.AnimController.Move();
                CurrentNodeState = NodeState.Success;

                return CurrentNodeState;
            }
            
            CurrentNodeState = NodeState.Failure;
            return CurrentNodeState;
        }
    }
}