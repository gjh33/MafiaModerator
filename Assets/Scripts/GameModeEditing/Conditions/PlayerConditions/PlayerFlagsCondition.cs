using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    [CreateAssetMenu(menuName = "Mafia/Conditions/Player/Flags")]
    public class PlayerFlagsCondition : PlayerCondition
    {
        public List<string> HasFlags = new List<string>();
        public List<string> DoesNotHaveFlags = new List<string>();

        protected override bool Pass(Player player, ConditionContext context)
        {
            foreach (string flag in HasFlags)
            {
                if (!player.HasFlag(flag)) return false;
            }
            foreach (string flag in DoesNotHaveFlags)
            {
                if (player.HasFlag(flag)) return false;
            }
            return true;
        }
    }
}
