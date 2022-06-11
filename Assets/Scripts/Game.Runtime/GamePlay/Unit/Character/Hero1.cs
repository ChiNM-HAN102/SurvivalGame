using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Lean.Pool;
using UnityEngine;

namespace Game.Runtime
{
 

    public class Hero1 : HeroBase
    {
        [SerializeField] private Transform spawnBulletPosition;
        [SerializeField] private Transform spawnBullet2Position;
        [SerializeField] private Transform spawnBullet3Position;

        [SerializeField] private GameObject prefabBullet;
        [SerializeField] private GameObject prefabBullet2;
        [SerializeField] private GameObject prefabBullet3;
        

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            
            var inputX = Input.GetAxisRaw("Horizontal");
            var moveVector = new Vector2(inputX, 0);

            if (UnitState.Current != State.ATTACK)
            {
                if (Input.GetKeyDown(KeyCode.T))
                {
                    ExecuteAttack1();
                    SetAttack();
                }

                if (Input.GetKeyDown(KeyCode.R))
                {
                   
                    ExecuteAttack2();
                    SetAttack();
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                  
                    ExecuteAttack3();
                    SetAttack();
                }
            }

            if (UnitState.Current != State.ATTACK)
            {
                if (inputX != 0)
                {
                    if (inputX < 0)
                    {
                        if (this.faceRight)
                        {
                            Flip();
                        }
                    }
                    else if (inputX > 0)
                    {
                        if (!this.faceRight)
                        {
                            Flip();
                        }
                    }

                    this.AnimController.DoAnim(this._animMove, State.MOVE);

                    transform.position = transform.position + (Vector3)moveVector * (this.Stats.GetStat<MoveSpeed>(RPGStatType.MoveSpeed).StatValue * Time.deltaTime);
                }
                else
                {
                    this.AnimController.DoAnim(this._animIdle, State.IDLE);
                }
            }
        }


        void ExecuteAttack1()
        {
            this.AnimController.DoAnim(this._animSkill1, State.USE_SKILL_1);
            SpawnBullet().Forget();
        }

        void ExecuteAttack2()
        {
            this.AnimController.DoAnim(this._animSkill2, State.USE_SKILL_2);
            SpawnBullet2().Forget();
        }

        void ExecuteAttack3()
        {
            this.AnimController.DoAnim(this._animSkill3, State.USE_SKILL_3);
            SpawnBullet3().Forget();
        }

        async UniTaskVoid SpawnBullet()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            var bullet = LeanPool.Spawn(this.prefabBullet, this.spawnBulletPosition.position, Quaternion.identity);
            var bulletBase = bullet.GetComponent<BulletBase>();
            bulletBase.InitBullet(2, this.faceRight ,20, Stats.GetStat<Damage>(RPGStatType.Damage).StatValue);
        }
        
        async UniTaskVoid SpawnBullet2()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            var bullet = LeanPool.Spawn(this.prefabBullet2, this.spawnBullet2Position.position, Quaternion.identity);
            var bulletBase = bullet.GetComponent<BulletBase>();
            bulletBase.InitBullet(2, this.faceRight ,20, Stats.GetStat<Damage>(RPGStatType.Damage).StatValue);
        }
        
        async UniTaskVoid SpawnBullet3()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            var bullet = LeanPool.Spawn(this.prefabBullet3, this.spawnBullet3Position.position, Quaternion.identity);
            var bulletBase = bullet.GetComponent<BulletBase>();
            bulletBase.InitBullet(2, this.faceRight ,20, Stats.GetStat<Damage>(RPGStatType.Damage).StatValue);
        }
        
        void SetAttack()
        {
            UnitState.Set(State.ATTACK);
            StopAttack().Forget();
        }

        async UniTaskVoid StopAttack()
        {
            await AnimController.WaitUntilFinishAnim();
            
            if (UnitState.Current == State.ATTACK)
            {
                UnitState.Set(State.NONE);
            }
        }

       
    }
}