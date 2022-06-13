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

        public BehaviorTree Tree { get; set; }

        public NodeState CurrentNodeState { get; set; }
        
        private bool started = false;

        protected bool GetStarted()
        {
            return this.started;
        }

        protected void SetStarted(bool value)
        {
            this.started = value;
        }

        private bool _reset;

        protected bool IsReset() => this._reset;
        
        public NodeState DoUpdate(float deltaTime, bool reset = false)
        {
            if (reset)
            {
                this._reset = true;
                this.started = false;
            }
            else
            {
                this._reset = false;
            }
            
            Prepare();
            
            if (!this.started)
            {
                OnStart();
                this.started = true;
            }

            this.CurrentNodeState = OnUpdate(deltaTime);

            if (this.CurrentNodeState == NodeState.Failure || this.CurrentNodeState == NodeState.Success)
            {
                OnStop();
                this.started = false;
            }

            return this.CurrentNodeState;
        }

        protected virtual void Prepare()
        {
            
        }

        protected virtual void OnStart()
        {
            
        }

        protected virtual void OnStop()
        {
            
        }
        protected abstract NodeState OnUpdate(float deltaTime);

        public virtual Node Clone(BehaviorTree tree)
        {
            var node = Instantiate(this);
            node.Tree = tree;
            return node;
        }

        public void SetTree(BehaviorTree inputTree)
        {
            this.Tree = inputTree;
        }
    }
}
