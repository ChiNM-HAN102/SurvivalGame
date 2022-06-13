using Lean.Pool;
using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "Hero1Skill2", menuName = "BehaviorTree/Node/InputControl/Skill/Hero1Skill2")]
    public class Hero1Skill2 : SkillNode
    {
        public GameObject prefabBullet;
        
        protected override void TriggerAction()
        {
            base.TriggerAction();
            SpawnBullet();
        }
        
        void SpawnBullet()
        {
            var faceRight = this.Tree.Owner.GetFaceRight();

            var positionList = this.Tree.Owner.GetComponent<UnitBodyPositionController>();
            var spawnPosition = positionList ? positionList.GetSpawnPosition(this.skillType) : new Vector2(0, 0);
            
            var bullet = LeanPool.Spawn(this.prefabBullet, spawnPosition, Quaternion.identity);
            var bulletBase = bullet.GetComponent<BulletBase>();
            bulletBase.InitBullet(new BulletData(2, faceRight ,20, this.Tree.Owner.Stats.GetStat<Damage>(RPGStatType.Damage).StatValue));
        }
    }
}