using System;
using Cysharp.Threading.Tasks;
using Lean.Pool;
using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "Hero1Skill0", menuName = "BehaviorTree/Node/InputControl/Skill/Hero1Skill0")]
    public class Hero1Skill0 : SkillNode
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
            var spawnPosition = positionList ? positionList.GetSpawnPosition(this.skillType)[0] : new Vector3(0, 0);
            
            var bullet = LeanPool.Spawn(this.prefabBullet, spawnPosition, Quaternion.identity);
            var bulletBase = bullet.GetComponent<BulletBase>();
            bulletBase.InitBullet(new BulletData(2, faceRight ,20, this.Tree.Owner.Stats.GetStat<Damage>(RPGStatType.Damage).StatValue));
        }
    }
}