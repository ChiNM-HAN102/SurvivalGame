namespace Game.Runtime
{
    public interface IStatModifiable
    {
        float StatModifiedValue { get; }

        void AddModifier(RPGStatModifier mod);
        void RemoveModifier(RPGStatModifier mod);
        void ClearModifiers();
        void UpdateModifiers();
    }
}