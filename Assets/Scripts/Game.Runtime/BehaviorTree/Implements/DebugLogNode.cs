using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class DebugLogNode : ActionNode
    {
        public string message;
        
        protected override void OnStart()
        {
            Debug.Log($"OnStart {this.message}");
        }

        protected override void OnStop()
        {
            Debug.Log($"OnStop {this.message}");
        }

        protected override NodeState OnUpdate(float deltaTime)
        {
            Debug.Log($"OnUpdate {this.message}");
            return Node.NodeState.Success;
        }
    }
}
