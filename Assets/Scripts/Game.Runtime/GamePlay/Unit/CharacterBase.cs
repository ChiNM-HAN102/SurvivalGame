﻿

namespace Game.Runtime
{
    public class CharacterBase : Unit
    {
        public virtual bool IsAlive
        {
            get
            {
                if (gameObject == null) return false;

                return gameObject.activeSelf && Stats.GetStat<Health>(RPGStatType.Health).CurrentValue > 0;
            }
        }

        public override void Remove()
        {
            
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            
            if (GamePlayController.Instance.State != GameState.RUNNING)
            {
                return;
            }
            
            
        }
    }
}