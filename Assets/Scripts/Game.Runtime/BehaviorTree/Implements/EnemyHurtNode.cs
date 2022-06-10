using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "EnemyHurtNode", menuName = "BehaviorTree/Node/Action/EnemyHurtNode")]
    public class EnemyHurtNode : ActionNode
    {
        private EnemyBase _owner;
        
        public string animHurt;
        public string animIdle;

        private bool _endHurtProcess;
        
        protected override void OnStart()
        {
            this._endHurtProcess = false;
            this._owner = this.tree.Owner.GetComponent<EnemyBase>();
            this._owner.DoAnim(this.animHurt);
            EndHurt().Forget();
        }

        protected override void OnStop()
        {
            this.tree.Owner.UnitState.Set(State.IDLE);
            this.tree.Owner.DoAnim(this.animIdle);
        }

        protected override NodeState OnUpdate(float deltaTime)
        {
            if (this._endHurtProcess)
            {
                return NodeState.Success;
            }

            return NodeState.Running;
        }
        
        async UniTaskVoid EndHurt()
        {
            await Utilities.WaitUntilFinishAnim(this._owner.animator);
            this._endHurtProcess = true;
        }
    }
}