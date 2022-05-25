using System;
using UnityEngine.Events;

namespace Game.Runtime
{
    public class RPGStat
    {
        protected float statBaseValue;
        public virtual float StatValue => StatBaseValue;

        public delegate void OnChanged();

        public OnChanged onChanged;
        
        public virtual float StatBaseValue
        {
            get => this.statBaseValue;
            set
            {
                this.statBaseValue = value;
                this.onChanged?.Invoke();
            }
        }

        public RPGStat()
        {
            this.statBaseValue = 0;
        }
    }
}