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

        public List<Player> Query(IEnumerable<Player> players)
        {
            List<Player> toRet = new List<Player>();
            foreach (Player player in players)
            {
                if (MatchPlayer(player)) toRet.Add(player);
            }
            return toRet;
        }

        public bool MatchPlayer(Player player)
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
            public enum Operator { Equal, NotEqual, GreaterThan, GreaterThanOrEqualTo, LessThan, LessThanOrEqualTo }

            public string Counter;
            public Operator Comparison;
            public int Value;
        }
    }
}
