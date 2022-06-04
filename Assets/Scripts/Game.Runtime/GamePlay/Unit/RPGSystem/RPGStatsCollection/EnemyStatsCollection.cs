namespace Game.Runtime
{
    public class EnemyStatsCollection : RPGStatCollection
    {
        private EnemyData data;
        private int level;
        
        public EnemyStatsCollection(Unit unit, EnemyData data, int level) : base(unit)
        {
            this.data = data;
            this.level = level;
            
            ConfigStats();
        }

        public override void ConfigStats()
        {
            var health = CreateStat<Health>(RPGStatType.Health);
            health.StatBaseValue = this.data.hp * (1 + level * this.data.percentIncreaseHPbyLevel);
            health.CurrentValue = health.StatBaseValue;

            var moveSpeed = CreateStat<MoveSpeed>(RPGStatType.MoveSpeed);
            moveSpeed.StatBaseValue = this.data.speed;

            var damageAtk = CreateStat<Damage>(RPGStatType.Damage);
            damageAtk.StatBaseValue = this.data.attack;
        }
    }
}