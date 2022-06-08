using System;

namespace Game.Runtime
{
    
    public enum State
    {
        NONE = -1,
        IDLE = 0,
        MOVE = 1,
        HURT = 2,
        ATTACK = 3,
        DIE = 4
    }
    
    public class UnitState
    {
        public bool IsLockState { get; set; }
        public State Current { get; private set; }
        public State PreviousState { get; private set; }
        
        public UnitState()
        {
            Current = State.NONE;
        }
        
        public Action<State> StartSetState { get; set; }
        
        public void Set(State state)
        {
            if (IsLockState)
            {
                return;
            }

            if (Current != state)
            {
                PreviousState = Current;
                this.Current = state;
            }
            
            StartSetState?.Invoke(state);
        }
        
        public bool CanUseSkill()
        {
            if (Current == State.DIE || Current == State.HURT || Current == State.ATTACK)
            {
                return false;
            }

            return true;
        }
        
        public bool CanMove()
        {
            if (Current == State.DIE)
            {
                return false;
            }

            return true;
        }
    }
}