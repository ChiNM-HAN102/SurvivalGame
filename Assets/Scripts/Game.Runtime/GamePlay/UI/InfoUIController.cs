using System;
using System.Threading;
using Cysharp.Threading.Tasks;
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

        [SerializeField] private Button btnSelect;
        [SerializeField] private Text countDown;
        [SerializeField] private GameObject countDownPanel;
        

        private HeroBase _heroBase;
        private int idx;
        private bool canSelect = false;
        private CancellationToken cancelToken;
        
        private bool isSelected;

        private void Awake()
        {
            this.btnSelect.onClick.AddListener(OnSelect);
        }

        void OnSelect()
        {
            SoundController.Instance.PlayClick();
            if (this.canSelect)
            {
                GamePlayController.Instance.SelectHero(idx);
            }
        }

        public void InitData(HeroBase heroBase, int idx)
        {
            this.idx = idx;
            this._heroBase = heroBase;
            
            this.avatar.sprite = ((HeroData)heroBase.Data).avatar;
            this.attackText.text = heroBase.Data.attack.ToString();

            this._heroBase.Stats.GetStat<Health>(RPGStatType.Health).onChanged = OnHealthChange;
            this._heroBase.Stats.GetStat<Damage>(RPGStatType.Damage).onChanged = OnAttackChange;

            this.bonusAttackText.text = "";
            this.countDownPanel.SetActive(false);
            this.canSelect = true;
            
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

        public void SetCountDown(int time, CancellationToken cancelToken)
        {
            this.countDownPanel.SetActive(true);
            this.canSelect = false;
            this.cancelToken = cancelToken;
            CountDown(time).Forget();
        }

        async UniTaskVoid CountDown(int time)
        {
            this.countDown.text = time.ToString();
            for (int i = 0; i < time; i++)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: this.cancelToken);
                this.countDown.text = (time - i).ToString();
            }

            this.canSelect = true;
            this.countDownPanel.SetActive(false);
        }
    }
}