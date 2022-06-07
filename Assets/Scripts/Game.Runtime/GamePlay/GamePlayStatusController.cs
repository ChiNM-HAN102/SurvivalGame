using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class GamePlayStatusController : MonoBehaviour, IUpdateSystem
    {
        public static GamePlayStatusController Instance { get; set; }
        
        private void OnEnable()
        {
            if (GlobalUpdateSystem.Instance != null)
            {
                GlobalUpdateSystem.Instance.Add(this);
            }
        }

        private void Awake()
        {
            Instance = this;
        }
        
        private void OnDisable()
        {
            
            if (GlobalUpdateSystem.Instance != null)
            {
                GlobalUpdateSystem.Instance.Remove(this);
            }
        }
        
        private Dictionary<string, (InventoryData, float)> dictStatuses = new Dictionary<string, (InventoryData, float)>();
        
        public void OnUpdate(float deltaTime)
        {
            
        }

        public void AddInventory(string id, InventoryData data)
        {
            if (!this.dictStatuses.ContainsKey(id))
            {
                this.dictStatuses.Add(id, (data, 0));
            }
            else
            {
                this.dictStatuses[id] = (data, 0);
            }
            
            var currentHero = GamePlayController.Instance.GetSelectedHero();
            if (data.type == RPGStatType.Health)
            {
                if (data.permanent)
                {
                    currentHero.Stats.GetStat<Health>(RPGStatType.Health).Heal(data.value);
                }
            }
            else
            {
                if (data.permanent)
                {
                
                }
                else
                {
                    
                }
            }
        }

        // public void RemoveInventory(string id)
        // {
        //     //UPDATE CURRENT HERO STATS
        //     if (this.dictStatuses.ContainsKey(id))
        //     {
        //         inventory = 
        //     }
        //     
        // }
        //
        // public void UpdateCurrentHeroStatus()
        // {
        //    
        //
        //     foreach (InventoryData inventory in listStats)
        //     {
        //         var stat = currentHero.Stats.GetStat(inventory.type);
        //         stat.
        //     }
        // }
    }
}