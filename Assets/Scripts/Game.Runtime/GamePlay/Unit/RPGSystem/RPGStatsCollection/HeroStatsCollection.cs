
namespace Game.Runtime
{
    public class HeroStatsCollection : RPGStatCollection
    {
        private CharacterData data;
        
        public HeroStatsCollection(Unit unit, CharacterData data) : base(unit)
        {
            this.data = data;
            ConfigStats();
        }

        public override void ConfigStats()
        {
            var health = CreateStat<Health>(RPGStatType.Health);
            health.StatBaseValue = this.data.hp;
            health.CurrentValue = health.StatBaseValue;

            var moveSpeed = CreateStat<MoveSpeed>(RPGStatType.MoveSpeed);
            moveSpeed.StatBaseValue = this.data.speed;

            var damageAtk = CreateStat<Damage>(RPGStatType.Damage);
            damageAtk.StatBaseValue = this.data.attack;
        }
    }
}