using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Runtime
{
    public class Utilities
    {
        public static async UniTask WaitUntilFinishAnim(Animator _animator, CancellationToken? token = null)
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
    }
}