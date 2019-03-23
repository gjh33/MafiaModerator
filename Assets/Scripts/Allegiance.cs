using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    [CreateAssetMenu(menuName = "Mafia/Allegiance")]
    public class Allegiance : ScriptableObject
    {
        public string DisplayName;
        public Color DisplayColor;
        public List<PlayerCondition> WinCondition = new List<PlayerCondition>();
    }
}
