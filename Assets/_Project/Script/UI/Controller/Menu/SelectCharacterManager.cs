using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Alphabet.Database;
using System;

namespace Alphabet.UI
{
    public class SelectCharacterManager : MonoBehaviour
    {
        #region Struct
        [Serializable]
        private struct CharacterComponent
        {
            public string Name;
            public GameObject Highlight;
            public Button CharacterButton;
            public Button SelectButton;
        }
        #endregion

        #region Fields & Property

        [Header("UI")]
        [SerializeField] CharacterComponent[] characterComponents;
        [SerializeField] private GameObject selectCharacterPanelUI;
        [SerializeField] private Button closeButtonUI;

        private Dictionary<Button, int> _selectButtonNums;
        private int _characterIndex;
        
        // Const Variable
        private const int MAX_PLAYER_COUNT = 2;

        [Header("Button Tweening")]
        [SerializeField] private float startTweenDuration;
        [SerializeField] private float endTweenDuration;
        [Range(0f, 1.5f)] [SerializeField] private float scalingMultiplier;

        private Vector3 defaultScale;
        
        [Header("Reference")]
        [SerializeField] private SelectStageManager selectStageManager;

        #endregion

        private void Start()
        {
            InitializeMainButton();
            InitializeCharacter();
            SetDefaultCharacter();
        }

        #region Methods

        // !-- Initialization
        private void InitializeMainButton()
        {
            closeButtonUI.onClick.AddListener(OnClickClose);
            foreach (var character in characterComponents)
            {
                character.SelectButton.onClick.AddListener(OnClickExplore);
            }
        }

        private void InitializeCharacter()
        {
            if (characterComponents.Length < MAX_PLAYER_COUNT)
            {
                Debug.LogError("player button kurang lekku");
                return;
            }

            var buttonObject = characterComponents[0].CharacterButton.GetComponent<RectTransform>();
            defaultScale = buttonObject.localScale;
            
            _selectButtonNums = new Dictionary<Button, int>();
            for (var i = 0; i < characterComponents.Length; i++)
            {
                var btn = characterComponents[i].CharacterButton;
                _selectButtonNums[btn] = i;
                btn.onClick.AddListener(() => OnSelectCharacter(btn));
            }
        }

        // !-- Core Functionality
        private void OnClickExplore()
        {
            PlayerDatabase.Instance.SetPlayerData(_characterIndex);
            selectStageManager.GoToStage();
        }

        private void OnClickClose()
        {
            selectCharacterPanelUI.SetActive(false);
            SetDefaultCharacter();
        }
        
        private void OnSelectCharacter(Button btn)
        {
            _characterIndex = _selectButtonNums[btn];
            SelectCharacter(_characterIndex);
        }

        private void SetDefaultCharacter()
        {
            _characterIndex = 0;
            SelectCharacter(_characterIndex);
        }

        private void SelectCharacter(int index)
        {
            if (index > characterComponents.Length)
            {
                Debug.LogError("index kebanaykan bvrok");
                return;
            }

            // Selected
            var selectComponent = characterComponents[index];

            selectComponent.Highlight.transform.GetChild(0).gameObject.SetActive(true);
            TweenScaledButton(selectComponent.CharacterButton.gameObject, isSelect: true);
            selectComponent.SelectButton.gameObject.SetActive(true);

            // Deselected
            var desselectedIndex = index >= 1 ? index - 1 : index + 1;
            var desselectComponent = characterComponents[desselectedIndex];

            desselectComponent.Highlight.transform.GetChild(0).gameObject.SetActive(false);
            TweenScaledButton(desselectComponent.CharacterButton.gameObject, isSelect: false);
            desselectComponent.SelectButton.gameObject.SetActive(false);
        }

        // !-- Helper/Utilities
        private void TweenScaledButton(GameObject target, bool isSelect)
        {
            var incrementVector = new Vector3(scalingMultiplier, scalingMultiplier, scalingMultiplier);
            var upperScale = defaultScale + incrementVector;
            var targetScale = isSelect ? upperScale : defaultScale;

            LeanTween.scale(target, targetScale, startTweenDuration).setEase(LeanTweenType.easeOutBack);
        }

        #endregion
    }
}
