namespace Game.Runtime
{
    public class Skill : IUpdateSystem
    {
        public float maxCoolDown;
        public float currentCoolDown;

        public void InitData(float maxCoolDown)
        {
            this.maxCoolDown = maxCoolDown;
        }

        public void DecreaseCoolDown(float value)
        {
            this.currentCoolDown -= value;
        }

        public void StartCoolDown()
        {
            this.currentCoolDown = 0;
        }

        public bool CanUse => this.currentCoolDown >= this.maxCoolDown;


        public void OnUpdate(float deltaTime)
        {
            if (this.currentCoolDown < this.maxCoolDown)
            {
                this.currentCoolDown += deltaTime;
            }
        }
    }
}