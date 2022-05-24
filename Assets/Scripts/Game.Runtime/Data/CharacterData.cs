using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "Data/UnitData")]
    public class CharacterData : UnitData
    {
        public float cooldownSkill1;
        public float cooldownSKill2;
        public float cooldownNormalAttack;
    }
}
