using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public abstract class Dummy : MonoBehaviour , IUpdateSystem
    {
        protected bool faceRight;
        
         

        protected virtual void Awake()
        {
            this.faceRight = true;
        }

        protected virtual void OnEnable()
        {
            if (GlobalUpdateSystem.Instance != null)
            {
                GlobalUpdateSystem.Instance.Add(this);
            }
        }

        protected void OnDisable()
        {
            
            if (GlobalUpdateSystem.Instance != null)
            {
                GlobalUpdateSystem.Instance.Remove(this);
            }
        }

        public abstract void OnUpdate(float deltaTime);
        
        public virtual void Flip()
        {
            var newScale = transform.localScale;
            newScale.x = -newScale.x;
            transform.localScale = newScale;

            this.faceRight = !this.faceRight;
        }

        public bool GetFaceRight()
        {
            return this.faceRight;
        }
    }
}
