using System;
using UnityEngine;
using Alphabet.Enum;
using Tsukuyomi.Utilities;
using Alphabet.Pattern.Singleton;

namespace Alphabet.Managers
{
    public class AudioManager : MonoDDOL<AudioManager>
    {
        #region Fields & Properties
        
        [Header("Data")]
        public Sound[] Musics;

        [Header("Container")]
        [SerializeField] private GameObject musicsContainer;
        [SerializeField] private GameObject sfxsContainer;
        
        #endregion

        #region MonoBehaviour Callbacks

        protected override void Awake()
        {
            base.Awake();
            InitializeAudio();
        }
        
        #endregion

        #region Methods
        
        // !- Initialize
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

        // !- Core
        public void PlayAudio(Musics music)
        {
            Sound sound = Array.Find(Musics, sound => sound.name == music.ToString());
            if (sound == null)
            {
                Debug.LogWarning($"Bgm: {music} not found!");
                return;
            }

            if (sound.sfx)
                sound.source.PlayOneShot(sound.clip);
            else
                sound.source.Play();
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

        // !- Helper
        public Sound GetAudio(Musics music)
        {
            Sound sound = Array.Find(Musics, sound => sound.name == music.ToString());
            if (sound == null)
            {
                Debug.LogWarning($"Bgm: {music} not found!");
                return null;
            }
            return sound;
        }

        public bool IsAudioPlaying(Musics music)
        {
            Sound sound = Array.Find(Musics, sound => sound.name == music.ToString());
            return sound.source.isPlaying;
        }
        
        #endregion

    }
}