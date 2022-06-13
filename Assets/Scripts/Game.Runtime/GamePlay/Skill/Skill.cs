using System;

namespace Game.Runtime
{
    public class Skill : IUpdateSystem
    {
        private float _maxCoolDown;
        private float _currentCoolDown;

        public Action<float> OnUpdateCoolDown;

        public void InitData(float maxCoolDown)
        {
            this._maxCoolDown = maxCoolDown;
            this._currentCoolDown = maxCoolDown;
        }

        public void StartCoolDown()
        {
            this._currentCoolDown = 0;
        }

        public bool CanUse => this._currentCoolDown >= this._maxCoolDown;

        public float GetPercentCoolDown()
        {
            return this._maxCoolDown == 0 ? 0 : 1 - this._currentCoolDown / this._maxCoolDown;
        }


        public void OnUpdate(float deltaTime)
        {
            if (this._currentCoolDown < this._maxCoolDown)
            {
                this._currentCoolDown += deltaTime;
                
                
                this.OnUpdateCoolDown?.Invoke(GetPercentCoolDown());
            }
        }
        
    }
}