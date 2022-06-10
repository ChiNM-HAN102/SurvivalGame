using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public abstract class CompositeNode : Node
    {
        public List<Node> children = new List<Node>();
        
        public override NodeType _NodeType { get; } = NodeType.Composite;

        public override Node Clone()
        {
            var node = Instantiate(this);

            node.children = this.children.ConvertAll(x => x.Clone());

            return node;
        }
    }
}
