using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    [System.Serializable]
    public class Player
    {
        public enum Status { Any, Alive, Dead }

        [ShowInInspector, ReadOnly]
        public string Name { get; private set; }
        [ShowInInspector, ReadOnly]
        public Status CurrentStatus { get; set; }
        [ShowInInspector, ReadOnly]
        public Role CurrentRole { get; set; }
        [ShowInInspector, ReadOnly]
        public Allegiance CurrentAllegiance { get; set; }

        public List<string> BillBoard = new List<string>();

        private HashSet<string> flags = new HashSet<string>();
        private Dictionary<string, int> counters = new Dictionary<string, int>();
        private Dictionary<string, Player> references = new Dictionary<string, Player>();

        public Player(string name)
        {
            Name = name;
            CurrentStatus = Status.Alive;
        }

        public void AddFlag(string flag)
        {
            flags.Add(flag.ToUpper());
        }

        public bool HasFlag(string flag)
        {
            return flags.Contains(flag.ToUpper());
        }

        public void RemoveFlag(string flag)
        {
            flags.Remove(flag.ToUpper());
        }

        public void ModifyCounter(string counter, int amount)
        {
            counter = counter.ToUpper();
            if (!counters.ContainsKey(counter)) counters[counter] = 0;
            counters[counter] += amount;
        }

        public int GetCounterValue(string counter)
        {
            counter = counter.ToUpper();
            if (!counters.ContainsKey(counter)) return 0;
            return counters[counter];
        }

        public void SetReference(string reference, Player referencePlayer)
        {
            reference = reference.ToUpper();
            references[reference] = referencePlayer;
        }

        public Player GetReference(string reference)
        {
            reference = reference.ToUpper();
            if (!references.ContainsKey(reference)) return null;
            return references[reference];
        }
    }
}
