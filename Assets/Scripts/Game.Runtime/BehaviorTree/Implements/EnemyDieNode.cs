using System;
using Cysharp.Threading.Tasks;
using Lean.Pool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "EnemyDieNode", menuName = "BehaviorTree/Node/Action/EnemyDieNode")]
    public class EnemyDieNode : ActionNode
    {
        private EnemyBase _owner;
        
        public string animDie;
        public string animIdle;

        private bool _endDieProcess;
        
        protected override void OnStart()
        {
            this._endDieProcess = false;
            this._owner = this.tree.Owner.GetComponent<EnemyBase>();
            this._owner.DoAnim(this.animDie);
            Die().Forget();
        }

        protected override void OnStop()
        {
            GamePlayController.Instance.IncreaseTotalKillEnemy();
            LeanPool.Despawn(this.tree.Owner.gameObject);
            
            this.tree.Owner.UnitState.Set(State.IDLE);
            this.tree.Owner.DoAnim(this.animIdle);
        }

        protected override NodeState OnUpdate(float deltaTime)
        {
            if (this._endDieProcess)
            {
                return NodeState.Success;
            }

            return NodeState.Running;
        }
        
        async UniTaskVoid Die()
        {
            await Utilities.WaitUntilFinishAnim(_owner.animator);
            var renderer = this.tree.Owner.GetComponentInChildren<SpriteRenderer>();
            if (renderer)
            {
                for (int i = 0; i < 4; i++)
                {
                    renderer.color = new Color32(255, 255, 255 , 100);
                    await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
                    renderer.color = new Color32(255, 255, 255 , 255);
                    await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
                }
            }

            var data = (EnemyData)_owner.Data;
            if (data.dropItems.Length > 0)
            {
                var random = Random.Range(0, 0.999f);
                if (random < data.percentDropItems)
                {
                    var itemRandomIdx = Random.Range(0, data.dropItems.Length);
                    var dropItem = data.dropItems[itemRandomIdx];
                    LeanPool.Spawn(dropItem, new Vector2(this._owner.transform.position.x,-3f), Quaternion.identity);
                }
            }

            this._endDieProcess = true;
        }
    }
}