using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public enum AudioSourceType
    {
        Main,
        Second
    }

    [System.Serializable]
    public struct AudioPlayerData
    {
        public AudioSourceType sourceType;
        public AudioSource audioSource;
    }

    public class AudioPlayer
    {
        private readonly Dictionary<AudioSourceType, AudioSource> _sources = new();


        public AudioPlayer(AudioPlayerData[] dataList)
        {
            foreach (var data in dataList)
            {
                _sources[data.sourceType] = data.audioSource;
            }
        }


        public void AddSource(AudioSourceType type, AudioSource audioSource) => _sources[type] = audioSource;


        public void RemoveSource(AudioSourceType type) => _sources.Remove(type);


        public void SetPitch(AudioSourceType type, float pitch)
        {
            if (_sources.TryGetValue(type, out var source)) source.pitch = pitch;
        }


        public void SetAudioClip(AudioSourceType type, AudioClip clip)
        {
            if (_sources.TryGetValue(type, out var source)) source.clip = clip;
        }


        public void Play(AudioSourceType type)
        {
            if (_sources.TryGetValue(type, out var source)) source.Play();
        }
    }
}