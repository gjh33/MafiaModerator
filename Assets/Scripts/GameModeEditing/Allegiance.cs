using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    [CreateAssetMenu(menuName = "Mafia/Allegiance")]
    public class Allegiance : ScriptableObject
    {
        public string DisplayName;
        [ColorPalette("Allegiance Colors")]
        public Color DisplayColor;
        public BaseCondition WinCondition;
    }
}
