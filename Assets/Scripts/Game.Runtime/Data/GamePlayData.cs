using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "GamePlayData", menuName = "Data/GamePlayData")]
    public class GamePlayData : ScriptableObject
    {
        public int timeCountDownStartGame;
        public int timeCountDownSpawnEnemy;
        public HeroBase[] heroBases;
        public EnemyBase[] enemyBases;
    }
}