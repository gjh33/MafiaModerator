using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Mafia
{
    [CreateAssetMenu(menuName = "Mafia/GameMode")]
    public class GameMode : ScriptableObject
    {
        public string DisplayName;
        public List<Role> Roles = new List<Role>();

        [ListDrawerSettings(HideAddButton = true)]
        public List<EffectExecutionOrderEntry> ExecutionOrder = new List<EffectExecutionOrderEntry>();

        [ListDrawerSettings(CustomAddFunction = "CreateVariant", NumberOfItemsPerPage = 1)]
        public List<Variant> Variants = new List<Variant>();

        public List<int> GetValidPlayerCounts()
        {
            List<int> toRet = new List<int>();
            foreach (Variant variant in Variants)
            {
                toRet.Add(variant.GetPlayerCount());
            }
            return toRet;
        }

        public void CreateVariant()
        {
            CreateVariant("New Variant");
        }

        public void CreateVariant(string name)
        {
            Variant variant = new Variant();
            variant.RoleCounts = new List<Variant.Entry>();
            variant.Name = name;
            foreach (Role role in Roles)
            {
                variant.RoleCounts.Add(new Variant.Entry
                {
                    Role = role,
                    Count = 0,
                });
            }
            Variants.Add(variant);
        }

        private void OnValidate()
        {
            // Build Execution Order List
            List<EffectExecutionOrderEntry> toRemove = new List<EffectExecutionOrderEntry>(ExecutionOrder);
            foreach (Role role in Roles)
            {
                foreach (Action action in role.Actions)
                {
                    foreach (BaseEffect effect in action.Effects)
                    {
                        EffectExecutionOrderEntry entry = new EffectExecutionOrderEntry
                        {
                            ExecutingRole = role,
                            ExecutingAction = action,
                            Effect = effect,
                        };
                        if (!ExecutionOrder.Contains(entry)) ExecutionOrder.Add(entry);
                        toRemove.Remove(entry);
                    }
                }
            }
            foreach (EffectExecutionOrderEntry entry in toRemove)
            {
                ExecutionOrder.Remove(entry);
            }

            // Ensure Roles are Unique
            HashSet<Role> uniqueRoles = new HashSet<Role>();
            List<Role> rolesToRemove = new List<Role>();
            foreach (Role role in Roles)
            {
                if (!uniqueRoles.Contains(role)) uniqueRoles.Add(role);
                else rolesToRemove.Add(role);
            }
            foreach (Role role in rolesToRemove) Roles.Remove(role);

            // Ensure variants has entry for each role and no more
            foreach (Variant variant in Variants)
            {
                Dictionary<Role, int> roleCounts = new Dictionary<Role, int>();
                List<Variant.Entry> entryRemove = new List<Variant.Entry>();
                foreach (Variant.Entry entry in variant.RoleCounts)
                {
                    if (!Roles.Contains(entry.Role))
                    {
                        entryRemove.Add(entry);
                    }
                    if (!roleCounts.ContainsKey(entry.Role)) roleCounts[entry.Role] = 0;
                    roleCounts[entry.Role]++;
                }
                foreach (Variant.Entry entry in entryRemove) variant.RoleCounts.Remove(entry);
                foreach (Role role in Roles)
                {
                    if (!roleCounts.ContainsKey(role) || roleCounts[role] <= 0)
                    {
                        variant.RoleCounts.Add(new Variant.Entry
                        {
                            Role = role,
                            Count = 0,
                        });
                    }
                    else if (roleCounts[role] > 1)
                    {
                        for (int i = 0; i < roleCounts[role] - 1; i++)
                        {
                            variant.RoleCounts.RemoveAt(variant.RoleCounts.FindLastIndex((Variant.Entry entry) => entry.Role == role));
                        }
                    }
                }
            }

            // Ensure there's always at least one variant
            if (Variants.Count <= 0)
            {
                CreateVariant("Default Variant");
            }
        }

        [System.Serializable]
        public struct Variant
        {
            public string Name;
            [TableList(HideToolbar = true, AlwaysExpanded = true)]
            public List<Entry> RoleCounts;

            public int GetPlayerCount()
            {
                int count = 0;
                foreach (Entry entry in RoleCounts)
                {
                    count += entry.Count;
                }
                return count;
            }

            [System.Serializable]
            public struct Entry
            {
                [ReadOnly]
                public Role Role;
                public int Count;
            }
        }
    }
}
