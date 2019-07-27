using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    public struct ActionContext
    {
        /// <summary>
        /// The game this action was executed in
        /// </summary>
        public Game Game;
        /// <summary>
        /// The Player that is performing this action. Note that their
        /// role may no longer match the role when the action was performed.
        /// Please use PerformingRole to get this information.
        /// </summary>
        public Player PerformingPlayer;
        /// <summary>
        /// The Role that peformed this action.
        /// </summary>
        public Role PerformingRole;
        /// <summary>
        /// If targets were selected for this action they will be here
        /// </summary>
        public List<Player> Targets;
    }
}
