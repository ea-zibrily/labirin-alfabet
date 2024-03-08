using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Alphabet.Database;

namespace Alphabet.UI
{
    public class SelectCharacterManager : SelectBase
    {
        #region Fields & Property

        [Header("UI")]
        [SerializeField] private GameObject selectCharacterPanelUI;
        [SerializeField] private Button[] playerButtonUI;
        [SerializeField] private GameObject highlightObjectUI;

        private Dictionary<Button, int> _selectButtonNums;
        private int _characterIndex;
        
        // Const Variable
        private const int MAX_PLAYER_COUNT = 2;

        [Header("Button Tweening")]
        [SerializeField] private float startTweenDuration;
        [SerializeField] private float endTweenDuration;
        [Range(0f, 1.5f)] [SerializeField] private float scalingMultiplier;

        private Vector3 ButtonDefaultScale 
        {
            get => playerButtonUI[0].GetComponent<RectTransform>().localScale;
        }
        
        [Header("Reference")]
        [SerializeField] private SelectStageManager selectStageManager;


        #endregion

        #region Methods

        // !-- Initialization
        protected override void InitialiazeOnStart()
        {
            base.InitialiazeOnStart();

            InitializeButton();
            SetDefaultCharacter();
        }

        private void InitializeButton()
        {
            if (playerButtonUI.Length < MAX_PLAYER_COUNT)
            {
                Debug.LogError("player button kurang lekku");
                return;
            }
            
            _selectButtonNums = new Dictionary<Button, int>();
            for (var i = 0; i < playerButtonUI.Length; i++)
            {
                Button btn = playerButtonUI[i];
                _selectButtonNums[btn] = i;
                btn.onClick.AddListener(() => OnSelectCharacter(btn));
            }
        }

        // !-- Core Functionality
        protected override void OnClickExplore()
        {
            base.OnClickExplore();
            PlayerDatabase.Instance.SetPlayerData(_characterIndex);
            selectStageManager.GoToStage();
        }

        protected override void OnClickClose()
        {
            base.OnClickClose();
            selectCharacterPanelUI.SetActive(false);
            SetDefaultCharacter();
        }

        private void OnSelectCharacter(Button btn)
        {
            TweenScaledButton(btn.gameObject);
            _characterIndex = _selectButtonNums[btn];

            if (!highlightObjectUI.activeSelf)
            {
                highlightObjectUI.SetActive(true);
            }
            highlightObjectUI.GetComponent<RectTransform>().position = playerButtonUI[_characterIndex].transform.position;
        }

        private void SetDefaultCharacter()
        {
            _characterIndex = 0;
            highlightObjectUI.GetComponent<RectTransform>().position = playerButtonUI[_characterIndex].transform.position;
        }

        // !-- Helper/Utilities
        private void TweenScaledButton(GameObject target)
        {
            var incrementVector = new Vector3(scalingMultiplier, scalingMultiplier, scalingMultiplier);
            var targetScale = ButtonDefaultScale + incrementVector;

            LeanTween.scale(target, targetScale, startTweenDuration).setEase(LeanTweenType.easeOutBack);
            LeanTween.scale(target, ButtonDefaultScale, endTweenDuration).setDelay(startTweenDuration).setEase(LeanTweenType.easeOutBack);
        }

        #endregion
    }
}
