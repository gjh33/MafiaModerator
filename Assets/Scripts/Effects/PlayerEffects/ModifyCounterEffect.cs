using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    [CreateAssetMenu(menuName = "Mafia/Effects/Player/Modify Counter")]
    public class ModifyCounterEffect : PlayerEffect
    {
        public string Counter;
        public int ModificationAmount = 1;

        protected override void ApplyToPlayer(Player player, EffectContext context)
        {
            player.ModifyCounter(Counter, ModificationAmount);
        }
    }
}
