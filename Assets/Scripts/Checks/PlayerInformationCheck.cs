using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    [CreateAssetMenu(menuName = "Mafia/Checks/Player Information")]
    public class PlayerInformationCheck : BaseCheck
    {
        public enum Type { Role, Allegiance, Status }
        public Type Check;
        public override string GetResult(Player player)
        {
            switch (Check)
            {
                case Type.Allegiance:
                    return player.CurrentAllegiance.DisplayName;
                case Type.Role:
                    return player.CurrentRole.DisplayName;
                case Type.Status:
                    if (player.CurrentStatus == Player.Status.Alive) return "Alive";
                    else return "Dead";
                default:
                    return "";
            }
        }
    }
}
