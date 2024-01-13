using UnityEngine;
using CariHuruf.Enum;
using CariHuruf.Entities.Item;

namespace CariHuruf.Gameplay.Controller
{
    public class AudioController : MonoBehaviour
    {
        #region Variable

        [Header("Enum")]
        [SerializeField] private AudioList playSoundEnum;
    
        [Header("Reference")]
        private AudioManager _audioManager;
    
        #endregion
    
        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        }
        
        private void Start()
        {
            _audioManager.PlayAudio(playSoundEnum);
        }

        #endregion
    }
}
