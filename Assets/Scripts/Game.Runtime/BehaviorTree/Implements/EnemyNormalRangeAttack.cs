using Lean.Pool;
using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "NormalRangeAttack", menuName = "BehaviorTree/Node/Action/NormalRangeAttack")]
    public class EnemyNormalRangeAttack: Node
    {
        public string attackName;
        public GameObject bullet;
        public float forceX;
        public float forceY;
        
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
                var go  = LeanPool.Spawn(this.bullet, position, Quaternion.identity);
                var enemyBullet = go.GetComponent<EnemyBullet>();

                if (Tree.Owner.GetFaceRight())
                {
                    var force = new Vector3(this.forceX, this.forceY);
                    enemyBullet.InitData(Tree.Owner, force);   
                }
                else
                {
                    var force = new Vector3(-this.forceX, this.forceY);
                    enemyBullet.InitData(Tree.Owner, force);
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