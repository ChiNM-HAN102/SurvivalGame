using System;
using Lean.Pool;
using UnityEngine;

namespace Game.Runtime
{
    public class InventoryBase : MonoBehaviour
    {
        [SerializeField] protected InventoryData data;

        public string id;

        private void OnEnable()
        {
            id = Guid.NewGuid().ToString();
        }

        private void OnTriggerEnter(Collider other)
        {
            var hero = other.GetComponent<HeroBase>();
            if (hero.IsAlive)
            {
                
            }
            
            LeanPool.Despawn(gameObject);
        }
    }
}