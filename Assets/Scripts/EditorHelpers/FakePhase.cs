using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    public class FakePhase : BaseGamePhase
    {
        protected override void OnBegin()
        {
        }

        protected override void OnComplete()
        {
        }

        [Button, HideInEditorMode]
        public void ForceComplete()
        {
            Complete();
        }
    }
}
