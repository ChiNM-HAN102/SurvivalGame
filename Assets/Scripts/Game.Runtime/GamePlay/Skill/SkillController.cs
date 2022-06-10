using System.Collections.Generic;

namespace Game.Runtime
{
    public enum SkillType
    {
        NormalAttack,
        Skill1,
        Skill2,
        Skill3
    }
    
    public class SkillController
    {
        protected Dictionary<SkillType, Skill> dictSkill;

        public void Init(Dictionary<SkillType, Skill> inputDictSkill)
        {
            this.dictSkill = inputDictSkill;
        }

        public Skill GetSkill(SkillType type)
        {
            return this.dictSkill[type];
        }

        public void RegisterSkill()
        {
            if (GlobalUpdateSystem.Instance != null)
            {
                var allSkills = this.dictSkill.Values;
                foreach (var skill in allSkills)
                {
                    GlobalUpdateSystem.Instance.Add(skill);
                }
            }
           
        }

        public void UnRegisterSkill()
        {
            if (GlobalUpdateSystem.Instance != null)
            {
                var allSkills = this.dictSkill.Values;
                foreach (var skill in allSkills)
                {
                    GlobalUpdateSystem.Instance.Remove(skill);
                }
            }
        }
    }
}