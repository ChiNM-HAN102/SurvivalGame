using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Runtime
{
    public class UnitAndSkill : MonoBehaviour, IUpdateSystem
    {
        [SerializeField] private Unit unit;
        [SerializeField] private Skill skill;
        [SerializeField] private KeyCode _keyCode;
        [SerializeField] private Button button;
        [SerializeField] private string animName;

        private void Awake()
        {
            if (this.button)
            {
                this.button.onClick.AddListener(ExecuteSkill);
            }
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
        
        public void OnUpdate(float deltaTime)
        {
            ExecuteSkill();
        }

        void ExecuteSkill()
        {
            if ( this.unit.IsAlive && this.unit.UnitState.CanUseSkill() && Input.GetKeyDown(this._keyCode))
            {
                this.skill.Execute();
            }
        }
    }
}