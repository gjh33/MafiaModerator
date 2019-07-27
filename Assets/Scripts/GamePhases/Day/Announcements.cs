using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    public class Announcements : BaseGamePhase
    {
        public AudioSource AudioSource;
        public SoundPack WakeUpSounds;
        public SoundPack NoOneDiedSounds;
        public SoundPack SomeoneDiedSounds;

        public float WaitForWakeupSeconds = 3f;
        public float WaitBeforeCompleteSeconds = 2f;

        private List<Player> LastDeadPlayers = new List<Player>();
        private List<Player> NewlyDeadPlayers = new List<Player>();

        protected override void OnInitialize()
        {
            LastDeadPlayers.Clear();
        }

        protected override void OnBegin()
        {
            NewlyDeadPlayers.Clear();
            foreach (Player player in game.Players)
            {
                if (player.CurrentStatus == Player.Status.Dead && !LastDeadPlayers.Contains(player))
                {
                    NewlyDeadPlayers.Add(player);
                }
            }
            StartCoroutine(PlayAnnouncements());
        }

        protected override void OnComplete()
        {
            LastDeadPlayers.AddRange(NewlyDeadPlayers);
        }

        private IEnumerator PlayAnnouncements()
        {
            AudioSource.clip = WakeUpSounds.PickSample();
            AudioSource.Play();
            yield return new WaitUntil(() => !AudioSource.isPlaying);
            yield return new WaitForSecondsRealtime(WaitForWakeupSeconds);
            if (!game.FirstDay)
            {
                if (NewlyDeadPlayers.Count > 0)
                {
                    AudioSource.clip = SomeoneDiedSounds.PickSample();
                }
                else
                {
                    AudioSource.clip = NoOneDiedSounds.PickSample();
                }
                AudioSource.Play();
                yield return new WaitUntil(() => !AudioSource.isPlaying);
                yield return new WaitForSecondsRealtime(WaitBeforeCompleteSeconds);
            }
            Complete();
        }
    }
}
