using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "SpawnData", menuName = "Data/SpawnData")]
    public class SpawnData : ScriptableObject
    {
        public int timeCountDownSpawnEnemy;
        public EnemyBase[] enemyBases;
    }
}