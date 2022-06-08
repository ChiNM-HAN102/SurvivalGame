using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Game.Runtime.Impact;
using Lean.Pool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Runtime
{
    public class EnemyBase : Unit
    {
        [SerializeField] public EnemyData data;

        [SerializeField] private EnemyNormalDamageBox _damageBox;

        protected HealthBarController _healthBarController;
        
        protected Animator _animator;
        protected UnitState _state;
        protected HeroBase target;

        [SerializeField] protected string _animAttack = "Attack_1";
        [SerializeField] protected string _animMove = "Walk";
        [SerializeField] protected string _animIdle = "Idle";
        [SerializeField] protected string _animHurt = "Hurt";
        [SerializeField] protected string _animDie = "Death";

        protected override void Awake()
        {
            this._animator = GetComponentInChildren<Animator>();
            this._healthBarController = GetComponentInChildren<HealthBarController>();
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
            
            this._damageBox.ToggleActive(false);
            
            SoundController.Instance.PlayCallEnemy();
        }

        private float currentAttackCoolDown = 0;

        public override void Remove()
        {
            
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            if (this._state == UnitState.HURT || this._state == UnitState.DIE || this._state == UnitState.ATTACK)
            {
                return;
            }

            this.currentAttackCoolDown += deltaTime;
            
            
            target = FindTarget();
            
            if(this.target != null)
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
                    if (this.currentAttackCoolDown > data.attackSpeed)
                    {
                        this.currentAttackCoolDown = 0;
                        if (this._state != UnitState.ATTACK)
                        {
                            this._state = UnitState.ATTACK;
                            this._animator.Play(this._animAttack);
                            PLayAttack().Forget();
                        }
                    }
                    else
                    {
                        if (this._state == UnitState.NONE || this._state == UnitState.MOVE)
                        {
                            this._state = UnitState.IDLE;
                            this._animator.Play(this._animIdle);
                        }
                    }

                }
            }
        }

        async UniTaskVoid PLayAttack()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            if (this._state == UnitState.ATTACK)
            {
                this._damageBox.ToggleActive(true);
                await UniTask.Yield();
                this._damageBox.ToggleActive(false);
            }

            if (this._state == UnitState.ATTACK)
            {
                await WaitUntilFinishAnim(this._animator);
                this._state = UnitState.NONE;
            }
        }

        protected virtual HeroBase FindTarget()
        {
            return GamePlayController.Instance.GetSelectedHero();
        }

        public override void Flip()
        {
            base.Flip();

            var newScale = this._healthBarController.transform.localScale;
            newScale.x = -newScale.x;
            _healthBarController.transform.localScale = newScale;
        }

        public override void GetHurt(float damageInfo)
        {
            
            base.GetHurt(damageInfo);
            
            UIManager.Instance.CreateFloatingText("-" + damageInfo, new Color32(219, 64, 53, 255),  
                new Vector2(transform.position.x, transform.position.y + 1.5f));
            
            SoundController.Instance.PlayEnemyHurt();
            
            if (Stats.GetStat<Health>(RPGStatType.Health).CurrentValue <= 0 && this._state != UnitState.DIE)
            {
                GamePlayController.Instance.IncreaseTotalKillEnemy();
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

            if (this.data.dropItems.Length > 0)
            {
                var random = Random.Range(0, 0.999f);
                if (random < this.data.percentDropItems)
                {
                    var itemRandomIdx = Random.Range(0, this.data.dropItems.Length);
                    var dropItem = this.data.dropItems[itemRandomIdx];
                    LeanPool.Spawn(dropItem, new Vector2(transform.position.x,-3f), Quaternion.identity);
                }
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