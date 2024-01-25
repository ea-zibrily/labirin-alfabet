using UnityEngine;
using LabirinKata.Item;

namespace LabirinKata.Entities.Item
{
    public class LetterController : MonoBehaviour, ITakeable
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
            var letterManagementObject = GameObject.FindGameObjectWithTag("LetterManager");
            
            _letterManager = letterManagementObject.GetComponentInChildren<LetterManager>();
            _letterUIManager = letterManagementObject.GetComponentInChildren<LetterUIManager>();
        }
        
        private void Start()
        {
            gameObject.name = letterName;
        }
        
        #endregion
        
        #region Labirin Kata Callbacks
        
        public void Taken()
        {
            _letterUIManager.TakeLetterEvent(LetterId);
            _letterManager.TakeLetterEvent(gameObject);
            
            gameObject.SetActive(false);
        }
        
        #endregion
    }
}