using System;
using UnityEngine;
using Alphabet.Enum;
using Tsukuyomi.Utilities;

namespace Alphabet.Managers
{
    public class AudioManager : MonoBehaviour
    {
        #region Fields & Properties
        
        [Header("Audio Data")]
        public Sound[] Musics;

        [Header("Container")]
        [SerializeField] private GameObject musicsContainer;
        [SerializeField] private GameObject sfxsContainer;

        public Musics LatestMusic { get; set; } = Enum.Musics.none;
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
            InitializeAudio();
        }
        
        #endregion

        #region Methods

        //!-- Initialization
        private void InitializeAudio()
        {
            foreach (var s in Musics)
            {
                var container = s.sfx ? sfxsContainer : musicsContainer;
                s.source = container.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }
        }

        //!-- Core Functionality
        public void PlayAudio(Musics music)
        {
            Sound sound = Array.Find(Musics, sound => sound.name == music.ToString());
            if (sound == null)
            {
                Debug.LogWarning($"Bgm: {music} not found!");
                return;
            }

            if (sound.sfx)
            {
                sound.source.PlayOneShot(sound.clip);
            }
            else
            {
                sound.source.Play();
                LatestMusic = music;
            }
        }


        public void StopAudio(Musics music)
        { 
            Sound sound = Array.Find(Musics, sound => sound.name == music.ToString());

            sound.source.Stop();
        }
        
        public void PauseAudio(Musics music)
        {
            Sound sound = Array.Find(Musics, sound => sound.name == music.ToString());
        
            sound.source.Pause();
        }
        
        public void SetVolume(Musics music, float value)
        {
            Sound sound = Array.Find(Musics, sound => sound.name == music.ToString());
        
            sound.source.volume = value;
        }
        
        public float GetVolume(Musics music)
        {
            Sound sound = Array.Find(Musics, sound => sound.name == music.ToString());
        
            return sound.volume;
        }

        public bool IsAudioPlaying(Musics music)
        {
            Sound sound = Array.Find(Musics, sound => sound.name == music.ToString());
            return sound.source.isPlaying;
        }
        
        #endregion
    }
}