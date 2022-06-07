
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Lean.Pool;
using UnityEngine;

namespace Game.Runtime
{
    public class BulletBase : DamageBox, IUpdateSystem, ICanClear
    {
        [SerializeField] private bool initFaceLeft = false;
        [SerializeField] private GameObject prefabImpact;

        protected float attack;

        protected bool faceRight;

        private CancellationTokenSource cts = new CancellationTokenSource();
        
        private Vector3 directionVector;
        
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
        

        public virtual void InitBullet(float lifeTime, bool targetFaceRight, float speed, float attack)
        {
            this.attack = attack;

            DestroyBullet(lifeTime).Forget();
            if (targetFaceRight)
            {
                this.directionVector = new Vector3(speed, 0);
                if (this.initFaceLeft)
                {
                    Flip();
                }
            }
            else
            {
                this.directionVector = new Vector3(-speed, 0);
                if (!this.initFaceLeft)
                {
                    Flip();
                }
            }
        }
        
        async UniTaskVoid DestroyBullet(float lifeTime)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(lifeTime), ignoreTimeScale: false);

            if (gameObject != null && gameObject.activeSelf)
            {
                Remove();
            }
        }

        void Remove()
        {
            if (this.prefabImpact != null)
            {
                LeanPool.Spawn(this.prefabImpact, transform.position, Quaternion.identity);
            }

            LeanPool.Despawn(gameObject);
        }
        
        public virtual void OnUpdate(float deltaTime)
        {
            transform.position = transform.position + directionVector * deltaTime;
        }

        public override float GetDamage(Unit target)
        {
            Remove();
            return this.attack;
        }
        
        protected virtual void Flip()
        {
            var newScale = transform.localScale;
            newScale.x = -newScale.x;
            transform.localScale = newScale;

            this.faceRight = !this.faceRight;
        }
    }
}