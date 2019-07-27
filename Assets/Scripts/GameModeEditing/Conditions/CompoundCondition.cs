using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    [CreateAssetMenu(menuName = "Mafia/Conditions/Compound")]
    public class CompoundCondition : BaseCondition
    {
        public enum CompoundOperator { AND, OR }
        public CompoundOperator Operator = CompoundOperator.AND;
        List<BaseCondition> Conditions;

        public override bool Check(ConditionContext context)
        {
            switch (Operator)
            {
                case CompoundOperator.AND:
                    foreach (BaseCondition condition in Conditions)
                    {
                        if (!condition.Check(context)) return false;
                    }
                    return true;
                case CompoundOperator.OR:
                    foreach (BaseCondition condition in Conditions)
                    {
                        if (condition.Check(context)) return true;
                    }
                    return false;
                default:
                    return false;
            }
        }
    }
}
