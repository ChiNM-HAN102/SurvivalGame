using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Lean.Pool;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Runtime
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Text countDownTxt;
        [SerializeField] private Text plusTxt;

        private CancellationTokenSource source;
        private InventoryData data;

        private Action callback;
        
        public void InitData(InventoryData data, Action callBack)
        {
            this.data = data;
            this.callback = callBack;
            this.icon.sprite = data.icon;
            this.plusTxt.text = $"+{data.value}";

            if (!data.permanent)
            {
                this.source = new CancellationTokenSource();
                CountDown().Forget();
            }
        }

        async UniTaskVoid CountDown()
        {
            var current = this.data.lastTime;
            while (current > 0)
            {
                this.countDownTxt.text = current.ToString();
                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: this.source.Token);
                current--;
            }
            
            this.callback?.Invoke();
            LeanPool.Despawn(gameObject);
        }

        private void OnDisable()
        {
            this.source.Cancel();
        }
    }
}