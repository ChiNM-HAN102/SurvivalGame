using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Lean.Pool;
using UnityEngine;

namespace Game.Runtime
{
    public class DamageBox : Dummy, IGetDamage
    {

        public override void OnUpdate(float deltaTime)
        {
            
        }


        public virtual float GetDamage(Unit target)
        {
            return 20;
        }
    }
}
