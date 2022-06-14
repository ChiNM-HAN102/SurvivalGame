using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Lean.Pool;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Runtime
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject endGamePanel;
        [SerializeField] private GameObject pauseGamePanel;
        [SerializeField] private GameObject startGamePanel;
        [SerializeField] private GameObject hurtPanel;
        [SerializeField] private GameObject startCountDown;
        [SerializeField] private Text startCountDownTxt;
        [SerializeField] private Text currentKilledEnemyTxt;
        [SerializeField] private Text currentTimeCountTxt;
        [SerializeField] private Text highestKilledEnemyTxt;
        [SerializeField] private Text highestTimeCountTxt;

        [SerializeField] private InfoUIController[] heroUiPrefabs;

        [SerializeField] private SkillButtonUI[] skillButtonUis;

        private CancellationTokenSource _ctsCountDownChangeHero;
        
        public static UIManager Instance { get; set; }

        private void Awake()
        {
            Instance = this;
        }

        public void InitCharacters(HeroBase[] characterList)
        {
            foreach (InfoUIController uiPrefab in this.heroUiPrefabs)
            {
                uiPrefab.gameObject.SetActive(false);
            }

            for (int i = 0; i < characterList.Length; i++)
            {
                this.heroUiPrefabs[i].InitData(characterList[i], i);
                this.heroUiPrefabs[i].gameObject.SetActive(true);
            }
        }

        public void SetSelectedHero(int idx, int countTime)
        {
            this._ctsCountDownChangeHero = new CancellationTokenSource();
            for (int i = 0; i < this.heroUiPrefabs.Length; i++)
            {
                if (i == idx)
                {
                    this.heroUiPrefabs[i].SetSelected(true);
                }
                else
                {
                    this.heroUiPrefabs[i].SetSelected(false);   
                    this.heroUiPrefabs[i].SetCountDown(countTime, this._ctsCountDownChangeHero.Token);
                }
            }

            foreach (SkillButtonUI skillButtonUi in this.skillButtonUis)
            {
                skillButtonUi.InitData();
            }
        }

        private bool _isHurting = false;
        public void PresentHurtAnimation()
        {
            if (!this._isHurting)
            {
                this._isHurting = true;
                HurtPresent().Forget();
            }
        }

        public void WaitGame()
        {
            var highestSurvival = PlayerPrefs.GetInt(Constants.DATA_HIGHEST_SURVIVAL_TIME_KEY);
            this.highestTimeCountTxt.text = "Highest Survived: " + highestSurvival + "s";

            var highestKilled = PlayerPrefs.GetInt(Constants.DATA_HIGHEST_KILL_ENEMY);
            this.highestKilledEnemyTxt.text = "Highest Killed: " + highestKilled;
            
            this.startGamePanel.SetActive(true);
            this.endGamePanel.SetActive(false);
            this.pauseGamePanel.SetActive(false);
            this.hurtPanel.SetActive(false);
            this.startCountDown.SetActive(false);
        }
        
        public void StartGame(int time, Action callBack)
        {
            this.startGamePanel.SetActive(false);
            this.endGamePanel.SetActive(false);
            this.pauseGamePanel.SetActive(false);
            this.hurtPanel.SetActive(false);
            this.startCountDown.SetActive(true);
            
            CountDown(time, callBack).Forget();
        }

        async UniTaskVoid CountDown(int time, Action callback)
        {
            for (int i = time; i >= 0; i--)
            {
                startCountDownTxt.text = (i).ToString();
                await UniTask.Delay(TimeSpan.FromSeconds(1), ignoreTimeScale: true);
            }
            
            this.startCountDown.gameObject.SetActive(false);
            callback?.Invoke();
        }

        public void EndGame()
        {
            this.startGamePanel.SetActive(false);
            this.endGamePanel.SetActive(true);
            this.pauseGamePanel.SetActive(false);
            this.hurtPanel.SetActive(false);
            this.startCountDown.SetActive(false);
        }
        
        public void PauseGame()
        {
            this.startGamePanel.SetActive(false);
            this.endGamePanel.SetActive(false);
            this.pauseGamePanel.SetActive(true);
            this.hurtPanel.SetActive(false);
            this.startCountDown.SetActive(false);
        }

        async UniTaskVoid HurtPresent()
        {
            for (int i = 0; i < 3; i++)
            {
                this.hurtPanel.SetActive(true);
                await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
                this.hurtPanel.SetActive(false);
                await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            }

            this._isHurting = false;
        }

        public void Clear()
        {
            this._ctsCountDownChangeHero.Cancel();
        }

        public void SetKillText(int number)
        {
            this.currentKilledEnemyTxt.text = "kill " + number;
        }
        

        public void SetCountDown(int number)
        {
            this.currentTimeCountTxt.text = number + "s";
        }
    }
}
