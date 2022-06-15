using Lean.Pool;
using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "Hero3Skill2", menuName = "BehaviorTree/Node/InputControl/Skill/Hero3Skill2")]
    public class Hero3Skill2: InputControlNode
    {
        public float timeToDash;
        public string animDash;
        public SkillType skillType;
        public State state;
        public int timeMultiplySpeed;

        private float _currentTimeToFinishDash;
        private Vector3 _directionDash;
        private bool _startedSkill;
        private Skill _skill;
        
        protected override void Prepare()
        {
            if (Tree.Owner.UnitState.Current != this.state)
            {
                SetStarted(false);
            }
        }
        
        protected override void OnStart()
        {
            base.OnStart();
            var owner = this.Tree.Owner;
            if (owner.CurrentControlType == this.controlType)
            {
                this._skill = owner.Skills.GetSkill(this.skillType);

                if (this._skill != null)
                {
                    this._skill.StartCoolDown();
                    this._startedSkill = true;
                    _currentTimeToFinishDash = 0;

                    Tree.Owner.AnimController.DoAnim(this.animDash, this.state);
                    if (Tree.Owner.GetFaceRight())
                    {
                        this._directionDash = new Vector3(1, 0, 0);
                    }
                    else
                    {
                        this._directionDash = new Vector3(-1, 0, 0);
                    }
                }
                else
                {
                    this._startedSkill = false;
                }
            }
            else
            {
                this._startedSkill = false;
            }
        }

        protected override NodeState OnUpdate(float deltaTime)
        {
            if (!this._startedSkill)
            {
                return NodeState.Failure;
            }

            
            if (this._currentTimeToFinishDash >= this.timeToDash)
            {
                CurrentNodeState = NodeState.Success;
                return NodeState.Success;
            }

            var transform = this.Tree.Owner.transform;
            var speed = this.Tree.Owner.Stats.GetStat<MoveSpeed>(RPGStatType.MoveSpeed).StatValue * timeMultiplySpeed;
            transform.position = transform.position + this._directionDash * (deltaTime * speed);
            _currentTimeToFinishDash += deltaTime;
            CurrentNodeState = NodeState.Running;
            return CurrentNodeState;
        }
    }
}