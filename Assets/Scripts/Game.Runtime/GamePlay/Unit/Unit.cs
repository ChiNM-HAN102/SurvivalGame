using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Runtime
{
    public abstract class Unit : Dummy
    {
        public Animator animator;
        
        protected bool faceRight;

        public Unit target;

        public virtual UnitData Data { get;}

        public RPGStatCollection Stats { get; set; }
        
        public SkillController Skills { get; set; }

        
        public UnitState UnitState { get; set; } = new UnitState();

        protected virtual void Awake()
        {
            this.animator = GetComponentInChildren<Animator>();
            Skills = new SkillController();
        }

        public virtual bool IsAlive
        {
            get
            {
                if (gameObject == null) return false;

                return gameObject.activeSelf && Stats.GetStat<Health>(RPGStatType.Health).CurrentValue > 0;
            }
        }
        
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

        public void UseSkill(string anim)
        {
            UnitState.Set(State.ATTACK);
            this.animator.Play(anim);
        }

        public virtual void DoAnim(string animName)
        {
            this.animator.Play(animName);
        }
    }
}
