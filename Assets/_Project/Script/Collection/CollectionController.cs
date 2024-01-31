using System;
using UnityEngine;
using UnityEngine.UI;

namespace LabirinKata.Collection
{
    public class CollectionController : MonoBehaviour
    {
        #region Variable

        [Header("Settings")] 
        private Button _buttonUI;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _buttonUI = GetComponent<Button>();
        }

        private void Start()
        {
            InitializeButton();
        }

        #endregion

        #region Labirin Kata Callbacks

        //-- Initialization
        private void InitializeButton()
        {
            _buttonUI.onClick.AddListener(ClickObject);
        }
        
        //-- Core Functionality
        /*
         * TODO: bikin logic buat efek letter jika ditekan
         * Efek list:
         * 1. Animasi scaling
         * 2. Suara VO sesuai letter
         */
        private void ClickObject()
        {
            //-- Drop logic here
        }

        #endregion
        
    }
}