using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "BehaviorTree", menuName = "BehaviorTree/Tree")]
    public class BehaviorTree : ScriptableObject
    {
        public Node rootNode;

        public List<Node> listNode;

        public Unit Owner { get; set; }

        public Node.NodeState TreeState { get; set; } = Node.NodeState.Running;

        public Node.NodeState DoUpdate(float deltaTime)
        {
            if (this.rootNode != null && this.rootNode.CurrentNodeState == Node.NodeState.Running)
            {
                this.TreeState = this.rootNode.DoUpdate(deltaTime);
            }

            return this.TreeState;
        }

        public void SetUpTree(Unit owner)
        {
            this.Owner = owner;
        }

        public BehaviorTree CloneTree()
        {
            var tree = Instantiate(this);
            tree.rootNode = this.rootNode.Clone();

            return tree;
        }
    }
}
