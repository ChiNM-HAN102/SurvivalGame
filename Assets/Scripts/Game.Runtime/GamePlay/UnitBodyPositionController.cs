using UnityEngine;

namespace Game.Runtime
{
    public class UnitBodyPositionController : MonoBehaviour
    {
        [SerializeField] private Transform _spawnNormalAttackTransform;
        [SerializeField] private Transform _spawnSkill1Transform;
        [SerializeField] private Transform _spawnSkill2Transform;
        [SerializeField] private Transform _spawnSkill3Transform;

        public Vector2 GetSpawnPosition(SkillType type)
        {
            switch (type)
            {
                case SkillType.Skill1:
                    if (this._spawnSkill1Transform)
                    {
                        return this._spawnSkill1Transform.transform.position;
                    }
                    break;
                case SkillType.NormalAttack:
                    if (this._spawnNormalAttackTransform)
                    {
                        return this._spawnNormalAttackTransform.transform.position;
                    }
                    break;
                case SkillType.Skill2:
                    if (this._spawnSkill2Transform)
                    {
                        return this._spawnSkill2Transform.transform.position;
                    }
                    break;
                case SkillType.Skill3:
                    if (this._spawnSkill3Transform)
                    {
                        return this._spawnSkill3Transform.transform.position;
                    }
                    break;
                
            }
            
            return new Vector2(0,0);
        }

    }
}