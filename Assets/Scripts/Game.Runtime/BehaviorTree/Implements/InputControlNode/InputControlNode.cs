namespace Game.Runtime
{
    public enum InputControlType
    {
        NONE = -1,
        MOVE_LEFT = 0,
        MOVE_RIGHT = 1,
        ATTACK = 2,
        USE_SKILL_1 = 30,
        USE_SKILL_2 = 31,
        USE_SKILL_3 = 32
    }
    
    public abstract class InputControlNode : ActionNode
    {
        public InputControlType controlType;
        
        
    }
}