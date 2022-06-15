using Game.Runtime.Impact;
using Lean.Pool;
using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "EnemyWoodRaiseAttack", menuName = "BehaviorTree/Node/Action/EnemyWoodRaiseAttack")]
    public class EnemyWoodRaiseAttack : Node
    {
        public string attackName;
        public GameObject woodPrefab;

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
                
                var triggerEvent = owner.GetComponentInChildren<SkillTriggerEvent>();
                if (triggerEvent != null)
                {
                    triggerEvent.StartAction = TriggerAction;
                }
                
                this._successTrigger = false;
            }
        }
        
        private void TriggerAction()
        {
            var spawnPositions = Tree.Owner.GetComponent<UnitBodyPositionController>()
                .GetSpawnPosition(SkillType.NormalAttack);

            foreach (Vector3 position in spawnPositions)
            {
                var go  = LeanPool.Spawn(this.woodPrefab, position, Quaternion.identity);
                var enemyNormalDamageBox = go.GetComponent<EnemyNormalDamageBox>();
                enemyNormalDamageBox.Init(Tree.Owner);

                if (Tree.Owner.GetFaceRight())
                {
                    go.transform.localScale = new Vector2(1, 1);
                }
                else
                {
                    go.transform.localScale = new Vector2(-1, 1);
                }
            }
        }

        void EndAction()
        {
            this._successTrigger = true;
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