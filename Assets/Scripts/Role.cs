using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    [CreateAssetMenu(menuName = "Mafia/Role")]
    public class Role : ScriptableObject
    {
        public string DisplayName;
        public Allegiance StartingAllegiance;
        public List<Action> Actions = new List<Action>();
        public BaseCondition WinCondition;
    }
}
