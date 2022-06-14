using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "SequenceNode", menuName = "BehaviorTree/Node/Composite/SequenceNode")]
    public class SequenceNode : CompositeNode
    {
        protected override NodeState OnUpdate(float deltaTime)
        {
            
            var childNeedReset = CurrentNodeState == NodeState.Failure || this.IsReset(); 
            foreach (Node node in this.children)
            {
                var nodeState = node.DoUpdate(deltaTime, childNeedReset);
                switch (nodeState)
                {
                    case NodeState.Failure:
                        CurrentNodeState = NodeState.Failure;
                        return CurrentNodeState;
                    case NodeState.Running:
                        CurrentNodeState = NodeState.Running;
                        return CurrentNodeState;
                    case NodeState.Success:
                        continue;
                }
            }

            CurrentNodeState = NodeState.Success;
            return CurrentNodeState;
        }
    }
}