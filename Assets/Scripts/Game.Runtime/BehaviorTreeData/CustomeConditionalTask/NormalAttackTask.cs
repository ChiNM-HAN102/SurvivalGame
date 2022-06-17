using BehaviorDesigner.Runtime.Tasks;

namespace Game.Runtime
{
    public class NormalAttackTask : Action
    {
        public string attackName;
        
        private bool _successTrigger;

        private SkillTriggerEvent _skillTriggerEvent;

        private Skill _skill;

        public override void OnStart()
        {
            base.OnStart();
            
            var owner = this.Owner.GetComponent<Unit>();
            this._skill = owner.Skills.GetSkill(SkillType.NormalAttack);

            if (this._skill != null)
            {
                owner.AnimController.DoAnim(this.attackName, State.ATTACK, EndAction);

                this._skill.StartCoolDown();
                
                this._successTrigger = false;
            }
        }
        
        void EndAction()
        {
            this._successTrigger = true;
        }

        public override TaskStatus OnUpdate()
        {
            if (this._skill == null)
            {
                
                return TaskStatus.Failure;
            }

            if (this._successTrigger)
            {
                return TaskStatus.Success;
            }
            
            return TaskStatus.Running;
        }
    }
}