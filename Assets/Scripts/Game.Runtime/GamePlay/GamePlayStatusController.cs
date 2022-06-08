using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

namespace Game.Runtime
{
    public class GamePlayStatusController : MonoBehaviour
    {
        [SerializeField] private Transform container;
        [SerializeField] private InventoryUI prefabUI;
        
        public static GamePlayStatusController Instance { get; set; }
        
        private void Awake()
        {
            Instance = this;
        }
        
        
        private Dictionary<string, InventoryData> dictStatuses = new Dictionary<string, InventoryData>();

        public void ClearInventory()
        {
            this.dictStatuses.Clear();
            foreach (Transform trans in this.container)
            {
                LeanPool.Despawn(trans.gameObject);
            }
        }

        public void AddInventory(string id, InventoryData data)
        {
            SoundController.Instance.PlayPickItem();
            
            var currentHero = GamePlayController.Instance.GetSelectedHero();
            if (data.type == RPGStatType.Health)
            {
                if (data.permanent)
                {
                    currentHero.Stats.GetStat<Health>(RPGStatType.Health).Heal(data.value);
                    UIManager.Instance.CreateFloatingText("+" + data.value, new Color32(0, 219, 4, 255),  
                        new Vector2(currentHero.transform.position.x, currentHero.transform.position.y + 1.5f));
                }
            }
            else
            {
                if (!this.dictStatuses.ContainsKey(id))
                {
                    this.dictStatuses.Add(id, data);
                }
                else
                {
                    this.dictStatuses[id] = data;
                }

                var allHeroes = GamePlayController.Instance.GetAllHero();

                
                
                if (data.permanent)
                {
                    foreach (HeroBase hero in allHeroes)
                    {
                        hero.Stats.GetStat(data.type).StatBaseValue += data.value;
                    }
                }
                else
                {
                    var modifier = new RPGStatModifier(RPGModifierType.BaseAdd, data.value, null);

                    foreach (HeroBase hero in allHeroes)
                    {
                        hero.Stats.AddStatModifier(data.type, modifier);
                    }
                    
                    UIManager.Instance.CreateFloatingText("+" + data.value, new Color32(101, 193, 219, 255),  
                        new Vector2(currentHero.transform.position.x, currentHero.transform.position.y + 1.5f));

                    var inventoryUI = LeanPool.Spawn(this.prefabUI, this.container);
                    inventoryUI.InitData(data, () => {
                        foreach (HeroBase hero in allHeroes)
                        {
                            hero.Stats.RemoveStatModifier(data.type, modifier);
                        }
                    });
                }
            }
        }
    }
}