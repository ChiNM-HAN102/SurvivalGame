using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Runtime.Impact;
using Lean.Pool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Runtime
{
    public class EnemyBase : Unit
    {
        [SerializeField] private EnemyData data;
        [SerializeField] private BehaviorTree tree;

        protected HealthBarController _healthBarController;

        public override UnitData Data { get => this.data;}

        protected override void Awake()
        {
            base.Awake();
            this.tree.SetUpTree(this);
            this._healthBarController = GetComponentInChildren<HealthBarController>();
            InitSkill();
        }

        void InitSkill()
        {
            this.Skills?.UnRegisterSkill();
            
            var skillNormal = new Skill();
            skillNormal.InitData(this.data.attackSpeed);
            var dict = new Dictionary<SkillType, Skill> {{SkillType.NormalAttack, skillNormal}};
            
            Skills?.Init(dict);
            Skills?.RegisterSkill();
        }

        public virtual void SetInfo(int level)
        {
            Stats = new EnemyStatsCollection(this, data, level);

            this._healthBarController.InitData(this);
            this._healthBarController.transform.localScale = new Vector3(1,1,1);
            this.transform.localScale = new Vector3(1,1,1);
            this.faceRight = false;

            SoundController.Instance.PlayCallEnemy();
        }
        

        public override void Remove()
        {
            
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            this.tree.DoUpdate(deltaTime);
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
                UnitState.Set(State.DIE);
            }
            else
            {
                UnitState.Set(State.HURT);
            }
        }

    }
}