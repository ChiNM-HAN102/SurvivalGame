using System;
using Lean.Pool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Runtime
{
    public class SpawnController : Dummy
    {
        [SerializeField] private Transform[] spawnEnemyList;
        [SerializeField] private SpawnData spawnData;
        [SerializeField] private bool isTest;
        private float _countDownSpawnEnemy = 0;

        public override void OnUpdate(float deltaTime)
        {
            if (GamePlayController.Instance.State == GameState.END || GamePlayController.Instance.State == GameState.WAITING)
            {
                this._countDownSpawnEnemy = 0;
            }
            else if (GamePlayController.Instance.State == GameState.RUNNING)
            {
                if (!this.isTest)
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
            var transformIdx = Random.Range(0, this.spawnEnemyList.Length);

            var randomTransform = this.spawnEnemyList[transformIdx];

            if (this.spawnData.enemyBases.Length > 0)
            {
                var enemyIdx = Random.Range(0, spawnData.enemyBases.Length);
                var enemy = LeanPool.Spawn(this.spawnData.enemyBases[enemyIdx], randomTransform.position, Quaternion.identity);
                enemy.GetComponent<EnemyBase>().SetInfo(GamePlayController.Instance.EnemyLevel);
                GamePlayController.Instance.EnemyLevel += 1;
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