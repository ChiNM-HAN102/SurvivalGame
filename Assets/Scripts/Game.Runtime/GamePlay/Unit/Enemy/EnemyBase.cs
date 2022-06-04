using System;
using Cysharp.Threading.Tasks;
using Lean.Pool;
using UnityEngine;

namespace Game.Runtime
{
    public class EnemyBase : CharacterBase
    {
        [SerializeField] public EnemyData data;

        protected HealthBarController _healthBarController;
        
        protected Animator _animator;
        protected UnitState _state;
        protected HeroBase target;

        protected string _animAttack;
        protected string _animMove;
        protected string _animIdle;
        protected string _animHurt;
        protected string _animDie;

        protected override void Awake()
        {
            this._animAttack = "Attack_1";
            this._animMove = "Walk";
            this._animIdle = "Idle";
            this._animHurt = "Hurt";
            this._animDie = "Death";
            
           
            this._animator = GetComponentInChildren<Animator>();
            this._healthBarController = GetComponentInChildren<HealthBarController>();
            
            this.faceRight = false;
        }

        public virtual void SetInfo(int level)
        {
            Stats = new EnemyStatsCollection(this, data, level);
            this._healthBarController.InitData(this);

            this._healthBarController.transform.localScale = new Vector3(1,1,1);
            this.transform.localScale = new Vector3(1,1,1);
            this.faceRight = false;
            this._state = UnitState.IDLE;
            this._animator.Play(this._animIdle);
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            if (this._state == UnitState.HURT || this._state == UnitState.DIE)
            {
                return;
            }
            
            if (this.target == null)
            {
                target = FindTarget();
            }
            else
            {
                
                
                if (this.target.transform.position.x > transform.position.x && !this.faceRight || 
                    this.target.transform.position.x < transform.position.x && this.faceRight)
                {
                    Flip();
                }

                if (Vector2.Distance(this.target.transform.position, transform.position) > data.meleeDetect)
                {
                    transform.position = Vector3.MoveTowards(transform.position, this.target.transform.position,
                        Stats.GetStat<MoveSpeed>(RPGStatType.MoveSpeed).StatValue * deltaTime);
                    
                    if (this._state != UnitState.MOVE)
                    {
                        this._state = UnitState.MOVE;
                        this._animator.Play(this._animMove);
                    }
                }
                else
                {
                    if (this._state != UnitState.IDLE)
                    {
                        this._state = UnitState.IDLE;
                        this._animator.Play(this._animIdle);
                    }
                }
            }
        }

        protected virtual HeroBase FindTarget()
        {
            return GamePlayController.Instance.GetSelectedHero();
        }

        protected override void Flip()
        {
            base.Flip();

            var newScale = this._healthBarController.transform.localScale;
            newScale.x = -newScale.x;
            _healthBarController.transform.localScale = newScale;
        }

        public override void GetHurt(float damageInfo)
        {
            base.GetHurt(damageInfo);
            
            if (Stats.GetStat<Health>(RPGStatType.Health).CurrentValue <= 0 && this._state != UnitState.DIE)
            {
                this._state = UnitState.DIE;
                this._animator.Play(this._animDie);
                Die().Forget();
            }
            else
            {
                this._state = UnitState.HURT;
                this._animator.Play(this._animHurt);
                EndHurt().Forget();
            }
        }

        async UniTaskVoid Die()
        {
            await WaitUntilFinishAnim(this._animator);
            var renderer = GetComponentInChildren<SpriteRenderer>();
            for (int i = 0; i < 4; i++)
            {
                renderer.color = new Color32(255, 255, 255 , 100);
                await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
                renderer.color = new Color32(255, 255, 255 , 255);
                await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            }
            LeanPool.Despawn(gameObject);
        }

        async UniTaskVoid EndHurt()
        {
            await WaitUntilFinishAnim(this._animator);

            if (this._state == UnitState.HURT)
            {
                this._state = UnitState.IDLE;
            }
        }
    }
}