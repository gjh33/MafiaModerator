using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    [CreateAssetMenu(menuName = "Mafia/Audio/Sound Pack")]
    public class SoundPack : ScriptableObject
    {
        public List<AudioClip> Sounds = new List<AudioClip>();

        public AudioClip PickSample()
        {
            if (Sounds.Count <= 0) return null;
            return Sounds[Random.Range(0, Sounds.Count)];
        }
    }
}
