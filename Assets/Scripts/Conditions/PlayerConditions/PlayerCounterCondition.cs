using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    [CreateAssetMenu(menuName = "Mafia/Conditions/Player/Counter")]
    public class PlayerCounterCondition : PlayerCondition
    {
        public enum CompareToType { Value, OtherCounter }
        public enum Operator { Equal, NotEqual, GreaterThan, GreaterThanOrEqualTo, LessThan, LessThanOrEqualTo }
        public Operator Comparison = Operator.Equal;
        public CompareToType CompareTo = CompareToType.Value;
        public string Counter;
        [ShowIf("CompareTo", CompareToType.Value)]
        public int Value;
        [ShowIf("CompareTo", CompareToType.OtherCounter)]
        public List<string> CounterReferenceChain;
        [ShowIf("CompareTo", CompareToType.OtherCounter)]
        public string CompareToCounter;

        protected override bool Pass(Player player, ConditionContext context)
        {
            int counterValue = player.GetCounterValue(Counter);
            int comparedValue = 0;
            switch (CompareTo)
            {
                case CompareToType.Value:
                    comparedValue = Value;
                    break;
                case CompareToType.OtherCounter:
                    Player owningPlayer = player;
                    foreach (string reference in CounterReferenceChain)
                    {
                        owningPlayer = owningPlayer.GetReference(reference);
                        if (owningPlayer == null) return false;
                    }
                    comparedValue = owningPlayer.GetCounterValue(CompareToCounter);
                    break;
            }
            switch (Comparison)
            {
                case Operator.Equal:
                    return counterValue == comparedValue;
                case Operator.NotEqual:
                    return counterValue != comparedValue;
                case Operator.GreaterThan:
                    return counterValue > comparedValue;
                case Operator.GreaterThanOrEqualTo:
                    return counterValue >= comparedValue;
                case Operator.LessThan:
                    return counterValue < comparedValue;
                case Operator.LessThanOrEqualTo:
                    return counterValue <= comparedValue;
                default:
                    return false;
            }
        }
    }
}
