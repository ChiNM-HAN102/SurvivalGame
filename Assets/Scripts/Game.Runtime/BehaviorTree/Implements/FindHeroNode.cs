using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "FindHeroNode", menuName = "BehaviorTree/Node/Action/FindHeroNode")]
    public class FindHeroNode : ActionNode
    {
        
        private Unit owner;
        
        protected override void OnStart()
        {
            this.owner = this.Tree.Owner;
        }

        protected override void OnStop()
        {
            
        }

        protected override NodeState OnUpdate(float deltaTime)
        {
            var hero = GamePlayController.Instance.GetSelectedHero();
            if (hero == null)
            {
                return NodeState.Running;
            }

            this.owner.target = hero;
            return NodeState.Success;
        }
    }
}