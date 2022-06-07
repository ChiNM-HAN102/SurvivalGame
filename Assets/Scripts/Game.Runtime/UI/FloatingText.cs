using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

namespace Game.Runtime
{
    public class FloatingText : MonoBehaviour, IUpdateSystem
    {
        private float disappearTimer;
        private Color textColor;
        private Vector3 moveVector;
        [SerializeField]private TextMesh _textMesh;
        
        private const float DISAPPEAR_TIMER_MAX = 0.75f;
        
        protected virtual void OnEnable()
        {
            if (GlobalUpdateSystem.Instance != null)
            {
                GlobalUpdateSystem.Instance.Add(this);
            }
        }

        protected void OnDisable()
        {
            
            if (GlobalUpdateSystem.Instance != null)
            {
                GlobalUpdateSystem.Instance.Remove(this);
            }
        }
        
        public void Setup(string value, Color32 color32 ) {
            this._textMesh.text = value.ToString();
            this._textMesh.fontSize = 9;
            textColor = color32;
            this._textMesh.color = textColor;
            disappearTimer = DISAPPEAR_TIMER_MAX;
            moveVector = new Vector3(.7f, 1) * 15f;
            
            transform.localScale = new Vector3(1,1,1);
        }

        public void OnUpdate(float deltaTime)
        {
            transform.position += moveVector * deltaTime;
            moveVector -= moveVector * 3f * deltaTime;

            if (disappearTimer > DISAPPEAR_TIMER_MAX * .5f) {
                // First half of the popup lifetime
                float increaseScaleAmount = 1f;
                transform.localScale += Vector3.one * increaseScaleAmount * deltaTime;
            } else {
                // Second half of the popup lifetime
                float decreaseScaleAmount = 1f;
                transform.localScale -= Vector3.one * decreaseScaleAmount * deltaTime;
            }

            disappearTimer -= deltaTime;
            if (disappearTimer < 0) {
                // Start disappearing
                float disappearSpeed = 1.5f;
                textColor.a -= disappearSpeed * Time.deltaTime;
                this._textMesh.color = textColor;
                if (textColor.a < 0) {
                    LeanPool.Despawn(gameObject);
                        
                }
            }
        }
    }
}
