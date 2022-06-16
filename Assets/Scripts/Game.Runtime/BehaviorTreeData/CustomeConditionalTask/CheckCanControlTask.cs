using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Game.Runtime;
using UnityEngine;

namespace Game.Runtime
{
    public class CheckCanControlTask : Conditional
    {
        private Unit _owner;


        public override void OnStart()
        {
            base.OnStart();
            this._owner = this.Owner.GetComponent<Unit>();
        }

        public override TaskStatus OnUpdate()
        {
            if (this._owner.UnitState.CanUseSkill())
            {
                return TaskStatus.Success;
            }

            return TaskStatus.Running;
        }
    }
}
