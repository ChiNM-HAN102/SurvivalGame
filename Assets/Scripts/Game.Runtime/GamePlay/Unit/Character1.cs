using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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

    public class Character1 : Dummy
    {
        [SerializeField] private float speed;

        private Animator _animator;

        private UnitState _state;

        private bool faceRight;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            this._state = UnitState.IDLE;
        }

        // Start is called before the first frame update
        void Start()
        {
            this.faceRight = true;
        }

        // Update is called once per frame
        void Update()
        {
        }

        public override void OnUpdate(float deltaTime)
        {
            var inputX = Input.GetAxisRaw("Horizontal");
            var moveVector = new Vector2(inputX, 0);

            if (this._state != UnitState.ATTACK)
            {
                if (Input.GetKeyDown(KeyCode.T))
                {
                    this._animator.Play("Attack_1");
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

        void Flip()
        {
            var newScale = transform.localScale;
            newScale.x = -newScale.x;
            transform.localScale = newScale;

            this.faceRight = !this.faceRight;
        }
    }
}