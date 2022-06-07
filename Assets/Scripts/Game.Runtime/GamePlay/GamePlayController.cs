using System;
using System.Collections.Generic;
using System.Linq;
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
    
    public class GamePlayController : MonoBehaviour, IUpdateSystem
    {
        [SerializeField] private GamePlayData data;
        
        [SerializeField] private Transform[] spawnCharacterList;
        [SerializeField] private Transform[] spawnEnemyList;

        private CancellationTokenSource ctsCountSurvivalTime = new CancellationTokenSource();

        private GameState _state;

        private int selectedHeroIdx;

        private int countTimeSurvival;

        private int enemyLevel;
        
        
        private List<HeroBase> listHeroes = new List<HeroBase>();
        public GameState State { get => this._state; }
        
        
        public static GamePlayController Instance { get; set; }

        private void OnEnable()
        {
            if (GlobalUpdateSystem.Instance != null)
            {
                GlobalUpdateSystem.Instance.Add(this);
            }
        }

        private void OnDisable()
        {
            
            if (GlobalUpdateSystem.Instance != null)
            {
                GlobalUpdateSystem.Instance.Remove(this);
            }
        }

        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            WaitingGame();
        }

        void WaitingGame()
        {
            this.enemyLevel = 0;
            Time.timeScale = 1;
            InitCharacter();
            this._state = GameState.WAITING;
            UIManager.Instance.WaitGame();
        }

        public void StartGame()
        {
            this.countTimeSurvival = 0;
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
            Time.timeScale = 0;
            this._state = GameState.END;
            SaveData();
            UIManager.Instance.EndGame();
           
        }

        void SaveData()
        {
            PlayerPrefs.SetInt(Constants.DATA_CURRENT_SURVIVAL_TIME_KEY, this.countTimeSurvival);
            var highestScore = PlayerPrefs.GetInt(Constants.DATA_HIGHEST_SURVIVAL_TIME_KEY);
            if (this.countTimeSurvival > highestScore)
            {
                PlayerPrefs.SetInt(Constants.DATA_HIGHEST_SURVIVAL_TIME_KEY, this.countTimeSurvival);
            }
        }
        
        public void QuitGame()
        {
            Application.Quit();
        }

        void CallBackStartGame()
        {
            this._state = GameState.RUNNING;
            ctsCountSurvivalTime.Cancel();
            ctsCountSurvivalTime = new CancellationTokenSource();
            StartCountTimeSurvival().Forget();
            Time.timeScale = 1;
        }

        async UniTask StartCountTimeSurvival()
        {
            while (this._state == GameState.RUNNING)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: this.ctsCountSurvivalTime.Token);
                this.countTimeSurvival += 1;
                UIManager.Instance.SetCountDown(this.countTimeSurvival);
            }
        }
        
        void InitCharacter()
        {
            SpawnHeroes();
        }

        void SpawnHeroes()
        {
            this.listHeroes.Clear();
            
            for (int i = 0; i < this.data.heroBases.Length; i++)
            {
                var go = LeanPool.Spawn(this.data.heroBases[i], this.spawnCharacterList[i].position, Quaternion.identity);
                go.SetInfo();
                this.listHeroes.Add(go);
            }
            
            UIManager.Instance.InitCharacters(this.listHeroes.ToArray());
            SelectHero(0);
           
        }
        
        private float countDownSpawnEnemy = 0;
        public void OnUpdate(float deltaTime)
        {
            
            if (State == GameState.END || State == GameState.WAITING)
            {
                this.countDownSpawnEnemy = 0;
            }
            else if (State == GameState.RUNNING)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    SpawnEnemy(0);
                }
                
                // if (this.countDownSpawnEnemy >= this.data.timeCountDownSpawnEnemy)
                // {
                //    SpawnEnemy();
                //    this.countDownSpawnEnemy = 0;
                // }
                // else
                // {
                //     this.countDownSpawnEnemy += deltaTime;
                // }
            }
        }

        void SpawnEnemy()
        {
            var tranformIdx = Random.Range(0, this.spawnEnemyList.Length);

            var randomTransform = this.spawnEnemyList[tranformIdx];

            if (this.data.enemyBases.Length > 0)
            {
                var enemyIdx = Random.Range(0, data.enemyBases.Length);
                var enemy = LeanPool.Spawn(this.data.enemyBases[enemyIdx], randomTransform.position, Quaternion.identity);
                enemy.GetComponent<EnemyBase>().SetInfo(0);
            }
        }
        
        void SpawnEnemy(int index)
        {
            var randomTransform = this.spawnEnemyList[0];

            if (this.data.enemyBases.Length > 0)
            {
                var enemy = LeanPool.Spawn(this.data.enemyBases[index], randomTransform.position, Quaternion.identity);
                enemy.GetComponent<EnemyBase>().SetInfo(this.enemyLevel);
                this.enemyLevel += 1;
            }
        }

        public HeroBase GetSelectedHero()
        {
            return this.listHeroes[this.selectedHeroIdx];
        }

        public List<HeroBase> GetAllHero()
        {
            return this.listHeroes;
        }

        public void SelectHero()
        {
            if (this.selectedHeroIdx < this.listHeroes.Count - 1)
            {
                SelectHero(this.selectedHeroIdx + 1);
            }
            else
            {
                SelectHero(0);
            }
        }

        public void SelectHero(int idx)
        {
            var oldSelectedHero = this.listHeroes[this.selectedHeroIdx];
            var oldPosition = oldSelectedHero.transform.position;
            var faceRight = oldSelectedHero.GetFaceRight();
            this.selectedHeroIdx = idx;
            UIManager.Instance.SetSelectedHero(selectedHeroIdx, this.listHeroes[this.selectedHeroIdx].Data.coolDownChangeHero);
            
            for (int i = 0; i < this.listHeroes.Count; i++)
            {
                if (idx == i)
                {
                    this.listHeroes[i].gameObject.SetActive(true);

                    this.listHeroes[i].transform.position = oldPosition;
                    if (faceRight != this.listHeroes[i].GetFaceRight())
                    {
                        this.listHeroes[i].Flip();
                    }
                    
                }
                else
                {
                    this.listHeroes[i].gameObject.SetActive(false);
                }
            }
        }
    }
}