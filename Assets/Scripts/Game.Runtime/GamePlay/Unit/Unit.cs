using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Runtime
{
    public enum UnitState
    {
        NONE = -1,
        IDLE = 0,
        MOVE = 1,
        HURT = 2,
        ATTACK = 3,
        DIE = 4
    }
    
    public abstract class Unit : Dummy
    {

        public BehaviorState state;
        public RPGStatCollection Stats { get; set; }

        public StatusController StatusController { get; set; }
        
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

        public async UniTask WaitUntilFinishAnim(Animator _animator)
        {
            await UniTask.Yield();
            var clips = _animator.GetCurrentAnimatorClipInfo(0);
            if (clips.Length > 0)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(clips[0].clip.length));
            }
        }
        
    }
}
