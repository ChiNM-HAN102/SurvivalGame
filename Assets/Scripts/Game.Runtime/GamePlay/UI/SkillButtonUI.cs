using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Runtime
{
    public class SkillButtonUI : MonoBehaviour
    {
        [SerializeField] private SkillType _skillType;
        [SerializeField] private Image countDownUI;
        [SerializeField] private Button clickButton;
        [SerializeField] private InputControlType controlType;


        private Skill _skill;
        private HeroBase _currentHero;
        
        public void InitData()
        {
            _currentHero = GamePlayController.Instance.GetSelectedHero();
            
            this._skill = this._currentHero.Skills.GetSkill(this._skillType);

            if (this._skill != null)
            {
                _skill.OnUpdateCoolDown = UpdateCoolDown;
            }
            
            this.clickButton.onClick.RemoveAllListeners();
            this.clickButton.onClick.AddListener(OnClick);
            
            UpdateCoolDown(this._skill.GetPercentCoolDown());
        }

        private void OnClick()
        {
            _currentHero.SetCurrentControlType(this.controlType);
        }

        private void UpdateCoolDown(float value)
        {
            this.countDownUI.fillAmount = value < 0 ? 0 : value;
        }
    }
}
