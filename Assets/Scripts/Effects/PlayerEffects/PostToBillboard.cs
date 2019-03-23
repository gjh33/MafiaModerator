using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    public class PostToBillboard : PlayerEffect
    {
        public string Message;

        protected override void ApplyToPlayer(Player player, EffectContext context)
        {
            player.BillBoard.Add(Message);
        }
    }
}
