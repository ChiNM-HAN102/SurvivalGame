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

        private void OnTriggerStay2D(Collider2D other)
        {
            GamePlayStatusController.Instance.AddInventory(this.id, this.data);
            LeanPool.Despawn(gameObject);
        }
    }
}