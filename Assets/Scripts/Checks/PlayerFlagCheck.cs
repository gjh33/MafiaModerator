using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    [CreateAssetMenu(menuName = "Mafia/Checks/Flag")]
    public class PlayerFlagCheck : BaseCheck
    {
        public List<string> HasFlags = new List<string>();
        public List<string> DoesNotHaveFlags = new List<string>();

        public override string GetResult(Player player)
        {
            foreach (string flag in HasFlags)
            {
                if (!player.HasFlag(flag)) return "No";
            }
            foreach (string flag in DoesNotHaveFlags)
            {
                if (player.HasFlag(flag)) return "No";
            }
            return "Yes";
        }
    }
}
