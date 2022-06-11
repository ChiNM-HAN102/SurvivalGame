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

        public enum NodeType
        {
            Decorator,
            Composite,
            Action
        }
        
        public virtual NodeType _NodeType {get;}

        public BehaviorTree tree;

        public NodeState CurrentNodeState { get; set; }
        
        private bool started = false;

        public NodeState DoUpdate(float deltaTime)
        {
            if (!this.started)
            {
                Debug.Log("Start " + this.GetType().FullName);
                OnStart();
                this.started = true;
            }
            
            Debug.Log("Update " + this.GetType().FullName);

            this.CurrentNodeState = OnUpdate(deltaTime);

            if (this.CurrentNodeState == NodeState.Failure || this.CurrentNodeState == NodeState.Success)
            {
                Debug.Log("Stop " + this.GetType().FullName);
                OnStop();
                this.started = false;
            }

            return this.CurrentNodeState;
        }

        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract NodeState OnUpdate(float deltaTime);

        public void SetTree(BehaviorTree tree)
        {
            this.tree = tree;
        }
    }
}
