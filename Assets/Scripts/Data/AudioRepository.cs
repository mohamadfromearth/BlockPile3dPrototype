using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    [CreateAssetMenu(fileName = "AudioRepository", menuName = "so/AudioRepository", order = 0)]
    public class AudioRepository : ScriptableObject
    {
        public AudioClip bubble;
        public AudioClip blockPickUp;
        public AudioClip coinCollect;
        public AudioClip soundTrack;
        public AudioClip lose;
        public AudioClip win;


        public float blockPushingInitialPitch;
        public float blockPushingPitchInterval;
        public float blockDestroyingInitialPitch;
        public float blockDestroyingPitchInterval;
    }
}