using LabirinKata.Item;
using UnityEngine;
using UnityEngine.Serialization;

namespace LabirinKata.Entities.Item
{
    public class LetterController : MonoBehaviour, IInteractable
    {
        #region Variable

        [Header("Settings")] 
        [SerializeField] private int letterId;
        [SerializeField] private string letterName;

        public int LetterId
        {
            get => letterId;
            set => letterId = value;
        }
        public string LetterName => letterName;

        [Header("Reference")] 
        private LetterManager _letterManager;
        private LetterUIManager _letterUIManager;
        
        #endregion

        #region MonoBehaviour Callbacks
        
        private void Awake()
        {
            _letterManager = GameObject.Find("LetterManager").GetComponent<LetterManager>();
            _letterUIManager = GameObject.Find("LetterUIManager").GetComponent<LetterUIManager>();
        }
        
        private void Start()
        {
            gameObject.name = letterName;
        }
        
        #endregion
        
        #region Labirin Kata Callbacks

        public void Taken()
        {
            _letterUIManager.LetterTakenEvent(letterId);
            _letterManager.AddUnlockLetter(gameObject);
            
            gameObject.SetActive(false);
        }
        
        #endregion
    }
}