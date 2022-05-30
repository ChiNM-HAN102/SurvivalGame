﻿using System;
using UnityEngine;

namespace Game.Runtime
{
  
    
    public class BodyCollider : MonoBehaviour
    {
        public Unit Owner;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!this.Owner.IsAlive)
            {
                return;
            }

            var objDamage = other.GetComponent<IGetDamage>();

            var damageInfo = objDamage?.GetDamage(this.Owner);

            if (damageInfo != null)
            {
                TakeDamage((float)damageInfo);
            }
        }

        public void TakeDamage(float damageInfo)
        {
            this.Owner.GetHurt(damageInfo);
        }
    }
}