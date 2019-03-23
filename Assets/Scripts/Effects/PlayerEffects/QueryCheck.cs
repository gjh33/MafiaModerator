using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Mafia
{
    [CreateAssetMenu(menuName = "Mafia/Effects/Player/Check/Query")]
    public class QueryCheck : PlayerEffect
    {
        public PlayerQuery CheckQuery;
        public string QueryPassResponse;
        public string QueryFailResponse;

        protected override void ApplyToPlayer(Player player, EffectContext context)
        {
            string response = "";
            if (Query.MatchPlayer(player))
            {
                response = QueryPassResponse;
            }
            else
            {
                response = QueryFailResponse;
            }
            context.ExecutingPlayer.BillBoard.Add(player.Name + response);
        }
    }
}
