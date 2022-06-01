using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Lean.Pool;
using UnityEngine;

namespace Game.Runtime
{
    public enum UnitState
    {
        NONE = -1,
        IDLE = 0,
        MOVE = 1,
        HURT = 2,
        ATTACK = 3,
        DIE = 4
    }

    public class Character1 : HeroBase
    {
        [SerializeField] private float speed;

        [SerializeField] private Transform spawnBulletPosition;

        [SerializeField] private Transform spawnBullet2Position;
        [SerializeField] private Transform spawnBullet3Position;

        [SerializeField] private GameObject prefabBullet;

        [SerializeField] private GameObject prefabBullet2;

        [SerializeField] private GameObject prefabBullet3;

        private Animator _animator;

        private UnitState _state;

      

        protected override void Awake()
        {
            base.Awake();
            _animator = GetComponentInChildren<Animator>();
            this._state = UnitState.IDLE;
        }

        public override void OnUpdate(float deltaTime)
        {
            var inputX = Input.GetAxisRaw("Horizontal");
            var moveVector = new Vector2(inputX, 0);

            if (this._state != UnitState.ATTACK)
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

            if (this._state != UnitState.ATTACK)
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

                    if (this._state != UnitState.MOVE)
                    {
                        this._state = UnitState.MOVE;
                        this._animator.Play("Run");
                    }

                    transform.position = transform.position + (Vector3)moveVector * (this.speed * Time.deltaTime);
                }
                else
                {
                    if (this._state != UnitState.IDLE)
                    {
                        this._state = UnitState.IDLE;
                        this._animator.Play("Idle");
                    }
                }
            }
        }


        void ExecuteAttack1()
        {
            this._animator.Play("Attack_1");
            SpawnBullet().Forget();
        }

        void ExecuteAttack2()
        {
            this._animator.Play("Attack_2");
            SpawnBullet2().Forget();
        }

        void ExecuteAttack3()
        {
            this._animator.Play("Attack_3");
            SpawnBullet3().Forget();
        }

        async UniTaskVoid SpawnBullet()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            var bullet = LeanPool.Spawn(this.prefabBullet, this.spawnBulletPosition.position, Quaternion.identity);
            var bulletBase = bullet.GetComponent<BulletBase>();
            bulletBase.InitBullet(2, this.faceRight ,20);
        }
        
        async UniTaskVoid SpawnBullet2()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            var bullet = LeanPool.Spawn(this.prefabBullet2, this.spawnBullet2Position.position, Quaternion.identity);
            var bulletBase = bullet.GetComponent<BulletBase>();
            bulletBase.InitBullet(2, this.faceRight ,20);
        }
        
        async UniTaskVoid SpawnBullet3()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            var bullet = LeanPool.Spawn(this.prefabBullet3, this.spawnBullet3Position.position, Quaternion.identity);
            var bulletBase = bullet.GetComponent<BulletBase>();
            bulletBase.InitBullet(2, this.faceRight ,20);
        }
        
        void SetAttack()
        {
            this._state = UnitState.ATTACK;
            StopAttack().Forget();
        }

        async UniTaskVoid StopAttack()
        {
            this._state = UnitState.ATTACK;
            await UniTask.Yield();
            var clips = this._animator.GetCurrentAnimatorClipInfo(0);
            if (clips.Length > 0)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(clips[0].clip.length));
            }
            
            if (this._state == UnitState.ATTACK)
            {
                this._state = UnitState.NONE;
            }
        }

       
    }
}