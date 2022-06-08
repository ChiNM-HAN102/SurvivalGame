using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public abstract class Node : ScriptableObject
    {
        public enum NodeState
        {
            Running,
            Failure,
            Success
        }

        public NodeState nodeState = NodeState.Running;
        public bool started = false;

        public NodeState Update()
        {
            if (!this.started)
            {
                OnStart();
                this.started = true;
            }

            this.nodeState = OnUpdate();

            if (this.nodeState == NodeState.Failure || this.nodeState == NodeState.Success)
            {
                OnStop();
                this.started = false;
            }

            return this.nodeState;
        }

        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract NodeState OnUpdate();
    }
}
