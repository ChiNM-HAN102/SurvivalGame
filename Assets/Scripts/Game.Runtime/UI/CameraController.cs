using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class CameraController : MonoBehaviour , IUpdateSystem
    {
        public float followSpeed = 2f;
        public float yOffset = 1f;
        private Transform target;
        
        // How long the object should shake for.
        private float shakeDuration = 0f;
        
        // Amplitude of the shake. A larger value shakes the camera harder.
        private float shakeAmount = 2f;
        private float decreaseFactor = 3.0f;

        private Vector3 originPosition;
        
        public static CameraController Instance { get; set; }

        protected void Awake()
        {
            Instance = this;
        }
        
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

        public void SetShakeDuration(float value)
        {
            this.shakeDuration = value;
        }

        public void OnUpdate(float deltaTime)
        {
            if (this.shakeDuration > 0)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition,
                    originPosition + Random.insideUnitSphere * shakeAmount, Time.deltaTime * 10);
                shakeDuration -= Time.deltaTime * decreaseFactor;
            }
            else
            {
                var heroBase = GamePlayController.Instance.GetSelectedHero();
                if (heroBase != null)
                {
                    this.target = heroBase.transform; 
                }
                
                if (this.target != null)
                {
                    Vector3 newPos = new Vector3(this.target.position.x, this.target.position.y + this.yOffset, - 10f);
                    transform.position = Vector3.Slerp(transform.position, newPos, this.followSpeed * Time.deltaTime);
                    this.originPosition = transform.position;
                }
            }
        }
    }
}
