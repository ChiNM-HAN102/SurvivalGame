using System.Collections.Generic;

namespace Game.Runtime
{
    public class RPGStatModifiable : RPGStat, IStatModifiable
    {
        private readonly List<RPGStatModifier> _statModifiers;

        public override float StatValue => StatModifiedValue;
        
        public override float StatBaseValue
        {
            get => statBaseValue;
            set
            {
                this.statBaseValue = value;
                UpdateModifiers();
            }
        }

        public float StatModifiedValue { get; set; }

        public RPGStatModifiable()
        {
            StatModifiedValue = 0;
            _statModifiers = new List<RPGStatModifier>();
        }
        

        public void AddModifier(RPGStatModifier mod)
        {
            this._statModifiers.Add(mod);
            UpdateModifiers();
            
            this.onChanged?.Invoke(StatModifiedValue);
        }

        public void RemoveModifier(RPGStatModifier mod)
        {
            this._statModifiers.Remove(mod);
            UpdateModifiers();
            
            this.onChanged?.Invoke(StatModifiedValue);
        }

        public void ClearModifiers()
        {
            this._statModifiers.Clear();
       
            UpdateModifiers();
            this.onChanged?.Invoke(StatModifiedValue);
        }

        public void UpdateModifiers()
        {
            float baseAdd = 0, basePercent = 0, totalAdd = 0, totalPercent = 0;

            foreach (var rpgStatModifier in this._statModifiers)
            {
                switch (rpgStatModifier.type)
                {
                    case RPGModifierType.BaseAdd:
                        baseAdd += rpgStatModifier.Value;
                        break;
                    case RPGModifierType.BasePercent:
                        basePercent += rpgStatModifier.Value;
                        break;
                    case RPGModifierType.TotalAdd:
                        totalAdd += rpgStatModifier.Value;
                        break;
                    case RPGModifierType.TotalPercent:
                        totalPercent += rpgStatModifier.Value;
                        break;
                }
            }

            StatModifiedValue = (StatBaseValue * (1 + basePercent) + baseAdd) * (1 + totalPercent) + totalAdd;
        }
    }
}