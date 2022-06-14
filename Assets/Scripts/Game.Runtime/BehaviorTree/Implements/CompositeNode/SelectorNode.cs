using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "SelectorNode", menuName = "BehaviorTree/Node/Composite/SelectorNode")]
    public class SelectorNode : CompositeNode
    {

        protected override NodeState OnUpdate(float deltaTime)
        {
            var childNeedReset = CurrentNodeState != NodeState.Running || this.IsReset();
            foreach (Node node in this.children)
            {
                var nodeState = node.DoUpdate(deltaTime, childNeedReset);
                switch (nodeState)
                {
                    case NodeState.Running:
                        CurrentNodeState = NodeState.Running;
                        return CurrentNodeState;
                    case NodeState.Failure:
                        continue;
                    case NodeState.Success:
                        CurrentNodeState = NodeState.Success;
                        return CurrentNodeState;
                }
            }

            CurrentNodeState = NodeState.Failure;

            return CurrentNodeState;
        }
    }
}