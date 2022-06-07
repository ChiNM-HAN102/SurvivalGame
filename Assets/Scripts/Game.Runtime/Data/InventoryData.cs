using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "InventoryData", menuName = "Data/InventoryData")]
    public class InventoryData : ScriptableObject
    {
        public RPGStatType type;
        public int value;
        public float lastTime;
        public bool permanent;
        private Sprite icon;
    }
}