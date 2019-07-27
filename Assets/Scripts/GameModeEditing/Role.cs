using Sirenix.OdinInspector;
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
        public List<string> StartingFlags = new List<string>();
        [TableList]
        public List<CounterEntry> StartingCounters = new List<CounterEntry>();
        public List<Action> SetupActions = new List<Action>();
        public List<Action> Actions = new List<Action>();
        public BaseCondition WinCondition;


        [System.Serializable]
        public struct CounterEntry
        {
            public string Counter;
            public int Value;
        }
    }
}
