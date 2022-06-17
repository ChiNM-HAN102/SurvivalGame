using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Game.Runtime
{
    public class FindHeroTask : Action
    {
        private Unit _owner;
        
        public override void OnStart()
        {
            base.OnStart();
            this._owner = Owner.GetComponent<Unit>();
        }

        public override TaskStatus OnUpdate()
        {
            var hero = GamePlayController.Instance.GetSelectedHero();
            if (hero == null)
            {
                return TaskStatus.Running;
            }

            this._owner.Target = hero;

            return TaskStatus.Success;
        }
    }
}
