using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    public class ForcePhaseExecution : MonoBehaviour
    {
        public Game Game;
        public BaseGamePhase Phase;

        [Button, HideInEditorMode]
        public void BeginPhase()
        {
            Phase.Init(Game);
            Phase.Begin();
        }
    }
}
