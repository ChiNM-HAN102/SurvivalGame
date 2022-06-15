using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading;
using Cysharp.Threading.Tasks;
using Lean.Pool;
using UnityEngine;

using Random = UnityEngine.Random;

namespace Game.Runtime
{
    public enum GameState
    {
        PAUSING = 0,
        END = 1,
        RUNNING = 2,
        WAITING = 3
    }
    
    public class GamePlayController : MonoBehaviour
    {
        [SerializeField] private GamePlayData data;

        private int _enemyLevel;

        public int EnemyLevel
        {
            get => this._enemyLevel;
            set => this._enemyLevel = value;
        }

        private CancellationTokenSource _ctsCountSurvivalTime = new CancellationTokenSource();

        private GameState _state;

        private int _selectedHeroIdx;

        private int _countTimeSurvival;

        private int _totalKillEnemy;
        
        private readonly List<HeroBase> _listHeroes = new List<HeroBase>();
        public GameState State { get => this._state; }

        public static GamePlayController Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            SoundController.Instance.Init();
            WaitingGame();
        }

        void WaitingGame()
        {
            SoundController.Instance.PlayWaitingMusic();
            SoundController.Instance.PlayWaitingSound();
            this._enemyLevel = 0;
            this._totalKillEnemy = 0;
            UIManager.Instance.SetKillText(0);
            UIManager.Instance.SetCountDown(0);
            Time.timeScale = 1;
            SpawnHeroes();
            this._state = GameState.WAITING;
            UIManager.Instance.WaitGame();
        }

        public void StartGame()
        {
            this._countTimeSurvival = 0;
            UIManager.Instance.StartGame(3, CallBackStartGame);
        }

        public void PauseGame()
        {
            Time.timeScale = 0;
            this._state = GameState.PAUSING;
            UIManager.Instance.PauseGame();
        }

        public void ResetGame()
        {
            ClearMap();
            UIManager.Instance.Clear();
            GamePlayStatusController.Instance.ClearInventory();
            WaitingGame();
        }

        void ClearMap()
        {
            var allObjects = FindObjectsOfType<Unit>();
            foreach (var go in allObjects)        
            {
                LeanPool.Despawn(go.gameObject);
            }
        }

        public void ResumeGame()
        {
            UIManager.Instance.StartGame(2, CallBackStartGame);
        }
        
        public void EndGame()
        {
            SoundController.Instance.StopMusic();
            SoundController.Instance.PlayEndSound();
            Time.timeScale = 0;
            this._state = GameState.END;
            SaveData();
            UIManager.Instance.EndGame();
        }

        void SaveData()
        {
            PlayerPrefs.SetInt(Constants.DATA_CURRENT_SURVIVAL_TIME_KEY, this._countTimeSurvival);
            var highestScore = PlayerPrefs.GetInt(Constants.DATA_HIGHEST_SURVIVAL_TIME_KEY);
            if (this._countTimeSurvival > highestScore)
            {
                PlayerPrefs.SetInt(Constants.DATA_HIGHEST_SURVIVAL_TIME_KEY, this._countTimeSurvival);
            }
            
            PlayerPrefs.SetInt(Constants.DATA_CURRENT_KILL_ENEMY, this._totalKillEnemy);
            var highestScoreKill = PlayerPrefs.GetInt(Constants.DATA_CURRENT_KILL_ENEMY);
            if (this._totalKillEnemy > highestScoreKill)
            {
                PlayerPrefs.SetInt(Constants.DATA_CURRENT_KILL_ENEMY, this._totalKillEnemy);
            }
            
            PlayerPrefs.Save();
        }
        
        public void QuitGame()
        {
            Application.Quit();
        }

        void CallBackStartGame()
        {
            this._state = GameState.RUNNING;
            this._ctsCountSurvivalTime.Cancel();
            this._ctsCountSurvivalTime = new CancellationTokenSource();
            StartCountTimeSurvival().Forget();
            Time.timeScale = 1;
            SoundController.Instance.PlayGamePlayMusic();
        }

        async UniTask StartCountTimeSurvival()
        {
            while (this._state == GameState.RUNNING)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: this._ctsCountSurvivalTime.Token);
                this._countTimeSurvival += 1;
                UIManager.Instance.SetCountDown(this._countTimeSurvival);
            }
        }

        void SpawnHeroes()
        {
            if (this._listHeroes.Count == 0)
            {
                for (int i = 0; i < this.data.heroBases.Length; i++)
                {
                    var go = LeanPool.Spawn(this.data.heroBases[i],  this.transform);
                    this._listHeroes.Add(go);
                }
            }

            for (int i = 0; i < this._listHeroes.Count; i++)
            {
                _listHeroes[i].SetInfo();
                _listHeroes[i].transform.position = new Vector2(0, 1);
            }

            UIManager.Instance.InitCharacters(this._listHeroes.ToArray());
            SelectHero(0);
        }
        
        public HeroBase GetSelectedHero()
        {
            return this._listHeroes[this._selectedHeroIdx];
        }

        public List<HeroBase> GetAllHero()
        {
            return this._listHeroes;
        }

        public void SelectHero(int idx)
        {
            var oldSelectedHero = this._listHeroes[this._selectedHeroIdx];
            var oldPosition = oldSelectedHero.transform.position;
            var faceRight = oldSelectedHero.GetFaceRight();
            this._selectedHeroIdx = idx;

            var heroData = (HeroData)this._listHeroes[this._selectedHeroIdx].Data;
            
            UIManager.Instance.SetSelectedHero(this._selectedHeroIdx, heroData.coolDownChangeHero);
            
            for (int i = 0; i < this._listHeroes.Count; i++)
            {
                if (idx == i)
                {
                    this._listHeroes[i].gameObject.SetActive(true);
                    SoundController.Instance.PlayStartSound();
                    LeanPool.Spawn(heroData.transformEffect, oldPosition, Quaternion.identity);
                    this._listHeroes[i].transform.position = oldPosition;
                    if (faceRight != this._listHeroes[i].GetFaceRight())
                    {
                        this._listHeroes[i].Flip();
                    }
                }
                else
                {
                    this._listHeroes[i].gameObject.SetActive(false);
                }
            }
        }

        public void IncreaseTotalKillEnemy()
        {
            this._totalKillEnemy++;
            UIManager.Instance.SetKillText(this._totalKillEnemy);
        }
    }
}