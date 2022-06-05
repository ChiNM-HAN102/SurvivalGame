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

        public override void GetHurt(float damageInfo)
        {
            base.GetHurt(damageInfo);

            if (IsAlive)
            {
                UIManager.Instance.PresentHurtAnimation();
                CameraController.Instance.SetShakeDuration(0.3f);
            }
        }
    }
}