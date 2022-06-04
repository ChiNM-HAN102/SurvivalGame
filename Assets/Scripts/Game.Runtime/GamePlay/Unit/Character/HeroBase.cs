using UnityEngine;

namespace Game.Runtime
{
    public class HeroBase : CharacterBase
    {
        [SerializeField] private HeroData data;
        
        public HeroData Data { get => this.data; }
        protected override void Awake()
        {
            base.Awake();
            SetInfo();
        }

        public virtual void SetInfo()
        {
            Stats = new HeroStatsCollection(this, data);
        }
    }
}