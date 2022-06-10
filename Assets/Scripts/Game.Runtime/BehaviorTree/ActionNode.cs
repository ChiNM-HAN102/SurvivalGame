using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public abstract class ActionNode : Node
    {
        public override NodeType _NodeType { get; } = NodeType.Action;
    }
}
