using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class Reverter : DecoratorNode
    {
        protected override void OnStart()
        {
            
        }

        protected override void OnStop()
        {
            
        }

        protected override NodeState OnUpdate(float deltaTime)
        {
            var currentState = this.child.DoUpdate(deltaTime);

            switch (currentState)
            {
                case  NodeState.Running:
                    return NodeState.Running;
                case NodeState.Failure:
                    return NodeState.Success;
                case NodeState.Success:
                    return NodeState.Failure;
            }

            return NodeState.Running;
        }
    }
}
