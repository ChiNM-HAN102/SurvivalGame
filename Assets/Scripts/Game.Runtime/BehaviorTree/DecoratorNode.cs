using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public abstract class DecoratorNode : Node
    {
        public Node child;
        
        public override NodeType _NodeType { get; } = NodeType.Decorator;

        public override Node Clone(BehaviorTree tree)
        {
            var node = Instantiate(this);
            Debug.Log("Clone " + GetType().Name);

            if (this.child != null)
            {
                node.child = this.child.Clone(tree);
            }
          

            return node;
        }
    }
}
