using System;
using UnityEngine;

namespace Game.Runtime
{
    public class SkillTriggerEvent : MonoBehaviour
    {
        public Action StartAction;
        public Action EndAction;

        public void Do()
        {
            this.StartAction?.Invoke();
        }

        public void Done()
        {
            this.EndAction?.Invoke();
        }
    }
}