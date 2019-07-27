using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mafia
{
    public class TrialPhase : BaseGamePhase
    {
        public float Timer { get; private set; }

        public AudioSource AnnouncerSource;
        public SoundPack CountdownQuipSounds;
        public SoundPack CountdownSounds;
        public TimerDisplay TrialTimerDisplay;
        public CountdownDisplay CountdownDisplay;
        public Button PushToVoteButton;
        public Button RescindButton;
        public Button GuiltyButton;
        public Button InnocentButton;

        public int Countdown = 5;

        protected override void OnBegin()
        {
            PushToVoteButton.gameObject.SetActive(false);
            RescindButton.gameObject.SetActive(false);
            GuiltyButton.gameObject.SetActive(false);
            InnocentButton.gameObject.SetActive(false);
            CountdownDisplay.gameObject.SetActive(false);
            TrialTimerDisplay.gameObject.SetActive(true);
            PushToVoteButton.onClick.AddListener(VoteButtonCallback);
            RescindButton.onClick.AddListener(RescindButtonCallback);
            GuiltyButton.onClick.AddListener(GuiltyButtonCallback);
            InnocentButton.onClick.AddListener(InnocentButtonCallback);

            StartCoroutine(TrialDiscourseTimer());
        }

        protected override void OnComplete()
        {
            PushToVoteButton.gameObject.SetActive(false);
            RescindButton.gameObject.SetActive(false);
            GuiltyButton.gameObject.SetActive(false);
            InnocentButton.gameObject.SetActive(false);
            CountdownDisplay.gameObject.SetActive(false);
            TrialTimerDisplay.gameObject.SetActive(false);
            PushToVoteButton.onClick.RemoveListener(VoteButtonCallback);
            RescindButton.onClick.RemoveListener(RescindButtonCallback);
            GuiltyButton.onClick.RemoveListener(GuiltyButtonCallback);
            InnocentButton.onClick.RemoveListener(InnocentButtonCallback);
        }

        private void VoteButtonCallback()
        {
            PushToVoteButton.gameObject.SetActive(false);
            RescindButton.gameObject.SetActive(false);
            StartCoroutine(VoteCoroutine());
        }

        private void RescindButtonCallback()
        {
            game.FreePlayerOnTrial();
            Complete();
        }

        private void GuiltyButtonCallback()
        {
            game.KillPlayerOnTrial();
            Complete();
        }

        private void InnocentButtonCallback()
        {
            game.FreePlayerOnTrial();
            Complete();
        }

        private IEnumerator TrialDiscourseTimer()
        {
            Timer = game.Configuration.TrialDiscourseTimerMinutes * 60f;
            while (!IsDone && Timer > 0)
            {
                yield return null;
                Timer -= Time.unscaledDeltaTime;
                TrialTimerDisplay.SetSeconds(Timer);
            }
            PushToVoteButton.gameObject.SetActive(true);
            RescindButton.gameObject.SetActive(true);
        }

        private IEnumerator VoteCoroutine()
        {
            Timer = game.Configuration.TrialSoloDefenseTimerMinutes * 60f;
            TrialTimerDisplay.SetSeconds(Timer);
            while (!IsDone && Timer > 0)
            {
                yield return null;
                Timer -= Time.unscaledDeltaTime;
                TrialTimerDisplay.SetSeconds(Timer);
            }
            CountdownDisplay.gameObject.SetActive(true);
            Timer = Countdown;
            AnnouncerSource.clip = CountdownQuipSounds.PickSample();
            AnnouncerSource.Play();
            yield return new WaitUntil(() => !AnnouncerSource.isPlaying);
            AnnouncerSource.clip = CountdownSounds.PickSample();
            AnnouncerSource.Play();
            while (!IsDone && (AnnouncerSource.isPlaying || Timer > 0))
            {
                yield return null;
                Timer -= Time.unscaledDeltaTime;
                CountdownDisplay.SetInteger(Mathf.CeilToInt(Timer));
            }
            CountdownDisplay.gameObject.SetActive(false);
            GuiltyButton.gameObject.SetActive(true);
            InnocentButton.gameObject.SetActive(true);
        }
    }
}
