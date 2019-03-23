using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    [CreateAssetMenu(menuName = "Mafia/Checks/Query")]
    public class PlayerQueryCheck : BaseCheck
    {
        public PlayerQuery Query;

        public override string GetResult(Player player)
        {
            if (Query.MatchPlayer(player)) return "Yes";
            else return "No";
        }
    }
}
