using System;
using Lean.Pool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Runtime
{
    public class SpawnController : Dummy
    {
        [SerializeField] private Transform[] spawnCharacterList;
        [SerializeField] private Transform[] spawnEnemyList;
        [SerializeField] private SpawnData spawnData;
        
        private float _countDownSpawnEnemy = 0;

        private void Awake()
        {
            
        }

        public override void OnUpdate(float deltaTime)
        {
            if (GamePlayController.Instance.State == GameState.END || GamePlayController.Instance.State == GameState.WAITING)
            {
                this._countDownSpawnEnemy = 0;
            }
            else if (GamePlayController.Instance.State == GameState.RUNNING)
            {
                if (!GamePlayController.Instance.IsTest)
                {
                    if (this._countDownSpawnEnemy >= this.spawnData.timeCountDownSpawnEnemy)
                    {
                        SpawnEnemy();
                        this._countDownSpawnEnemy = 0;
                    }
                    else
                    {
                        this._countDownSpawnEnemy += deltaTime;
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.I))
                    {
                        SpawnEnemy(0);
                    }
                    else if (Input.GetKeyDown(KeyCode.O))
                    {
                        SpawnEnemy(1);
                    }
                    else if (Input.GetKeyDown(KeyCode.P))
                    {
                        SpawnEnemy(2);
                    }
                }
            }
        }
        
        void SpawnEnemy()
        {
            var tranformIdx = Random.Range(0, this.spawnEnemyList.Length);

            var randomTransform = this.spawnEnemyList[tranformIdx];

            if (this.spawnData.enemyBases.Length > 0)
            {
                var enemyIdx = Random.Range(0, spawnData.enemyBases.Length);
                var enemy = LeanPool.Spawn(this.spawnData.enemyBases[enemyIdx], randomTransform.position, Quaternion.identity);
                enemy.GetComponent<EnemyBase>().SetInfo(GamePlayController.Instance.EnemyLevel);
            }
        }
        
        void SpawnEnemy(int index)
        {
            var randomTransform = this.spawnEnemyList[0];

            if (this.spawnData.enemyBases.Length > 0)
            {
                var enemy = LeanPool.Spawn(this.spawnData.enemyBases[index], randomTransform.position, Quaternion.identity);
                enemy.GetComponent<EnemyBase>().SetInfo(GamePlayController.Instance.EnemyLevel);
                GamePlayController.Instance.EnemyLevel += 1;
            }
        }
    }
}