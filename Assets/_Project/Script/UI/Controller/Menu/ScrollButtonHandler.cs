using UnityEngine;
using UnityEngine.UI;
using Alphabet.Enum;
using Alphabet.Managers;

namespace Alphabet.UI
{
    public class ScrollButtonHandler : MonoBehaviour
    {
        [Header("Scroll UI")]
        [SerializeField] private Button nextButtonUI;
        [SerializeField] private Button previousButtonUI;

        private void Start()
        {
            // Init scroll button SFXs
            static void OnButtonClick() => FindObjectOfType<AudioManager>().PlayAudio(Musics.ButtonSfx);
            
            nextButtonUI.onClick.AddListener(OnButtonClick);
            previousButtonUI.onClick.AddListener(OnButtonClick);
        }
    }
}