using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "UnitData", menuName = "Data/UnitData")]
    public class UnitData : ScriptableObject
    {
        public int hp;
        public int attack;
        public int speed;
        public int attackSpeed;
    }
}
