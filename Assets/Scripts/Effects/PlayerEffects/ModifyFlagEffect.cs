using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    [CreateAssetMenu(menuName = "Mafia/Effects/Player/Modify Flag")]
    public class ModifyFlagEffect : PlayerEffect
    {
        public enum FlagModification { Add, Remove }

        public string Flag;
        public FlagModification Modification;

        protected override void ApplyToPlayer(Player player, EffectContext context)
        {
            switch (Modification)
            {
                case FlagModification.Add:
                    player.AddFlag(Flag);
                    break;
                case FlagModification.Remove:
                    player.RemoveFlag(Flag);
                    break;
            }
        }
    }
}
