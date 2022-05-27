using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Lean.Pool;
using UnityEngine;

namespace Game.Runtime
{
    public class BulletBase : Dummy
    {

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }

        public virtual void InitBullet(float lifeTime)
        {
            DestroyBullet(lifeTime).Forget();  
        }

        async UniTaskVoid DestroyBullet(float lifeTime)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(lifeTime), ignoreTimeScale: false);
            LeanPool.Despawn(gameObject);
        }
        
        
        

        public override void OnUpdate(float deltaTime)
        {
            
        }
    }
}
