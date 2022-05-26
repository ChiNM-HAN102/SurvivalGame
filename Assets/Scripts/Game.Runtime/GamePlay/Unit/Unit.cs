using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public abstract class Unit : Dummy, IUpdateSystem
    {
        public BehaviorState state;
        
        protected virtual void OnEnable()
        {
            if (GlobalUpdateSystem.Instance != null)
            {
                GlobalUpdateSystem.Instance.Add(this);
            }
        }

        protected virtual void OnDisable()
        {

            if (GlobalUpdateSystem.Instance != null)
            {
                GlobalUpdateSystem.Instance.Remove(this);
            }
        }

        public override void OnUpdate(float deltaTime)
        {
            
        }
    }
}
