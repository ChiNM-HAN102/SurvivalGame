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
        protected HeroBase target;

        [SerializeField] protected string _animAttack = "Attack_1";
        [SerializeField] protected string _animMove = "Walk";
        [SerializeField] protected string _animIdle = "Idle";
        [SerializeField] protected string _animHurt = "Hurt";
        [SerializeField] protected string _animDie = "Death";

        protected override void Awake()
        {
            base.Awake();
            this._healthBarController = GetComponentInChildren<HealthBarController>();
        }

        public virtual void SetInfo(int level)
        {
            Stats = new EnemyStatsCollection(this, data, level);
            this._healthBarController.InitData(this);
            this._healthBarController.transform.localScale = new Vector3(1,1,1);
            this.transform.localScale = new Vector3(1,1,1);
            this.faceRight = false;
            UnitState.Set(State.IDLE);
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

            if (UnitState.Current == State.HURT || UnitState.Current == State.DIE || UnitState.Current == State.ATTACK)
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
                    
                    if (UnitState.Current != State.MOVE)
                    {
                        UnitState.Set(State.MOVE);
                        this._animator.Play(this._animMove);
                    }
                }
                else
                {
                    if (this.currentAttackCoolDown > data.attackSpeed)
                    {
                        this.currentAttackCoolDown = 0;
                        if (UnitState.Current != State.ATTACK)
                        {
                            UnitState.Set(State.ATTACK);
                            this._animator.Play(this._animAttack);
                            PLayAttack().Forget();
                        }
                    }
                    else
                    {
                        if (UnitState.Current == State.NONE || UnitState.Current == State.MOVE)
                        {
                            UnitState.Set(State.IDLE);
                            this._animator.Play(this._animIdle);
                        }
                    }

                }
            }
        }

        async UniTaskVoid PLayAttack()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            if (UnitState.Current == State.ATTACK)
            {
                this._damageBox.ToggleActive(true);
                await UniTask.Yield();
                this._damageBox.ToggleActive(false);
            }

            if (UnitState.Current == State.ATTACK)
            {
                await WaitUntilFinishAnim(this._animator);
                UnitState.Set(State.NONE);
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
            
            if (Stats.GetStat<Health>(RPGStatType.Health).CurrentValue <= 0 && UnitState.Current != State.DIE)
            {
                GamePlayController.Instance.IncreaseTotalKillEnemy();
                UnitState.Set(State.DIE);
                this._animator.Play(this._animDie);
                Die().Forget();
            }
            else
            {
                UnitState.Set(State.HURT);
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

            if (this.UnitState.Current == State.HURT)
            {
                UnitState.Set(State.IDLE);
            }
        }
    }
}