using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "HeroData", menuName = "Data/HeroData")]
    public class HeroData : UnitData
    {
        public int coolDownChangeHero;
        public Sprite avatar;

        public int cooldownSkill1;
        public int cooldownSkill2;
    }
}
