using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    public abstract class PlayerCondition : BaseCondition
    {
        public enum PlayerApplication
        {
            Self,
            Target,
            ReferenceChain,
            PlayerQuery,
            TargetQuery,
        }

        public string DisplayName;
        public PlayerApplication AppliesTo = PlayerApplication.Self;

        [ShowIf("AppliesTo", PlayerApplication.ReferenceChain)]
        public List<string> ReferenceChain = new List<string>();

        [ShowIf("AppliesTo", PlayerApplication.Target)]
        public int Target;

        [ShowIf("IsMultiPlayerOperation")]
        public PlayerQuery Query;
        [ShowIf("IsMultiPlayerOperation")]
        public ConditionRequirement Requirement = ConditionRequirement.All;
        [ShowIf("requireInputNumber")]
        public int NumberOfPlayers;

        /// <summary>
        /// Checks if the condition passes.
        /// </summary>
        /// <returns>whether or not the condition is currently passing</returns>
        protected abstract bool Pass(Player player, ConditionContext context);

        public override bool Check(ConditionContext context)
        {
            List<Player> playersToApply = new List<Player>();
            switch (AppliesTo)
            {
                // First are single target, we can return right here
                case PlayerApplication.Self:
                    return Pass(context.Subject, context);
                case PlayerApplication.Target:
                    if (context.Targets == null) return false;
                    if (context.Targets.Count <= Target) return false;
                    return Pass(context.Targets[Target], context);
                case PlayerApplication.ReferenceChain:
                    Player refPlayer = context.Subject;
                    foreach (string reference in ReferenceChain)
                    {
                        refPlayer = refPlayer.GetReference(reference);
                        if (refPlayer == null) return false;
                    }
                    return Pass(refPlayer, context);
                // Rest are multi target, we need another pass over after so break instead of returning
                case PlayerApplication.PlayerQuery:
                    playersToApply.AddRange(context.Game.Players);
                    break;
                case PlayerApplication.TargetQuery:
                    if (context.Targets == null) return false;
                    playersToApply.AddRange(context.Targets);
                    break;
                default:
                    return false;
            }

            List<Player> queried = Query.Query(playersToApply);

            int passCount = 0;
            foreach (Player player in queried)
            {
                if (Pass(player, context)) passCount++;
            }

            switch (Requirement)
            {
                case ConditionRequirement.Exactly:
                    return passCount == NumberOfPlayers;
                case ConditionRequirement.AtLeast:
                    return passCount >= NumberOfPlayers;
                case ConditionRequirement.AtMost:
                    return passCount <= NumberOfPlayers;
                case ConditionRequirement.All:
                    return passCount == queried.Count;
                case ConditionRequirement.None:
                    return passCount == 0;
                default:
                    return false;
            }
        }

        public bool IsMultiPlayerOperation()
        {
            return AppliesTo == PlayerApplication.PlayerQuery ||
                AppliesTo == PlayerApplication.TargetQuery;
        }

        private bool requireInputNumber()
        {
            return IsMultiPlayerOperation() && (
                Requirement == ConditionRequirement.AtLeast ||
                Requirement == ConditionRequirement.AtMost ||
                Requirement == ConditionRequirement.Exactly);
        }
    }
}
