using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    /// <summary>
    /// Defines a triplet of role, action, effect that should
    /// be added to game mode execution order to control when
    /// they are executed
    /// </summary>
    [System.Serializable]
    public struct EffectExecutionOrderEntry
    {
        public Role ExecutingRole;
        public Action ExecutingAction;
        public BaseEffect Effect;
    }
}
