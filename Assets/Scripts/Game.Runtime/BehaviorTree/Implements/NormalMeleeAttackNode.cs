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

        private Skill _skill;

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

        protected override void OnStop()
        {
            
        }

        protected override NodeState OnUpdate(float deltaTime)
        {

            if (this._skill == null)
            {
                CurrentNodeState = NodeState.Failure;
                return CurrentNodeState;
            }

            if (this._successTrigger)
            {
                CurrentNodeState = NodeState.Success;
                return CurrentNodeState;
            }

            CurrentNodeState = NodeState.Running;
            return CurrentNodeState;
        }
    }
}