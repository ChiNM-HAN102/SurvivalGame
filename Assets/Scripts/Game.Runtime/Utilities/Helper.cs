using System;
using Lean.Pool;
using UnityEngine;

namespace Game.Runtime
{
    public class Helper : MonoBehaviour
    {
        [SerializeField] private FloatingText floatTextPrefab;

        public static Helper Instance { get; private set; }
        
        private void Awake()
        {
            Instance = this;
        }

        public void CreateFloatingText(string value, Color32 color, Vector3 position)
        {
            var floatingText = LeanPool.Spawn(this.floatTextPrefab, position, Quaternion.identity);
            floatingText.Setup(value, color);
        }

        public void DisplayHealthPlus(int dataValue, Vector3 position)
        {
            CreateFloatingText("+" + dataValue, new Color32(0, 219, 4, 255),  
                new Vector2(position.x, position.y + 1.5f));
        }

        public void DisplayInventoryPlus(int dataValue, Vector3 position)
        {
            CreateFloatingText("+" + dataValue, new Color32(101, 193, 219, 255),  
                new Vector2(position.x, position.y + 1.5f));
        }

        public void DisplayDamage(float dataValue, Vector3 position)
        {
            CreateFloatingText("-" + dataValue, new Color32(219, 64, 53, 255),
                new Vector2(position.x, position.y + 1.5f));
        }
    }
}