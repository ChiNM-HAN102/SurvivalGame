using System.Linq;
using UnityEngine;

namespace Game.Runtime
{
    public class UnitBodyPositionController : MonoBehaviour
    {
        [SerializeField] private Transform[] spawnNormalAttackTransform;
        [SerializeField] private Transform[] spawnSkill1Transform;
        [SerializeField] private Transform[] spawnSkill2Transform;
        [SerializeField] private Transform[] spawnSkill3Transform;

        public Vector3[] GetSpawnPosition(SkillType type)
        {
            switch (type)
            {
                case SkillType.Skill1:
                    if (this.spawnSkill1Transform.Length > 0)
                    {
                        return this.spawnNormalAttackTransform.Select(x => x.position).ToArray();
                    }
                    break;
                case SkillType.NormalAttack:
                    if (this.spawnNormalAttackTransform.Length > 0)
                    {
                        return this.spawnNormalAttackTransform.Select(x => x.position).ToArray();
                    }
                    break;
                case SkillType.Skill2:
                    if (this.spawnSkill2Transform.Length > 0)
                    {
                        return this.spawnSkill2Transform.Select(x => x.position).ToArray();
                    }
                    break;
                case SkillType.Skill3:
                    if (this.spawnSkill3Transform.Length > 0)
                    {
                        return this.spawnSkill3Transform.Select(x => x.position).ToArray();
                    }
                    break;
                
            }
            
            return new []{Vector3.zero};
        }

    }
}