using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    [CreateAssetMenu(menuName = "Mafia/Conditions/Player/QueryInformation")]
    public class PlayerQueryCondition : BaseCondition
    {
        public PlayerQuery Query;
        public ConditionRequirement Match;
        [ShowIf("showAmount")]
        public int Amount;

        public override bool Check(ConditionContext context)
        {
            List<Player> query = Query.Query(context.Game.Players);

            switch (Match)
            {
                case ConditionRequirement.All:
                    return query.Count == context.Game.Players.Count;
                case ConditionRequirement.None:
                    return query.Count == 0;
                case ConditionRequirement.AtLeast:
                    return query.Count >= Amount;
                case ConditionRequirement.AtMost:
                    return query.Count <= Amount;
                case ConditionRequirement.Exactly:
                    return query.Count == Amount;
                default:
                    return false;
            }
        }

        private bool showAmount()
        {
            return Match == ConditionRequirement.AtLeast ||
                Match == ConditionRequirement.AtMost ||
                Match == ConditionRequirement.Exactly;
        }
    }
}
