using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia {
    public abstract class PlayerEffect : BaseEffect
    {
        public enum EffectApplication {
            ExecutingPlayer,
            Target,
            ReferenceChain,
            PlayerQuery,
            TargetQuery,
        }

        public EffectApplication AppliesTo = EffectApplication.ExecutingPlayer; 
        [ShowIf("AppliesTo", EffectApplication.ReferenceChain)]
        public List<string> ReferenceChain = new List<string>(); // Only valid if target is ReferenceChain type
        [ShowIf("AppliesTo", EffectApplication.Target)]
        public int Target;
        [ShowIf("IsMultiPlayerOperation")]
        public PlayerQuery Query;

        /// <summary>
        /// This is called on every effect, and is how it modifies
        /// game state
        /// </summary>
        /// <param name="game">the game it's affecting</param>
        protected abstract void ApplyToPlayer(Player player, EffectContext context);

        public override void Execute(EffectContext context)
        {
            List<Player> playersToApply = new List<Player>();
            switch (AppliesTo)
            {
                case EffectApplication.ExecutingPlayer:
                    ApplyToPlayer(context.ExecutingPlayer, context);
                    break;
                case EffectApplication.ReferenceChain:
                    Player curPlayer = context.ExecutingPlayer;
                    foreach (string reference in ReferenceChain)
                    {
                        curPlayer = curPlayer.GetReference(reference);
                        if (curPlayer == null)
                        {
                            Debug.LogError("Could not follow reference chain. Reference does not exist: " + reference);
                            return;
                        }
                    }
                    ApplyToPlayer(curPlayer, context);
                    break;
                case EffectApplication.Target:
                    if (context.Targets == null) return;
                    if (context.Targets.Count <= Target) return;
                    ApplyToPlayer(context.Targets[Target], context);
                    break;
                case EffectApplication.PlayerQuery:
                    playersToApply.AddRange(context.Game.Players);
                    break;
                case EffectApplication.TargetQuery:
                    if (context.Targets == null) return;
                    playersToApply.AddRange(context.Targets);
                    break;
            }

            List<Player> queried = Query.Query(playersToApply);
            foreach (Player player in queried)
            {
                ApplyToPlayer(player, context);
            }
        }

        public bool IsMultiPlayerOperation()
        {
            return AppliesTo == EffectApplication.PlayerQuery ||
                AppliesTo == EffectApplication.TargetQuery;
        }
    }
}
