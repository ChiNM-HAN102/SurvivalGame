using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "Reverter", menuName = "BehaviorTree/Node/Decorator/Reverter")]
    public class Reverter : DecoratorNode
    {

        protected override NodeState OnUpdate(float deltaTime)
        {
            var currentState = this.child.DoUpdate(deltaTime);

            switch (currentState)
            {
                case  NodeState.Running:
                    CurrentNodeState = NodeState.Running;
                    return CurrentNodeState;
                case NodeState.Failure:
                    CurrentNodeState = NodeState.Success;
                    return CurrentNodeState;
                case NodeState.Success:
                    CurrentNodeState = NodeState.Failure;
                    return CurrentNodeState;
            }

            CurrentNodeState = NodeState.Running;
            return CurrentNodeState;
        }
    }
}
