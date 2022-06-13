using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "RepeatNode", menuName = "BehaviorTree/Node/Decorator/RepeatNode")]
    public class RepeatNode : DecoratorNode
    {
        protected override NodeState OnUpdate(float deltaTime)
        {
            this.child.DoUpdate(deltaTime);
            CurrentNodeState = NodeState.Running;
            return CurrentNodeState;
        }
    }
}
