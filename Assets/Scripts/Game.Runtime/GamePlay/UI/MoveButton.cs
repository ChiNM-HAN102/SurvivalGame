using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Runtime
{
    public class MoveButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {

        [SerializeField] private InputControlType inputType;

        public void OnPointerDown(PointerEventData eventData)
        {
            GamePlayController.Instance.GetSelectedHero().SetCurrentControlType(this.inputType, true);
            transform.localScale = new Vector2(0.8f, 0.8f);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            GamePlayController.Instance.GetSelectedHero().SetCurrentControlType(InputControlType.NONE);
            transform.localScale = new Vector2(1, 1);
        }
    }
}
