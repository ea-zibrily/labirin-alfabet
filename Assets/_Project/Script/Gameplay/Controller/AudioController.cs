using UnityEngine;
using Alphabet.Enum;
using Alphabet.Managers;

namespace Alphabet.Gameplay.Controller
{
    public class AudioController : MonoBehaviour
    {
        #region Variable

        [SerializeField] private Musics musicName;
        private AudioManager _audioManager;

        #endregion
    
        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }
        
        private void Start()
        {
            var latestBgm = GetLatestBgm();
            _audioManager.StopAudio(latestBgm);
            _audioManager.PlayAudio(musicName);
        }

        private Musics GetLatestBgm()
        {
            return musicName switch
            {
                Musics.MainMenu => Musics.Gameplay,
                Musics.Gameplay => Musics.MainMenu,
                _ => musicName
            };
        }

        #endregion
    }
}
