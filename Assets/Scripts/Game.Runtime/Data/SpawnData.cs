using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "SpawnData", menuName = "Data/SpawnData")]
    public class SpawnData : ScriptableObject
    {
        public float timeCountDownSpawnEnemy;
        public EnemyBase[] enemyBases;
    }
}