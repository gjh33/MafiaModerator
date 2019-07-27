using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Mafia
{
    public class CountdownDisplay : MonoBehaviour
    {
        public TextMeshProUGUI NumberText;

        public void SetInteger(int val)
        {
            NumberText.text = val.ToString();
        }
    }
}
