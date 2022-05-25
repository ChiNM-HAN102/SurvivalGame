namespace Game.Runtime
{
    public class RPGStatModifier
    {
        public float value;

        public Unit owner;

        public RPGModifierType type;

        public float Value
        {
            get => this.value;
            set => this.value = value;
        }

        public RPGStatModifier( RPGModifierType type, float value, Unit owner = null)
        {
            this.owner = owner;
            this.value = value;
            this.type = type;
        }
    }
}