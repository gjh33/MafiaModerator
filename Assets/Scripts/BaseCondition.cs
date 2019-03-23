using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    public abstract class BaseCondition : ScriptableObject
    {
        public enum ConditionRequirement
        {
            All,
            None,
            Exactly,
            AtMost,
            AtLeast,
        }

        public abstract bool Check(ConditionContext context);
    }
}
