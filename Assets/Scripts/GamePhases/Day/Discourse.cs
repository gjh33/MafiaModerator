using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mafia
{
    public class Discourse : BaseGamePhase
    {
        public float Timer { get; private set; }
        public bool Paused { get; set; }

        public TimerDisplay TimerDisplay;
        public TimerDisplay MinifiedTimerDisplay;
        public Button StartTrialButton;
        public Button SleepEarlyButton;
        public Button BackButton;
        public PlayerSelectDisplay PlayerSelection;
        public BaseGamePhase TrialPhase;

        private bool onTrial = false;

        protected override void OnBegin()
        {
            Timer = game.FirstDay ? game.Configuration.FirstDayTimerMinutes * 60f : game.Configuration.DayTimerMinutes * 60f;
            StartTrialButton.gameObject.SetActive(true);
            if (game.Configuration.CanSleepEarly)
            {
                SleepEarlyButton.gameObject.SetActive(true);
                SleepEarlyButton.onClick.AddListener(Complete);
            }
            TimerDisplay.gameObject.SetActive(true);
            TrialPhase.Init(game);
            onTrial = false;
            StartTrialButton.onClick.AddListener(OnStartTrialButtonCallback);
            StartCoroutine(CountdownTimer());
        }

        protected override void OnComplete()
        {
            StartTrialButton.onClick.RemoveListener(OnStartTrialButtonCallback);
            SleepEarlyButton.onClick.RemoveListener(Complete);
            SleepEarlyButton.gameObject.SetActive(false);
            StartTrialButton.gameObject.SetActive(false);
            TimerDisplay.gameObject.SetActive(false);
            MinifiedTimerDisplay.gameObject.SetActive(false);
        }
        
        public void CheckComplete()
        {
            if (TrialPhase.IsDone && Timer <= 0)
            {
                Complete();
            }
        }

        private void OnStartTrialButtonCallback()
        {
            StartTrialButton.gameObject.SetActive(false);
            SleepEarlyButton.gameObject.SetActive(false);
            PlayerSelection.gameObject.SetActive(true);
            BackButton.gameObject.SetActive(true);
            List<Player> alive = new List<Player>();
            foreach (Player player in game.Players)
            {
                if (player.CurrentStatus == Player.Status.Alive)
                    alive.Add(player);
            }
            PlayerSelection.SetPlayers(alive);
            BackButton.onClick.AddListener(BackFromPlayerSelect);
            PlayerSelection.PlayerSelected += OnPlayerChosenForTrial;
        }

        private void BackFromPlayerSelect()
        {
            BackButton.onClick.RemoveListener(BackFromPlayerSelect);
            PlayerSelection.PlayerSelected -= OnPlayerChosenForTrial;
            BackButton.gameObject.SetActive(false);
            PlayerSelection.gameObject.SetActive(false);
            StartTrialButton.gameObject.SetActive(true);
            if (game.Configuration.CanSleepEarly)
            {
                SleepEarlyButton.gameObject.SetActive(true);
            }
        }

        private void OnPlayerChosenForTrial(Player player)
        {
            onTrial = true;
            BackButton.onClick.RemoveListener(BackFromPlayerSelect);
            PlayerSelection.PlayerSelected -= OnPlayerChosenForTrial;
            if (game.Configuration.PauseDayTimerOnTrial) Paused = true;
            BackButton.gameObject.SetActive(false);
            PlayerSelection.gameObject.SetActive(false);
            TimerDisplay.gameObject.SetActive(false);
            MinifiedTimerDisplay.gameObject.SetActive(true);
            game.PutPlayerOnTrial(player);
            TrialPhase.Completed += OnTrialPhaseCompleted;
            TrialPhase.Begin();
        }

        private void OnTrialPhaseCompleted()
        {
            onTrial = false;
            TrialPhase.Completed -= OnTrialPhaseCompleted;
            float roll = Random.value;
            if (roll < game.Configuration.TrialExtensionChanceNormalized && Timer <= 0)
                Timer += game.Configuration.TrialExtensionTimeMinutes * 60f;
            CheckComplete();
            // If we didn't complete
            if (!IsDone)
            {
                StartTrialButton.gameObject.SetActive(true);
                TimerDisplay.gameObject.SetActive(true);
                MinifiedTimerDisplay.gameObject.SetActive(false);
                if (game.Configuration.CanSleepEarly)
                {
                    SleepEarlyButton.gameObject.SetActive(true);
                }
            }
        }

        private IEnumerator CountdownTimer()
        {
            while (!IsDone)
            {
                yield return null;
                if (!Paused)
                {
                    Timer -= Time.unscaledDeltaTime;
                    Timer = Mathf.Max(0, Timer);
                }
                if (!onTrial)
                    TimerDisplay.SetSeconds(Timer);
                else
                    MinifiedTimerDisplay.SetSeconds(Timer);
            }
            CheckComplete();
        }
    }
}
