using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class RPGStatCollection
    {
        protected Unit owner;
        
        public Dictionary<RPGStatType, RPGStat> DictStat { get; }

        public RPGStatCollection(Unit unit)
        {
            this.owner = unit;
            DictStat = new Dictionary<RPGStatType, RPGStat>();
        }

        public virtual void ConfigStats()
        {
            
        }
        
        public bool ContainStat(RPGStatType statType)
        {
            return DictStat.ContainsKey(statType);
        }
        
        public RPGStat GetStat(RPGStatType statType)
        {
            if (ContainStat(statType))
            {
                return DictStat[statType];
            }

            return null;
        }
        
        public T GetStat<T>(RPGStatType type) where T : RPGStat
        {
            return GetStat(type) as T;
        }
        
        protected T CreateStat<T>(RPGStatType statType) where T : RPGStat
        {
            T stat = System.Activator.CreateInstance<T>();
            DictStat.Add(statType, stat);
            return stat;
        }
        
        protected T CreateOrGetStat<T>(RPGStatType statType) where T : RPGStat
        {
            T stat = GetStat<T>(statType);
            if (stat == null)
            {
                stat = CreateStat<T>(statType);
            }

            return stat;
        }
        
        public void AddStatModifier(RPGStatType target, RPGStatModifier mod)
        {
            AddStatModifier(target, mod, false);
        }
        
        public void AddStatModifier(RPGStatType target, RPGStatModifier mod, bool update)
        {
            if (ContainStat(target))
            {
                var modStat = GetStat(target) as IStatModifiable;
                if (modStat != null)
                {
                    modStat.AddModifier(mod);
                    if (update == true)
                    {
                        modStat.UpdateModifiers();
                    }
                }
                else
                {
                    Debug.Log("[RPGStats] Trying to add Stat Modifier to non modifiable stat \"" + target.ToString() +
                              "\"");
                }
            }
            else
            {
                Debug.Log("[RPGStats] Trying to add Stat Modifier to \"" + target.ToString() +
                          "\", but RPGStats does not contain that stat");
            }
        }
        
        public void RemoveStatModifier(RPGStatType target, RPGStatModifier mod, bool update = false)
        {
            if (ContainStat(target))
            {
                var modStat = GetStat(target) as IStatModifiable;
                if (modStat != null)
                {
                    modStat.RemoveModifier(mod);
                    if (update == true)
                    {
                        modStat.UpdateModifiers();
                    }
                }
                else
                {
                    Debug.Log("[RPGStats] Trying to remove Stat Modifier from non modifiable stat \"" +
                              target.ToString() + "\"");
                }
            }
            else
            {
                Debug.Log("[RPGStats] Trying to remove Stat Modifier from \"" + target.ToString() +
                          "\", but RPGStatCollection does not contain that stat");
            }
        }
        
        public void ClearStatModifiers(bool update = true)
        {
            foreach (var key in DictStat.Keys)
            {
                ClearStatModifier(key, update);
            }
        }
        
        public void ClearStatModifier(RPGStatType target, bool update)
        {
            if (ContainStat(target))
            {
                var modStat = GetStat(target) as IStatModifiable;
                if (modStat != null)
                {
                    modStat.ClearModifiers();
                    if (update)
                    {
                        modStat.UpdateModifiers();
                    }
                }
                else
                {
                    Debug.Log("[RPGStats] Trying to clear Stat Modifiers from non modifiable stat \"" +
                              target.ToString() + "\"");
                }
            }
            else
            {
                Debug.Log("[RPGStats] Trying to clear Stat Modifiers from \"" + target.ToString() +
                          "\", but RPGStatCollection does not contain that stat");
            }
        }
        
        public void UpdateStatModifiers()
        {
            foreach (var key in DictStat.Keys)
            {
                UpdateStatModifer(key);
            }
        }

        /// <summary>
        /// Updates the target stat's modifier value
        /// </summary>
        public void UpdateStatModifer(RPGStatType target)
        {
            if (ContainStat(target))
            {
                var modStat = GetStat(target) as IStatModifiable;
                if (modStat != null)
                {
                    modStat.UpdateModifiers();
                }
                else
                {
                    Debug.Log("[RPGStats] Trying to Update Stat Modifiers for a non modifiable stat \"" +
                              target.ToString() + "\"");
                }
            }
            else
            {
                Debug.Log("[RPGStats] Trying to Update Stat Modifiers for \"" + target.ToString() +
                          "\", but RPGStatCollection does not contain that stat");
            }
        }
    }
}
