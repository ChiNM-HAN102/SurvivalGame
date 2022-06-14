using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Runtime
{
    public class UnitAnimController : Dummy
    {
        [SerializeField] private Unit _owner;
        [SerializeField] private Animator _animator;
        [SerializeField] private string _animDie = "Death";
        [SerializeField] private string _animHurt = "Hurt";
        [SerializeField] private string _animIdle = "Idle";
        [SerializeField] private string _animMove = "Walk";
        
        
        private Action _callBackAfterAnim;

        private bool _haveAnimTime;
        private float _countDownAnim;
        private string _currentAnimStateName;

        public void DoAnim(string animName, State state, Action callBackAfterDone = null)
        {
            if (this._owner.UnitState.Current != state)
            {
                this._owner.UnitState.Set(state);
                this._animator.Play(animName);

                this._currentAnimStateName = animName;
                _callBackAfterAnim = null;

                if (callBackAfterDone != null)
                {
                    _callBackAfterAnim = callBackAfterDone;
                    this._haveAnimTime = false;
                }
            }
        }

        public void Idle(Action callBackAfterDone = null)
        {
            DoAnim(this._animIdle, State.IDLE, callBackAfterDone);
        }

        public void Hurt(Action callBackAfterDone = null)
        {
            DoAnim(this._animHurt, State.HURT, callBackAfterDone);
        }
        
        public void Die(Action callBackAfterDone = null)
        {
            DoAnim(this._animDie, State.DIE, callBackAfterDone);
        }
        
        public void Move(Action callBackAfterDone = null)
        {
            DoAnim(this._animMove, State.MOVE, callBackAfterDone);
        }

        public async UniTask WaitUntilFinishAnim(CancellationToken? token = null)
        {
            
            await UniTask.Yield();
            var clips = _animator.GetCurrentAnimatorClipInfo(0);
            
            if (clips.Length > 0)
            {
                if (token != null)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(clips[0].clip.length), cancellationToken: (CancellationToken)token);
                }
                else
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(clips[0].clip.length));
                }
            }
        }

        public override void OnUpdate(float deltaTime)
        {

            if (this._callBackAfterAnim != null)
            {
                if (!this._haveAnimTime)
                {
                    var animState = this._animator.GetCurrentAnimatorStateInfo(0);
                    if (animState.IsName(this._currentAnimStateName))
                    {
                        this._haveAnimTime = true;
                        this._countDownAnim = this._animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
                    }
                }

                if (this._haveAnimTime)
                {
                    this._countDownAnim -= deltaTime;
                    if (this._countDownAnim <= 0)
                    {
                        this._callBackAfterAnim?.Invoke();
                        this._callBackAfterAnim = null;
                    }
                }
            }
        }
    }
}