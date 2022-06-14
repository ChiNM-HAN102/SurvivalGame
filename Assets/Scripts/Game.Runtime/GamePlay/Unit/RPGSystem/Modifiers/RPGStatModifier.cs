namespace Game.Runtime
{
    public class RPGStatModifier
    {
        public float value;

        public RPGModifierType type;

        public float Value
        {
            get => this.value;
            set => this.value = value;
        }

        public RPGStatModifier( RPGModifierType type, float value)
        {
            this.value = value;
            this.type = type;
        }
    }
}