using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public abstract class CompositeNode : Node
    {
        public List<Node> children = new List<Node>();
        
        public override NodeType _NodeType { get; } = NodeType.Composite;

        public override Node Clone(BehaviorTree tree)
        {
            var node = Instantiate(this);
            Debug.Log("Clone " + GetType().Name);

            if (this.children.Count > 0)
            {
                node.children = this.children.ConvertAll(x => x.Clone(tree));
            }
     
            return node;
        }
    }
}
