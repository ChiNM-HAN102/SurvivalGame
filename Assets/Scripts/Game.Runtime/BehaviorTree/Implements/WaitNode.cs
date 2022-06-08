using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class WaitNode : ActionNode
    {
        public float duration = 1;
        private float startTime;

        protected override void OnStart()
        {
            this.startTime = Time.time;
        }

        protected override void OnStop()
        {
            
        }

        protected override NodeState OnUpdate()
        {
            if (Time.time - this.startTime > this.duration)
            {
                return NodeState.Success;
            }

            return NodeState.Running;
        }
    }
}
