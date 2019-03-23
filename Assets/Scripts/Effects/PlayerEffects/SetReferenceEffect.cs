using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    [CreateAssetMenu(menuName = "Mafia/Effects/Player/Set Reference")]
    public class SetReferenceEffect : PlayerEffect
    {
        public enum SetReferenceMode { TargetedPlayer, ExecutingPlayer }
        public string Reference;
        public SetReferenceMode SetTo = SetReferenceMode.ExecutingPlayer;
        [ShowIf("SetTo", SetReferenceMode.TargetedPlayer)]
        public int SetToTarget = 0;

        protected override void ApplyToPlayer(Player player, EffectContext context)
        {
            switch (SetTo)
            {
                case SetReferenceMode.ExecutingPlayer:
                    player.SetReference(Reference, context.ExecutingPlayer);
                    break;
                case SetReferenceMode.TargetedPlayer:
                    if (context.Targets == null) return;
                    if (context.Targets.Count <= SetToTarget) return;
                    player.SetReference(Reference, context.Targets[SetToTarget]);
                    break;
            }
        }
    }
}
