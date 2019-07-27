using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    public class SkipPlayerEntry : MonoBehaviour
    {
        public Game Game;
        
        [Button, HideInEditorMode]
        public void InjectPlayers()
        {
            char playerName = 'A';
            List<Role> roles = new List<Role>();
            foreach (GameMode.Variant.Entry entry in Game.Variant.RoleCounts)
            {
                for (int i = 0; i < entry.Count; i++)
                {
                    roles.Add(entry.Role);
                }
            }

            // Shitty shuffle cause this is for test
            roles.Sort((a, b) => Random.Range(-1, 1));
            roles.Sort((a, b) => Random.Range(-1, 1));
            roles.Sort((a, b) => Random.Range(-1, 1));
            roles.Sort((a, b) => Random.Range(-1, 1));

            foreach (Role role in roles)
            {
                Player player = new Player(playerName.ToString());
                player.CurrentRole = role;
                player.CurrentAllegiance = role.StartingAllegiance;
                Game.Players.Add(player);
                playerName++;
            }
        }
    }
}
