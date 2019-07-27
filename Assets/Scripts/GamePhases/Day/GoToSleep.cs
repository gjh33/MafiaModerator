using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    public class GoToSleep : BaseGamePhase
    {
        public float preSecond = 2f;
        public float postSeconds = 10f;

        public AudioSource announcerSource;
        public SoundPack goToSleepSounds;

        protected override void OnBegin()
        {
            StartCoroutine(MakeAnnouncement());
        }

        protected override void OnComplete()
        {
        }

        private IEnumerator MakeAnnouncement()
        {
            yield return new WaitForSecondsRealtime(preSecond);
            announcerSource.clip = goToSleepSounds.PickSample();
            announcerSource.Play();
            yield return new WaitUntil(() => !announcerSource.isPlaying);
            yield return new WaitForSecondsRealtime(postSeconds);
            Complete();
        }
    }
}
