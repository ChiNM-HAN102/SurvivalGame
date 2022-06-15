using System;
using Cysharp.Threading.Tasks;
using Lean.Pool;
using UnityEngine;

namespace Game.Runtime
{
    public class EnemyImpact : DamageBox
    {
        private Unit _owner;
        
        public void InitData(Unit owner, float lifeTime)
        {
            this._owner = owner;
            GetComponent<Collider2D>().enabled = true;
            DisableObject(lifeTime).Forget();
        }

        async UniTaskVoid DisableObject(float lifeTime)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(lifeTime));
            GetComponent<Collider2D>().enabled = false;
        }
        
        public override float GetDamage(Unit target)
        {
            var damage = this._owner.Stats.GetStat<Damage>(RPGStatType.Damage).StatValue;
            return damage;
        }
    }
}