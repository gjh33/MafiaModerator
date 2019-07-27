using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    public abstract class BaseEffect : ScriptableObject
    {
        public abstract void Execute(EffectContext context);
    }
}
