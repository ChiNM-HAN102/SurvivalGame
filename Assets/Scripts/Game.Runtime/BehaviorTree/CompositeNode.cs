using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public abstract class CompositeNode : Node
    {
        public List<Node> children = new List<Node>();
        
    }
}
