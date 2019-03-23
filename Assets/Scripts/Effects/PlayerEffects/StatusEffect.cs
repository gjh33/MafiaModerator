using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    [CreateAssetMenu(menuName = "Mafia/Effects/Player/Status")]
    public class StatusEffect : PlayerEffect
    {
        public Player.Status SetStatusTo;

        protected override void ApplyToPlayer(Player player, EffectContext context)
        {
            player.CurrentStatus = SetStatusTo;
        }
    }
}
