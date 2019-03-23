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

        private void OnValidate()
        {
            List<EffectExecutionOrderEntry> ToRemove = new List<EffectExecutionOrderEntry>(ExecutionOrder);
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
                        ToRemove.Remove(entry);
                    }
                }
            }
            foreach (EffectExecutionOrderEntry entry in ToRemove)
            {
                ExecutionOrder.Remove(entry);
            }
        }
    }
}
