using UnityEngine;
using UnityEngine.UI;
using LabirinKata.Database;

namespace LabirinKata.Collection
{
    public class CollectionManager : MonoBehaviour
    {
        #region Variable
        
        [Header("UI")] 
        [SerializeField] private GameObject[] letterImageUI;
        [SerializeField] private Button closeButtonUI;
        
        #endregion

        #region MonoBehaviour Callbacks
        
        private void Start()
        {
            InitializeButton();
            SetUnlockedCollection();
        }
        
        #endregion

        #region Labirin Kata Callbacks

        //-- Initialization
        private void InitializeButton()
        {
            closeButtonUI.onClick.AddListener(CloseCollectionPanel);
        }
        
        //-- Core Functionality
        private void SetUnlockedCollection()
        {
            for (var i = 0; i < letterImageUI.Length; i++)
            {
                var letterId = i + 1;
                var isLetterUnlock = GameDatabase.Instance.LoadLetterConditions(letterId);
                
                letterImageUI[i].SetActive(isLetterUnlock);
                Debug.Log($"{letterImageUI[i]} is {isLetterUnlock}");
            }
        }
        
        private void CloseCollectionPanel()
        {
            gameObject.SetActive(false);
        }

        #endregion
        
    }
}