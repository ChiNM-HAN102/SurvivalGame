
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Lean.Pool;
using UnityEngine;

namespace Game.Runtime
{
    public class BulletBase : DamageBox
    {
        [SerializeField] private bool initFaceLeft = false;
        [SerializeField] private GameObject prefabImpact;

        protected float attack;

        private CancellationTokenSource cts = new CancellationTokenSource();
        
        private Vector3 directionVector;
        

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

            if (gameObject.activeSelf)
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
        
        public override void OnUpdate(float deltaTime)
        {
            transform.position = transform.position + directionVector * deltaTime;
        }

        public override float GetDamage(Unit target)
        {
            Remove();
            return this.attack;
        }
    }
}