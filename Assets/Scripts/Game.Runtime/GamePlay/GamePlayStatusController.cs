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
        
        public void ClearInventory()
        {
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
                    Helper.Instance.DisplayHealthPlus(data.value, currentHero.transform.position);
                }
            }
            else
            {
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
                    var modifier = new RPGStatModifier(RPGModifierType.BaseAdd, data.value);

                    foreach (HeroBase hero in allHeroes)
                    {
                        hero.Stats.AddStatModifier(data.type, modifier);
                    }
                    
                    Helper.Instance.DisplayInventoryPlus(data.value, currentHero.transform.position);

                    var inventoryUi = LeanPool.Spawn(this.prefabUI, this.container);
                    inventoryUi.InitData(data, () => {
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