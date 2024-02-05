using System;
using UnityEngine;
using LabirinKata.Enum;
using Tsukuyomi.Utilities;
using UnityEngine.Serialization;

namespace LabirinKata.Managers
{
    public class AudioManager : MonoBehaviour
    {
        #region Fields & Properties
        
        public Sound[] Musics;
        public Sound[] SoundEffects;

        public static AudioManager Instance;
        
        #endregion

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
            InitializeAudio(Musics);
        }
        
        #endregion

        #region Tsukuyomi Callbacks

        //-- Initialization
        private void InitializeMusic()
        {
            foreach (var s in Musics)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }
        }

        private void InitializeSoundEffect()
        {
            foreach (var s in SoundEffects)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }
        }
        
        private void InitializeAudio(Sound[] sounds)
        {
            foreach (var s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }
            Debug.Log("done initialize");
        }
        
        //-- Core Functionality
        public void PlayAudio(AudioList audioListName)
        {
            Sound sound = Array.Find(Musics, sound => sound.name == audioListName.ToString());
            if (sound == null)
            {
                Debug.LogWarning($"Sound: {audioListName} not found!");
                return;
            }
        
            sound.source.Play();
        }
        
        public void StopAudio(AudioList audioListName)
        { 
            Sound sound = Array.Find(Musics, sound => sound.name == audioListName.ToString());
        
            sound.source.Stop();
        }
        
        public void PauseAudio(AudioList audioListName)
        {
            Sound sound = Array.Find(Musics, sound => sound.name == audioListName.ToString());
        
            sound.source.Pause();
        }
        
        public void SetVolume(AudioList audioListName, float value)
        {
            Sound sound = Array.Find(Musics, sound => sound.name == audioListName.ToString());
        
            sound.source.volume = value;
        }
        
        public float GetVolume(AudioList audioListName)
        {
            Tsukuyomi.Utilities.Sound sound = Array.Find(Musics, sound => sound.name == audioListName.ToString());
        
            return sound.volume;
        }

        #endregion
    }
}