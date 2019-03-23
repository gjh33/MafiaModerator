using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    public class Game : MonoBehaviour
    {
        public static Game ActiveGame { get; private set; }

        public GameMode Mode { get; private set; }

        [ReadOnly, PropertyOrder(1000)]
        public List<Player> Players = new List<Player>();

        private void Awake()
        {
            if (ActiveGame != null)
            {
                Destroy(gameObject);
            }
            ActiveGame = this;
        }

        private void OnDestroy()
        {
            if (ActiveGame == this)
            {
                ActiveGame = null;
            }
        }

        [Button(Expanded = true)]
        public void ResetGame(GameMode mode)
        {
            Players.Clear();
            Mode = mode;
        }
    }
}
