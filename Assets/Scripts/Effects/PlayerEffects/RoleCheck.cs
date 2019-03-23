using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    [CreateAssetMenu(menuName = "Mafia/Effects/Player/Check/Role")]
    public class RoleCheck : PlayerEffect
    {
        protected override void ApplyToPlayer(Player player, EffectContext context)
        {
            context.ExecutingPlayer.BillBoard.Add(player.Name + " is a " + player.CurrentRole);
        }
    }
}
