using UnityEngine;
using UnityEngine.UI;

namespace Game.Runtime
{
    public class InfoUIController : MonoBehaviour
    {
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image avatar;
        
        [SerializeField] private GameObject healthBarFrame;
        [SerializeField] private Transform healthBar;

        [SerializeField] private Text attackText;
        [SerializeField] private Text bonusAttackText;

        [SerializeField] private Transform itemContainer;

        private int characterId;
    }
}