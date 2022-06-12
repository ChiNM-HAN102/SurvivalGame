
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Lean.Pool;
using UnityEngine;

namespace Game.Runtime
{
    public class BulletData
    {
        public float LifeTime { get; set; }
        public float Damage { get; set; }
        public float Speed { get; set; }
        public bool Flip { get; set; }
        
        public BulletData(float lifeTime, bool flip, float speed, float damage)
        {
            this.LifeTime = lifeTime;
            this.Damage = damage;
            this.Speed = speed;
            this.Flip = flip;
        }
    }
    
    public class BulletBase : DamageBox, IUpdateSystem
    {
        [SerializeField] private bool initFaceLeft = false;
        [SerializeField] private GameObject prefabImpact;
        [SerializeField] private bool isAoe;

        protected BulletData data;

        protected bool faceRight;

        protected bool canDamage;

        private CancellationTokenSource cts;
        
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
        

        public virtual void InitBullet(BulletData inputData)
        {
            this.data = inputData;
            canDamage = true;
            cts = new CancellationTokenSource();
            DestroyBullet(this.data.LifeTime).Forget();
            if (this.data.Flip)
            {
                this.directionVector = new Vector3(this.data.Speed, 0);
                if (this.initFaceLeft)
                {
                    Flip();
                }
            }
            else
            {
                this.directionVector = new Vector3(- this.data.Speed, 0);
                if (!this.initFaceLeft)
                {
                    Flip();
                }
            }
        }
        
        async UniTaskVoid DestroyBullet(float lifeTime)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(lifeTime), cancellationToken: this.cts.Token);

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

            this.cts.Cancel();
            LeanPool.Despawn(gameObject);
        }
        
        public virtual void OnUpdate(float deltaTime)
        {
            transform.position = transform.position + directionVector * deltaTime;
        }

        public override float GetDamage(Unit target)
        {
            if (this.canDamage || this.isAoe)
            {
                this.canDamage = false;
                Remove();
                return this.data.Damage;
            }

            return 0;
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