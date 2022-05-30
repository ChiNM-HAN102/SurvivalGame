using System;
using System.Collections;
using System.Collections.Generic;
using Game.Runtime.RPGStats;
using UnityEngine;

namespace Game.Runtime
{
    public abstract class Unit : Dummy, IUpdateSystem
    {
        public BehaviorState state;
        
        protected RPGStatCollection stats;
        public virtual RPGStatCollection Stats => stats;

        protected StatusController statusController;
        
        public virtual bool IsAlive => true;
        public abstract void Remove();
        
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
        public virtual void GetHurt(float damageInfo)
        {
            if (IsAlive == false)
            {
                print("Unit was died: " + name);
                return;
            }

            CalculateHealthPoint(damageInfo);

            if (IsAlive == false)
            {
                Remove();
            }
        }
        
        protected virtual void CalculateHealthPoint(float damageInfo)
        {
            var health = this.Stats.GetStat<Health>(RPGStatType.Health);
            health.TakeDamage(damageInfo);
        }
    }
}
