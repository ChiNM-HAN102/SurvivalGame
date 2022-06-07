using UnityEngine;

namespace Game.Runtime
{
    public class HeroBase : CharacterBase
    {
        [SerializeField] private HeroData data;
        [SerializeField] protected string _animSkill1 = "Attack_1";
        [SerializeField] protected string _animSkill2 = "Attack_3";
        [SerializeField] protected string _animSkill3 = "Attack_4";
        [SerializeField] protected string _animMove = "Walk";
        [SerializeField] protected string _animIdle = "Idle";
        [SerializeField] protected string _animDie = "Death";
        
        protected UnitState _state;
        protected Animator _animator;

        public HeroData Data { get => this.data; }
        protected override void Awake()
        {
            base.Awake();
            _animator = GetComponentInChildren<Animator>();
            this._state = UnitState.IDLE;
        }

        public virtual void SetInfo()
        {
            this.transform.localScale = new Vector3(1,1,1);
            this.faceRight = true;
            this._state = UnitState.IDLE;
            this._animator.Play(this._animIdle);
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