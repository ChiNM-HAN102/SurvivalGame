using System;
using Lean.Pool;
using UnityEngine;

namespace Game.Runtime.Impact
{
    public class EnemyNormalDamageBox : DamageBox
    {
        [SerializeField] private GameObject prefabImpact;

        [SerializeField] EnemyBase owner;

        [SerializeField] private Collider2D _collider2D;


        private void OnEnable()
        {
            this._collider2D.enabled = false;
        }

        public override float GetDamage(Unit target)
        {
            Remove();
            return this.owner.Stats.GetStat<Damage>(RPGStatType.Damage).StatValue;
        }
        
        void Remove()
        {
            if (this.prefabImpact != null)
            {
                LeanPool.Spawn(this.prefabImpact, transform.position, Quaternion.identity);
            }
        }
        
    }
}