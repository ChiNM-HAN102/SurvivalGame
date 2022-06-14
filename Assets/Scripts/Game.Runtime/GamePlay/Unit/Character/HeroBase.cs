using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class HeroBase : Unit
    {
        [SerializeField] private HeroData data;
        [SerializeField] protected string idleName;
        [SerializeField] private BehaviorTree tree;

        private BehaviorTree _cloneTree;

        public override UnitData Data { get => this.data;}

        protected override void Awake()
        {
            base.Awake();
            UnitState.Set(State.IDLE);
            
            InitSkill();
            this._cloneTree = this.tree.CloneTree();
            this._cloneTree.SetUpTree(this);
        }

        void InitSkill()
        {
            this.Skills?.UnRegisterSkill();
            
            var skillNormal = new Skill();
            skillNormal.InitData(this.data.attackSpeed);
            
            var skill1 = new Skill();
            skill1.InitData(this.data.cooldownSkill1);
            
            var skill2 = new Skill();
            skill2.InitData(this.data.cooldownSkill2);
            
            var dict = new Dictionary<SkillType, Skill> {
                {SkillType.NormalAttack, skillNormal},
                {SkillType.Skill1, skill1},
                {SkillType.Skill2, skill2}
            };
            
            Skills?.Init(dict);
            Skills?.RegisterSkill();
        }
        
        public virtual void SetInfo()
        {
            this.transform.localScale = new Vector3(1,1,1);
            this.faceRight = true;
            this.AnimController.DoAnim(this.idleName, State.IDLE);
            Stats = new HeroStatsCollection(this, data);
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                CurrentControlType = InputControlType.MOVE_RIGHT;
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                CurrentControlType = InputControlType.MOVE_LEFT;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                CurrentControlType = InputControlType.ATTACK;
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                CurrentControlType = InputControlType.USE_SKILL_1;
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                CurrentControlType = InputControlType.USE_SKILL_2;
            }

            var state = this._cloneTree.DoUpdate(deltaTime);
            
            CurrentControlType = InputControlType.NONE;
        }

        public void SetCurrentControlType(InputControlType inputControlType)
        {
            CurrentControlType = inputControlType;
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
    }
}