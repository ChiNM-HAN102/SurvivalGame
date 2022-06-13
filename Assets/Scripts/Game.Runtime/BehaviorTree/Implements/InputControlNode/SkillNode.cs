namespace Game.Runtime
{
    public class SkillNode : InputControlNode
    {
        public SkillType skillType;
        public State state;

        protected Skill skill;

        public string skillAnim;
        
        protected bool successTrigger;

        protected bool startedSkill;

        protected override void Prepare()
        {
            if (Tree.Owner.UnitState.Current != State.ATTACK)
            {
                SetStarted(false);
            }
        }

        protected override void OnStart()
        {
            var owner = this.Tree.Owner;
            if (owner.CurrentControlType == this.controlType)
            {
                skill = owner.Skills.GetSkill(this.skillType);

                if (skill != null)
                {
                    this.startedSkill = true;
                    owner.AnimController.DoAnim(this.skillAnim, state, EndAction);

                    this.skill.StartCoolDown();
                    this.successTrigger = false;

                    var triggerEvent = owner.GetComponentInChildren<SkillTriggerEvent>();
                    if (triggerEvent != null)
                    {
                        triggerEvent.StartAction = TriggerAction;
                    }
                }
                else
                {
                    this.startedSkill = false;
                }
            }
            else
            {
                this.startedSkill = false;
            }
        }
        
        protected virtual void EndAction()
        {
            this.successTrigger = true;
        }

        protected virtual void TriggerAction()
        {
            
        }

        protected override NodeState OnUpdate(float deltaTime)
        {
            if (!this.startedSkill)
            {
                return NodeState.Failure;
            }

            if (this.successTrigger)
            {
                CurrentNodeState = NodeState.Success;
                return CurrentNodeState;
            }

            CurrentNodeState = NodeState.Running;
            return CurrentNodeState;
        }
    }
}