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

                    if (UnitState.Current != State.MOVE)
                    {
                        UnitState.Set(State.MOVE);
                        this._animator.Play(this._animMove);
                    }

                    transform.position = transform.position + (Vector3)moveVector * (this.Stats.GetStat<MoveSpeed>(RPGStatType.MoveSpeed).StatValue * Time.deltaTime);
                }
                else
                {
                    if (UnitState.Current != State.IDLE)
                    {
                        UnitState.Set(State.IDLE);
                        this._animator.Play(this._animIdle);
                    }
                }
            }
        }


        void ExecuteAttack1()
        {
            this._animator.Play(this._animSkill1);
            SpawnBullet().Forget();
        }

        void ExecuteAttack2()
        {
            this._animator.Play(this._animSkill2);
            SpawnBullet2().Forget();
        }

        void ExecuteAttack3()
        {
            this._animator.Play(this._animSkill3);
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
            await UniTask.Yield();
            var clips = this._animator.GetCurrentAnimatorClipInfo(0);
            if (clips.Length > 0)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(clips[0].clip.length));
            }
            
            if (UnitState.Current == State.ATTACK)
            {
                UnitState.Set(State.NONE);
            }
        }

       
    }
}