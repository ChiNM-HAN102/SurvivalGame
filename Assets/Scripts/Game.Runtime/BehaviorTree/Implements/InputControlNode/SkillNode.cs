namespace Game.Runtime
{
    public class SkillNode : InputControlNode
    {
        public SkillType skillType;
        public State state;

        protected Skill skill;

        public string skillAnim;
        public string idleName;
        
        protected bool successTrigger;

        protected bool startedSkill;
        
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
            if (this.Tree.Owner.UnitState.Current == this.state)
            {
                this.Tree.Owner.AnimController.DoAnim(this.idleName, State.IDLE);
            }
        }

        protected virtual void TriggerAction()
        {
            
        }

        protected override void OnStop()
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
                return NodeState.Success;
            }
            
            return NodeState.Running;
        }
    }
}