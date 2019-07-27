using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    [System.Serializable]
    public struct PlayerQuery
    {
        public Player.Status Status;
        public Allegiance RequiredAllegiance;
        public Role RequiredRole;
        public List<string> RequiredFlags;
        public List<string> ForbiddenFlags;
        [TableList]
        public List<CounterQuery> CounterRequirements;

        private Dictionary<string, List<Player>> sortedByCounterCache;

        /// <summary>
        /// Query a set of players for several conditions
        /// </summary>
        /// <param name="players">set of players to query against</param>
        /// <returns>a set of players matching the query requirements</returns>
        public List<Player> Query(IEnumerable<Player> players)
        {
            sortedByCounterCache = new Dictionary<string, List<Player>>();
            List<Player> toRet = new List<Player>();
            foreach (Player player in players)
            {
                if (MatchPlayer(player, players)) toRet.Add(player);
            }
            return toRet;
        }

        // Private because API so not clear. Instead use Query, then check if the results
        // contain the player you want.
        private bool MatchPlayer(Player player, IEnumerable<Player> players)
        {
            if (Status != Player.Status.Any && player.CurrentStatus != Status) return false;
            if (RequiredAllegiance != null && player.CurrentAllegiance != RequiredAllegiance) return false;
            if (RequiredRole != null && player.CurrentRole != RequiredRole) return false;
            foreach (string flag in RequiredFlags)
            {
                if (!player.HasFlag(flag)) return false;
            }
            foreach (string flag in ForbiddenFlags)
            {
                if (player.HasFlag(flag)) return false;
            }

            foreach (CounterQuery cq in CounterRequirements)
            {
                int value = player.GetCounterValue(cq.Counter);
                bool pass = true;

                // We need a list ordered by counter value
                List<Player> counterSorted = null;
                if (cq.Comparison == CounterQuery.Operator.InTop ||
                    cq.Comparison == CounterQuery.Operator.InBottom)
                {
                    if (sortedByCounterCache != null && sortedByCounterCache.ContainsKey(cq.Counter))
                    {
                        counterSorted = sortedByCounterCache[cq.Counter];
                    }
                    else
                    {
                        counterSorted = new List<Player>(players);
                        counterSorted.Sort((Player p1, Player p2) => {
                            return p1.GetCounterValue(cq.Counter).CompareTo(p2.GetCounterValue(cq.Counter));
                        });
                        if (sortedByCounterCache != null) sortedByCounterCache[cq.Counter] = counterSorted;
                    }
                }

                int index = -1; // used in top/bottom
                switch (cq.Comparison)
                {
                    case CounterQuery.Operator.Equal:
                        pass = value == cq.Value;
                        break;
                    case CounterQuery.Operator.NotEqual:
                        pass = value != cq.Value;
                        break;
                    case CounterQuery.Operator.GreaterThan:
                        pass = value > cq.Value;
                        break;
                    case CounterQuery.Operator.GreaterThanOrEqualTo:
                        pass = value >= cq.Value;
                        break;
                    case CounterQuery.Operator.LessThan:
                        pass = value < cq.Value;
                        break;
                    case CounterQuery.Operator.LessThanOrEqualTo:
                        pass = value <= cq.Value;
                        break;
                    case CounterQuery.Operator.InTop:
                        index = counterSorted.IndexOf(player);
                        if (index < 0 || index >= counterSorted.Count)
                        {
                            pass = false;
                            break;
                        }
                        pass = index >= counterSorted.Count - cq.Value;
                        break;
                    case CounterQuery.Operator.InBottom:
                        index = counterSorted.IndexOf(player);
                        if (index < 0 || index >= counterSorted.Count)
                        {
                            pass = false;
                            break;
                        }
                        pass = index < cq.Value;
                        break;
                    default:
                        pass = false;
                        break;
                }
                if (!pass) return false;
            }
            return true;
        }

        [System.Serializable]
        public struct CounterQuery
        {
            public enum Operator {
                Equal,
                NotEqual,
                GreaterThan,
                GreaterThanOrEqualTo,
                LessThan,
                LessThanOrEqualTo,
                InTop,
                InBottom,
            }

            public string Counter;
            public Operator Comparison;
            public int Value;
        }
    }
}
