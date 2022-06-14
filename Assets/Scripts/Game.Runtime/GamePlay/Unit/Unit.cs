using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Runtime
{
    public abstract class Unit : Dummy
    {
        protected bool faceRight;

        public Unit Target { get; set; }

        public virtual UnitData Data { get;}

        public RPGStatCollection Stats { get; set; }
        
        public SkillController Skills { get; set; }
        
        public UnitAnimController AnimController { get; set; }

        
        public UnitState UnitState { get; set; } = new UnitState();

        public InputControlType CurrentControlType { get; protected set; } = InputControlType.NONE;

        protected virtual void Awake()
        {
            Skills = new SkillController();
            AnimController = GetComponent<UnitAnimController>();
        }

        public virtual bool IsAlive
        {
            get
            {
                if (gameObject == null) return false;

                return gameObject.activeSelf && Stats.GetStat<Health>(RPGStatType.Health).CurrentValue > 0;
            }
        }
        
        public override void OnUpdate(float deltaTime)
        {
            if (GamePlayController.Instance.State != GameState.RUNNING)
            {
                return;
            }
        }
        public virtual void GetHurt(float damageInfo)
        {
            if (IsAlive == false)
            {
                print("Unit was died: " + name);
                return;
            }

            CalculateHealthPoint(damageInfo);
        }
        
        protected virtual void CalculateHealthPoint(float damageInfo)
        {
            var health = this.Stats.GetStat<Health>(RPGStatType.Health);
            health.TakeDamage(damageInfo);
        }

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

#if UNITY_EDITOR
        public State debugState;
        private void Update()
        {
            this.debugState = UnitState.Current;
        }
#endif
    }
}
