using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace Mafia
{
    public class NameEntryPhase : BaseGamePhase
    {
        public List<GameObject> EnableDuringPhase = new List<GameObject>();
        public Button PassButton;
        public TMP_InputField NameInput;
        public TextMeshProUGUI RoleText;
        public BaseGamePhase PassPhase;

        [ReadOnly]
        public List<Role> roles = new List<Role>();
        private int curIndex = 0;

        protected override void OnBegin()
        {
            // Setup UI
            foreach (GameObject obj in EnableDuringPhase)
            {
                obj.SetActive(true);
            }
            PassButton.onClick.AddListener(PassButtonPressedCallback);
            NameInput.onEndEdit.AddListener(OnTextEditEndCallback);
            PassButton.interactable = false;

            // Setup and distribute roles
            roles.Clear();
            foreach (var roleCount in game.Variant.RoleCounts)
            {
                for (int i = 0; i < roleCount.Count; i++)
                    roles.Add(roleCount.Role);
            }
            ShuffleRoles();

            // Clear players and display first role
            game.Players.Clear();
            RoleText.text = roles[curIndex].DisplayName;
            RoleText.color = roles[curIndex].StartingAllegiance.DisplayColor;

            // Setup pass phase
            PassPhase.Init(game);
            PassPhase.Completed += OnPassPhaseEndCallback;
        }

        protected override void OnComplete()
        {
            foreach (GameObject obj in EnableDuringPhase)
            {
                obj.SetActive(false);
            }
            PassPhase.Completed -= OnPassPhaseEndCallback;
            PassButton.onClick.RemoveListener(PassButtonPressedCallback);
            NameInput.onEndEdit.RemoveListener(OnTextEditEndCallback);
        }

        private void PassButtonPressedCallback()
        {
            Player player = new Player(NameInput.text);
            player.CurrentRole = roles[curIndex];
            player.CurrentAllegiance = player.CurrentRole.StartingAllegiance;
            game.Players.Add(player);
            NameInput.text = "";
            RoleText.text = "";
            PassButton.interactable = false;
            curIndex++;
            if (curIndex >= roles.Count) Complete();
            else
            {
                PassPhase.Begin();
            }
        }

        private void OnPassPhaseEndCallback()
        {
                RoleText.text = roles[curIndex].DisplayName;
                RoleText.color = roles[curIndex].StartingAllegiance.DisplayColor;
        }

        private void OnTextEditEndCallback(string val)
        {
            if (string.IsNullOrEmpty(val)) PassButton.interactable = false;
            else PassButton.interactable = true;
        }

        private void ShuffleRoles()
        {
            Random.InitState(Mathf.RoundToInt(Time.realtimeSinceStartup) - Time.frameCount);
            Role temp;
            for (int i = 0; i < roles.Count; i++)
            {
                int r = Random.Range(i, roles.Count);
                temp = roles[r];
                roles[r] = roles[i];
                roles[i] = temp;
            }
        }
    }
}
