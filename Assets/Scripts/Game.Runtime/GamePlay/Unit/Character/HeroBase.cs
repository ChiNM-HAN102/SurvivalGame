using UnityEngine;

namespace Game.Runtime
{
    public class HeroBase : Unit
    {
        [SerializeField] private HeroData data;
        [SerializeField] protected string _animSkill1 = "Attack_1";
        [SerializeField] protected string _animSkill2 = "Attack_3";
        [SerializeField] protected string _animSkill3 = "Attack_4";
        [SerializeField] protected string _animMove = "Walk";
        [SerializeField] protected string _animIdle = "Idle";
        [SerializeField] protected string _animDie = "Death";
        
        protected Animator _animator;

        public HeroData Data { get => this.data; }
        protected override void Awake()
        {
            base.Awake();
            _animator = GetComponentInChildren<Animator>();
            UnitState.Set(State.IDLE);
        }

        public virtual void SetInfo()
        {
            this.transform.localScale = new Vector3(1,1,1);
            this.faceRight = true;
            UnitState.Set(State.IDLE);
            this._animator.Play(this._animIdle);
            Stats = new HeroStatsCollection(this, data);
        }

        public override void Remove()
        {
            
        }

        public override void GetHurt(float damageInfo)
        {
            base.GetHurt(damageInfo);

            if (IsAlive)
            {
                SoundController.Instance.PlayHeroHurt();
                UIManager.Instance.PresentHurtAnimation();
                CameraController.Instance.SetShakeDuration(0.3f);
            }
            else
            {
                GamePlayController.Instance.EndGame();
            }
        }

        public void AddInventory()
        {
            
        }
    }
}