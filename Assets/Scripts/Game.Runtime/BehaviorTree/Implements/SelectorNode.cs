namespace Game.Runtime
{
    public class SelectorNode : CompositeNode
    {
        protected override void OnStart()
        {
            
        }

        protected override void OnStop()
        {
           
        }

        protected override NodeState OnUpdate(float deltaTime)
        {
            return NodeState.Failure;
        }
    }
}