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
            if (_audioManager.LatestMusic != Musics.none)
            {
                _audioManager.StopAudio(_audioManager.LatestMusic);
            }

            _audioManager.PlayAudio(musicName);
        }

        #endregion
    }
}
