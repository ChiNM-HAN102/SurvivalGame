using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Runtime
{
    public class UnitAnimController : IUpdateSystem
    {
        protected Unit _owner;

        protected Animator _animator;

        private CancellationTokenSource cts;

        private Action _callBackAfterAnim;

        private bool haveAnimTime;
        private float countDownAnim;
        private string currentAnimStateName;

        public UnitAnimController(Unit owner)
        {
            this._owner = owner;
            this._animator = owner.GetComponentInChildren<Animator>();
            
            GlobalUpdateSystem.Instance.Add(this);
        }

        ~UnitAnimController()
        {
            GlobalUpdateSystem.Instance.Remove(this);
        }

        public virtual void DoAnim(string animName, State state, Action callBackAfterDone = null)
        {
            if (this._owner.UnitState.Current != state)
            {
                this._owner.UnitState.Set(state);
                this._animator.Play(animName);

                currentAnimStateName = animName;
                _callBackAfterAnim = null;

                if (callBackAfterDone != null)
                {
                    _callBackAfterAnim = callBackAfterDone;
                    this.haveAnimTime = false;
                }
            }
           
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

        public void OnUpdate(float deltaTime)
        {

            if (this._callBackAfterAnim != null)
            {
                if (!this.haveAnimTime)
                {
                    if (this.currentAnimStateName == "Attack_3")
                    {
                        Debug.Log("here");
                    }
                    var animState = this._animator.GetCurrentAnimatorStateInfo(0);
                    if (animState.IsName(this.currentAnimStateName))
                    {
                        this.haveAnimTime = true;
                        this.countDownAnim = this._animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
                    }
                }

                if (this.haveAnimTime)
                {
                    this.countDownAnim -= deltaTime;
                    if (this.countDownAnim <= 0)
                    {
                        this._callBackAfterAnim?.Invoke();
                        this._callBackAfterAnim = null;
                    }
                }
            }
        }
    }
}