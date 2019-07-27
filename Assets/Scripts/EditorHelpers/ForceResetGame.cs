using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    public class ForceResetGame : MonoBehaviour
    {
        public Game Game;
        public GameMode Mode;
        public int VariantIndex;

        [Button, HideInEditorMode]
        public void ResetGame()
        {
            Game.ResetGame(Mode, Mode.Variants[VariantIndex], Game.DefaultSettings);
        }
    }
}
