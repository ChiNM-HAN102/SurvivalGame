using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "SelectorNode", menuName = "BehaviorTree/Node/Composite/SelectorNode")]
    public class SelectorNode : CompositeNode
    {
        private int current;
        
        protected override void OnStart()
        {
            this.current = 0;
        }

        protected override void OnStop()
        {
           
        }

        protected override NodeState OnUpdate(float deltaTime)
        {
            var child = this.children[this.current];
            switch (child.DoUpdate(deltaTime))
            {
                case NodeState.Running:
                    return NodeState.Running;
                case NodeState.Failure:
                    this.current++;
                    break;
                case NodeState.Success:
                    return NodeState.Success;
            }

            return this.current == this.children.Count ? NodeState.Success : NodeState.Running;
        }
    }
}