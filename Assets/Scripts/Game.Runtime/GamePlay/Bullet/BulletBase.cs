
using System;
using Cysharp.Threading.Tasks;
using Lean.Pool;
using UnityEngine;

namespace Game.Runtime
{
    public class BulletBase : DamageBox
    {
        [SerializeField] private bool initFaceLeft = false;
        
        private Vector3 directionVector;
        

        public virtual void InitBullet(float lifeTime, bool targetFaceRight, float speed)
        {
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
            LeanPool.Despawn(gameObject);
        }
        
        public override void OnUpdate(float deltaTime)
        {
            transform.position = transform.position + directionVector * deltaTime;
        }
    }
}