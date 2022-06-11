using Game.Runtime.Impact;
using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "NormalMeleeAttackNode", menuName = "BehaviorTree/Node/Action/NormalMeleeAttackNode")]
    public class NormalMeleeAttackNode : Node
    {
        public string attackName;
        public string idleName;
        
        private bool _successTrigger;

        private SkillTriggerEvent _skillTriggerEvent;

        private Skill skill;
        
        protected override void OnStart()
        {
            var owner = this.Tree.Owner;
            skill = owner.Skills.GetSkill(SkillType.NormalAttack);

            if (skill != null && this.skill.CanUse)
            {
                owner.AnimController.DoAnim(this.attackName, State.ATTACK, EndAction);

                this.skill.StartCoolDown();
                
                this._successTrigger = false;
            }
        }

        void EndAction()
        {
            this._successTrigger = true;
            if (this.Tree.Owner.UnitState.Current == State.ATTACK)
            {
                this.Tree.Owner.AnimController.DoAnim(this.idleName, State.IDLE);
            }
        }

        protected override void OnStop()
        {
            
        }

        protected override NodeState OnUpdate(float deltaTime)
        {

            if (skill == null)
            {
                return NodeState.Failure;
            }

            if (this._successTrigger)
            {
                return NodeState.Success;
            }
            
            return NodeState.Running;
        }
    }
}