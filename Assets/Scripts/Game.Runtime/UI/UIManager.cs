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
        
        [SerializeField] private Text currentEnemyTxt;
        [SerializeField] private Text currentKilledEnemyTxt;
        [SerializeField] private Text currentTimeCountTxt;

        [SerializeField] private FloatingText floatTextPrefab;


        [SerializeField] private InfoUIController[] heroUIPrefabs;

        private CancellationTokenSource ctsCountDownChangeHero;
        
        public static UIManager Instance { get; set; }

        private void Awake()
        {
            Instance = this;
        }

        public void InitCharacters(HeroBase[] characterList)
        {
            foreach (InfoUIController uiPrefab in this.heroUIPrefabs)
            {
                uiPrefab.gameObject.SetActive(false);
            }

            for (int i = 0; i < characterList.Length; i++)
            {
                this.heroUIPrefabs[i].InitData(characterList[i], i);
                this.heroUIPrefabs[i].gameObject.SetActive(true);
            }
        }

        public void SetSelectedHero(int idx, int countTime)
        {
            this.ctsCountDownChangeHero = new CancellationTokenSource();
            for (int i = 0; i < this.heroUIPrefabs.Length; i++)
            {
                if (i == idx)
                {
                    this.heroUIPrefabs[i].SetSelected(true);
                }
                else
                {
                    this.heroUIPrefabs[i].SetSelected(false);   
                    this.heroUIPrefabs[i].SetCountDown(countTime, this.ctsCountDownChangeHero.Token);
                }
            }
        }

        private bool isHurting = false;
        public void PresentHurtAnimation()
        {
            if (!this.isHurting)
            {
                this.isHurting = true;
                HurtPresent().Forget();
            }
        }

        public void WaitGame()
        {
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
            
            CountDown(time, callBack);
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

            this.isHurting = false;
        }

        public void Clear()
        {
            this.ctsCountDownChangeHero.Cancel();
        }

        public void SetKillText(int number)
        {
            this.currentKilledEnemyTxt.text = "kill" + number;
        }

        public void SetEnemyText(int number)
        {
            this.currentEnemyTxt.text = "enemy " + number;
        }

        public void SetCountDown(int number)
        {
            this.currentTimeCountTxt.text = number + "s";
        }

        public void CreateFloatingText(string value, Color32 color, Vector3 position)
        {
           var floatingText = LeanPool.Spawn(this.floatTextPrefab, position, Quaternion.identity);
            floatingText.Setup(value, color);
        }
    }
}
