using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Data/UnitData")]
    public class EnemyData : UnitData
    {
        public int percentIncreaseHPbyLevel;
    }
}
