using Game.Runtime.Impact;
using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "NormalMeleeAttackNode", menuName = "BehaviorTree/Node/Action/NormalMeleeAttackNode")]
    public class NormalMeleeAttackNode : Node
    {
        public string attackName;
        
        private bool _successTrigger;

        private SkillTriggerEvent _skillTriggerEvent;

        private Skill skill;
        
        protected override void OnStart()
        {
            var owner = this.tree.owner;
            skill = owner.Skills.GetSkill(SkillType.NormalAttack);

            if (skill != null && this.skill.CanUse)
            {
                owner.UnitState.Set(State.ATTACK);
                owner.DoAnim(this.attackName);

                this.skill.StartCoolDown();
                
                this._successTrigger = false;
            
                this._skillTriggerEvent = owner.GetComponent<SkillTriggerEvent>();

                if (this._skillTriggerEvent)
                {
                    this._skillTriggerEvent.StartAction = StartTrigger;
                }
            }
        }

        void StartTrigger()
        {
            this._successTrigger = true;
        }

        protected override void OnStop()
        {
            
        }

        protected override NodeState OnUpdate(float deltaTime)
        {

            if (!this._skillTriggerEvent || skill == null || this.skill.CanUse)
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