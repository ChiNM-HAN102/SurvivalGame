namespace Game.Runtime
{
    public class HeroBase : CharacterBase
    {
        protected override void Awake()
        {
            base.Awake();
            SetInfo();
        }

        public virtual void SetInfo()
        {
            Stats = new RPGStatCollection(this);
            
            
        }
    }
}