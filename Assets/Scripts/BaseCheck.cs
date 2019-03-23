using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    public abstract class BaseCheck : ScriptableObject
    {
        public abstract string GetResult(Player player);
    }
}
