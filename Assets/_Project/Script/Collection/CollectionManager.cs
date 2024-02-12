using System;
using UnityEngine;
using UnityEngine.UI;
using LabirinKata.Database;
using UnityEngine.Serialization;

namespace LabirinKata.Collection
{
    public class CollectionManager : MonoBehaviour
    {
        #region Fields & Properties
        
        [Header("UI")] 
        [SerializeField] private GameObject[] letterObjectUI;
        [SerializeField] private GameObject collectionPanelUI;
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

        // !-- Initialization
        private void InitializeButton()
        {
            closeButtonUI.onClick.AddListener(CloseCollectionPanel);
        }
        
        // !-- Core Functionality
        private void SetUnlockedCollection()
        {
            for (var i = 0; i < letterObjectUI.Length; i++)
            {
                var letterId = i + 1;
                var isLetterUnlock = GameDatabase.Instance.LoadLetterConditions(letterId);

                letterObjectUI[i].GetComponent<Button>().interactable = isLetterUnlock;
                letterObjectUI[i].transform.GetChild(0).gameObject.SetActive(isLetterUnlock);
                Debug.Log($"{letterObjectUI[i]} is {isLetterUnlock}");
            }
        }
        
        private void CloseCollectionPanel()
        {
            collectionPanelUI.SetActive(false);
        }

        #endregion
        
    }
}