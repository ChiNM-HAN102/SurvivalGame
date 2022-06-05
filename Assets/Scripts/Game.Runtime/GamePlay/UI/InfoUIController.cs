using UnityEngine;
using UnityEngine.UI;

namespace Game.Runtime
{
    public class InfoUIController : MonoBehaviour
    {
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image avatar;
        
        [SerializeField] private Image healthBarFrame;
        [SerializeField] private Transform healthBar;

        [SerializeField] private Text attackText;
        [SerializeField] private Text bonusAttackText;

        [SerializeField] private Transform itemContainer;

        private HeroBase _heroBase;

        private bool isSelected;

        public void InitData(HeroBase heroBase)
        {
            this._heroBase = heroBase;
            
            this.avatar.sprite = heroBase.Data.avatar;
            this.attackText.text = heroBase.Data.attack.ToString();

            this._heroBase.Stats.GetStat<Health>(RPGStatType.Health).onChanged = OnHealthChange;
            this._heroBase.Stats.GetStat<Damage>(RPGStatType.Damage).onChanged = OnAttackChange;
            
            UpdateHealthBar();
        }

        void OnHealthChange(float value)
        {
            UpdateHealthBar();
        }

        void OnAttackChange(float value)
        {
            this.bonusAttackText.text = $"+ {value - this._heroBase.Data.attack}";
        }

        void UpdateHealthBar()
        {
           var ratio =  _heroBase.Stats.GetStat<Health>(RPGStatType.Health).CurrentValue / this._heroBase.Data.hp;
           this.healthBarFrame.fillAmount = ratio / 1;
        }

        public void AddInventory()
        {
            
        }

        public void RemoveInventory()
        {
            
        }

        public void SetSelected(bool value)
        {
            this.isSelected = value;
            if (this.isSelected)
            {
                transform.localScale = new Vector3(1,1,1);
                this.backgroundImage.color = new Color32(255,255,255, 255);
            }
            else
            {
                transform.localScale = new Vector3(0.75f,0.75f,0.75f);
                this.backgroundImage.color = new Color32(255,255,255, 150);
            }
        }
    }
}