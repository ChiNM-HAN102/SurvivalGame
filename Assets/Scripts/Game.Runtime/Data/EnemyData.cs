using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Data/EnemyData")]
    public class EnemyData : UnitData
    {
        public float percentIncreaseHPbyLevel;
        public float meleeDetect;
        public float rangeDetect;

        public float percentDropItems;
        public InventoryBase[] dropItems;
    }
}
