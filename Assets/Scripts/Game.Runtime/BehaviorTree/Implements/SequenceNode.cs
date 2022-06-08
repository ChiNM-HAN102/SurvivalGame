using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class SequenceNode : CompositeNode
    {
        private int current;

        protected override void OnStart()
        {
            this.current = 0;
        }

        protected override void OnStop()
        {
            
        }

        protected override NodeState OnUpdate()
        {
            var child = this.children[this.current];
            switch (child.Update())
            {
                case NodeState.Running:
                    return NodeState.Running;
                case NodeState.Failure:
                    return NodeState.Failure;
                case NodeState.Success:
                    this.current++;
                    break;
            }

            return this.current == this.children.Count ? NodeState.Success : NodeState.Running;
        }
    }
}
