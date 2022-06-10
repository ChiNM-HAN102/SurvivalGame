using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public abstract class DecoratorNode : Node
    {
        public Node child;
        
        public override NodeType _NodeType { get; } = NodeType.Decorator;

        public override Node Clone()
        {
            var node = Instantiate(this);
            node.child = this.child.Clone();

            return node;
        }
    }
}
