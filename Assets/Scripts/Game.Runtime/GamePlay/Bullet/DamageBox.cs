using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Lean.Pool;
using UnityEngine;

namespace Game.Runtime
{
    public class BulletBase : Dummy, IGetDamage
    {
        private Vector3 directionVector;

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }

        public virtual void InitBullet(float lifeTime, Vector2 target, float speed)
        {
            DestroyBullet(lifeTime).Forget();
            if (target.x < transform.position.x)
            {
                
            }
            else
            {
                
            }
        }

        async UniTaskVoid DestroyBullet(float lifeTime)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(lifeTime), ignoreTimeScale: false);
            LeanPool.Despawn(gameObject);
        }
        
        
        

        public override void OnUpdate(float deltaTime)
        {
            if (this.goLeft)
            {
                var directionVector = new Vector3(this.speed, 0);
                transform.position = transform.position + directionVector;
            }   
        }

        public float GetDamage(Unit target)
        {
            
        }
    }
}
