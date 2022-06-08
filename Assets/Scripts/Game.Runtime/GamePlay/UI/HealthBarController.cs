using UnityEngine;

namespace Game.Runtime
{
    public class HealthBarController : MonoBehaviour
    {
        [SerializeField] private GameObject healthBarFrame;
        [SerializeField] private Transform healthBar;
        
        private Unit _characterBase;
        
        public void InitData(Unit characterBase)
        {
            this._characterBase = characterBase;
            
            this._characterBase.Stats.GetStat<Health>(RPGStatType.Health).onChanged = OnHealthChange;
            
            UpdateHealthBar();
        }
        
        void OnHealthChange(float value)
        {
            UpdateHealthBar();
        }
        

        void UpdateHealthBar()
        {
            var ratio =  _characterBase.Stats.GetStat<Health>(RPGStatType.Health).CurrentValue / _characterBase.Stats.GetStat<Health>(RPGStatType.Health).StatValue;
            this.healthBarFrame.transform.localScale = new Vector3(ratio, 1, 1);
        }
    }
}