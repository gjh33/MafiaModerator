using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    public class ForceStartGame : MonoBehaviour
    {
        public Game Game;

        [Button]
        public void StartGame()
        {
            Game.Begin();
        }
    }
}
