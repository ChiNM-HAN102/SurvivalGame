using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "WaitNode", menuName = "BehaviorTree/Node/Action/WaitNode")]
    public class WaitNode : ActionNode
    {
        public float duration = 1;
        private float startTime;

        protected override void OnStart()
        {
            this.startTime = Time.time;
        }
        

        protected override NodeState OnUpdate(float deltaTime)
        {
            if (Time.time - this.startTime > this.duration)
            {
                CurrentNodeState = NodeState.Success;
                return CurrentNodeState;
            }

            CurrentNodeState = NodeState.Running;
            return CurrentNodeState;
        }
    }
}
