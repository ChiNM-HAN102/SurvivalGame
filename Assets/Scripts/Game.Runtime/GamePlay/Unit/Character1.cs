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
            //

            if (this._state == UnitState.IDLE)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    SetAttack();
                    this._animator.SetTrigger("Attack_1");
                }
                else if (Input.GetKeyDown(KeyCode.R))
                {
                    SetAttack();
                    this._animator.SetTrigger("Attack_2");
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    SetAttack();
                    this._animator.SetTrigger("Attack_3");
                }
            }
            
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
                    this._animator.SetTrigger("Run");
                }

                transform.position = transform.position + (Vector3)moveVector * (this.speed * Time.deltaTime);
            }
            else
            {
                if (this._state == UnitState.MOVE || this._state == UnitState.NONE)
                {
                    this._state = UnitState.IDLE;
                    this._animator.SetTrigger("Idle");
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
            await UniTask.Delay(TimeSpan.FromSeconds(1.5f));
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