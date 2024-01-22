using UnityEngine;

namespace LabirinKata.Entities.Item
{
    public class LetterController : MonoBehaviour
    {
        #region Variable

        [Header("Settings")] 
        [SerializeField] private int itemIdNumber;
        [SerializeField] private string letterName;
        
        [Header("Reference")] 
        private LetterManager _letterManager;
        
        #endregion

        #region MonoBehaviour Callbacks
        
        private void Awake()
        {
            _letterManager = GameObject.FindGameObjectWithTag("LetterManager").GetComponent<LetterManager>();
        }
        
        private void Start()
        {
            gameObject.name = letterName;
        }
        
        #endregion

        #region CariHuruf Callbacks

        public void TakeLetter()
        {
            _letterManager.LetterTakenEvent(itemIdNumber);
            gameObject.SetActive(false);
        }

        #endregion
    }
}