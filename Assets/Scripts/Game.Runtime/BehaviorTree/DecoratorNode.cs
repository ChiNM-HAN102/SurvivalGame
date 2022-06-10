using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public abstract class DecoratorNode : Node
    {
        public Node child;
        
        public override NodeType _NodeType { get; } = NodeType.Decorator;
    }
}
