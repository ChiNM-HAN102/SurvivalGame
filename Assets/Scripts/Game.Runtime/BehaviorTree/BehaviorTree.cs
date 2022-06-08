using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu()]
    public class BehaviorTree : ScriptableObject
    {
        public Node rootNode;

        public Node.NodeState treeState = Node.NodeState.Running;

        public Node.NodeState Update()
        {
            if (this.rootNode.nodeState == Node.NodeState.Running)
            {
                this.treeState = this.rootNode.Update();
            }

            return this.treeState;
        }
    }
}
