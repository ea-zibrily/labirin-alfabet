using System;
using LabirinKata.Enum;
using UnityEngine;

namespace LabirinKata.Managers
{
    public class AudioManager : MonoBehaviour
    {
        public Tsukuyomi.Utilities.Sound[] sounds;
        public static AudioManager Instance;

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            
            DontDestroyOnLoad(gameObject);
            
            foreach (var s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }
        }

        #endregion

        #region Tsukuyomi Callbacks

        public void PlayAudio(AudioList audioListName)
        {
            Tsukuyomi.Utilities.Sound sound = Array.Find(sounds, sound => sound.name == audioListName.ToString());
            if (sound == null)
            {
                Debug.LogWarning($"Sound: {audioListName} not found!");
                return;
            }
        
            sound.source.Play();
            Debug.Log($"Sound: {audioListName} playing!");
        }
        
        public void StopAudio(AudioList audioListName)
        {
            Tsukuyomi.Utilities.Sound sound = Array.Find(sounds, sound => sound.name == audioListName.ToString());
        
            sound.source.Stop();
            Debug.Log($"Sound: {audioListName} stops!");
        }
        
        public void PauseAudio(AudioList audioListName)
        {
            Tsukuyomi.Utilities.Sound sound = Array.Find(sounds, sound => sound.name == audioListName.ToString());
        
            sound.source.Pause();
        }
        
        public void SetVolume(AudioList audioListName, float value)
        {
            Tsukuyomi.Utilities.Sound sound = Array.Find(sounds, sound => sound.name == audioListName.ToString());
        
            sound.source.volume = value;
        }
        
        public float GetVolume(AudioList audioListName)
        {
            Tsukuyomi.Utilities.Sound sound = Array.Find(sounds, sound => sound.name == audioListName.ToString());
        
            return sound.volume;
        }

        #endregion
    }
}