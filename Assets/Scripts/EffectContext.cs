using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    [System.Serializable]
    public struct EffectContext
    {
        /// <summary>
        /// The game this was executed in
        /// </summary>
        public Game Game;
        /// <summary>
        /// The player that executed the query. Be aware that the player's
        /// role etc may have changed since execution. You should query the
        /// context parameters over the player's.
        /// </summary>
        public Player ExecutingPlayer;
        /// <summary>
        /// The role that executed this effect. This may not equal the executing
        /// player's current role
        /// </summary>
        public Role ExecutingRole;
        /// <summary>
        /// The action that executed this effect.
        /// </summary>
        public Action ExecutingAction;
        /// <summary>
        /// A list of targets. Can be null, 1, or multiple
        /// </summary>
        public List<Player> Targets;
    }
}
