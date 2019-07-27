using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using TMPro;

namespace Mafia
{
    public class PlayerSelectDisplay : MonoBehaviour
    {
        public delegate void PlayerSelectedCallback(Player selectedPlayer);
        public event PlayerSelectedCallback PlayerSelected;

        public Button FloatingButtonTemplate;
        public Transform Container;

        private Dictionary<Button, Player> ButtonLookup = new Dictionary<Button, Player>();

        public void SetPlayers(IEnumerable<Player> players)
        {
            ButtonLookup.Clear();
            List<Transform> children = new List<Transform>();
            foreach (Transform child in Container) children.Add(child);
            foreach (Transform child in children)
            {
                child.parent = null;
                Destroy(child.gameObject);
            }

            foreach(Player player in players)
            {
                Button button = Instantiate(FloatingButtonTemplate, Container);
                ButtonLookup[button] = player;
                button.onClick.AddListener(() => { OnButtonPressed(button); });
                TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();
                if (text != null) text.text = player.Name;
            }
        }

        private void OnButtonPressed(Button button)
        {
            PlayerSelected?.Invoke(ButtonLookup[button]);
        }
    }
}
