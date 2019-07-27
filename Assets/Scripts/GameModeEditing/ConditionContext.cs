using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    public struct ConditionContext
    {
        /// <summary>
        /// The game the condition is evaluated in
        /// </summary>
        public Game Game;
        /// <summary>
        /// The subject of the condition. For an action or effect
        /// this is the player who called it
        /// </summary>
        public Player Subject;
        /// <summary>
        /// Any aditional targets that may be referenced and passed.
        /// For an action of effect this is the targeted players.
        /// </summary>
        public List<Player> Targets;
    }
}
