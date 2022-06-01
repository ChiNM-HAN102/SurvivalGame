using UnityEngine;

namespace Game.Runtime
{
    public class Health : RPGStatModifiable
    {
        private float currentValue;
        
        public float CurrentValue
        {
            get => Mathf.Clamp(currentValue, 0, StatValue);
            set => this.currentValue = Mathf.Clamp(value, 0, StatValue);
        }

        public bool IsFull => CurrentValue >= StatValue;

        public float CalculateCurrentPercent => StatValue > 0 ? currentValue / StatValue : 0;

        public void TakeDamage(float value)
        {
            if (value > 0)
            {
                CurrentValue -= value;
            }
        }
        
        public void Heal(float value)
        {
            if (value > 0)
            {
                CurrentValue += value;
            }
        }

        public void Clear()
        {
            CurrentValue = 0;
        }
    }
}