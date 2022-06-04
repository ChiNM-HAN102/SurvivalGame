using System;
using System.Collections.Generic;
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

        private GameState _state;

        private int selectedHeroIdx;
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
            InitCharacter();
            WaitingGame();
        }

        void WaitingGame()
        {
            this._state = GameState.WAITING;
            UIManager.Instance.WaitGame();
        }

        public void StartGame()
        {
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
            
        }

        public void ResumeGame()
        {
            UIManager.Instance.StartGame(2, () => {
                CallBackStartGame();
                Time.timeScale = 1;
            });
        }

        public void EndGame()
        {
            
        }
        
        public void QuitGame()
        {
            Application.Quit();
        }

        void CallBackStartGame()
        {
            this._state = GameState.RUNNING;
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
                enemy.GetComponent<EnemyBase>().SetInfo(0);
            }
        }

        public HeroBase GetSelectedHero()
        {
            return this.listHeroes[this.selectedHeroIdx];
        }

        public void SelectHero(int idx)
        {
            this.selectedHeroIdx = 0;
            UIManager.Instance.SetSelectedHero(selectedHeroIdx);
            
            for (int i = 0; i < this.listHeroes.Count; i++)
            {
                if (idx == i)
                {
                    this.listHeroes[i].gameObject.SetActive(true);
                }
                else
                {
                    this.listHeroes[i].gameObject.SetActive(false);
                }
            }
        }
    }
}