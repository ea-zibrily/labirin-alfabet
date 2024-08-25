using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using Alphabet.Enum;
using Alphabet.Managers;
using Alphabet.Database;

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
        [SerializeField] private Color unselectedColor;

        private Dictionary<Button, int> _selectButtonNums;
        private int _characterIndex;
        
        // Const Variable
        private const int MAX_PLAYER_COUNT = 2;

        [Header("Button Tweening")]
        [SerializeField] private float startTweenDuration;
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

        // !- Initialize
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

        // !- Core Functionality
        private void OnClickExplore()
        {
            AudioManager.Instance.PlayAudio(Musics.ButtonSfx);
            PlayerDatabase.Instance.SetPlayerData(_characterIndex);
            selectStageManager.GoToStage();
        }

        private void OnClickClose()
        {
            AudioManager.Instance.PlayAudio(Musics.ButtonSfx);
            selectCharacterPanelUI.SetActive(false);
            SetDefaultCharacter();
        }
        
        private void OnSelectCharacter(Button btn)
        {
            AudioManager.Instance.PlayAudio(Musics.ButtonSfx);
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

            // Set Selected and Unselected
            SetSelection(index, true);
            SetSelection(GetUnselectedIndex(index), false);
        }

        private void SetSelection(int index, bool isSelected)
        {
            var component = characterComponents[index];
            if (isSelected)
            {
                SetSelectCharacter(component);
            }
            else
            {
                SetUnselectCharacter(component);
            }
        }

        private void SetSelectCharacter(CharacterComponent component)
        {
            var selectIcon = component.CharacterButton.transform.GetChild(0);

            component.Highlight.transform.GetChild(0).gameObject.SetActive(true);
            TweenScaledButton(component.CharacterButton.gameObject, true);
            component.SelectButton.gameObject.SetActive(true);
            
            if (!selectIcon.TryGetComponent(out SkeletonGraphic graphic)) return;
            graphic.color = Color.white;
            graphic.AnimationState.SetAnimation(0, "QF_walk", true).TimeScale = 0.8f;
        }

        private void SetUnselectCharacter(CharacterComponent component)
        {
            var unselectIcon = component.CharacterButton.transform.GetChild(0);

            component.Highlight.transform.GetChild(0).gameObject.SetActive(false);
            TweenScaledButton(component.CharacterButton.gameObject, false);
            component.SelectButton.gameObject.SetActive(false);

            if (!unselectIcon.TryGetComponent(out SkeletonGraphic graphic)) return;
            graphic.color = unselectedColor;
            graphic.AnimationState.SetAnimation(0, "QF_idle", true);
        }

        // !- Helper
        private void TweenScaledButton(GameObject target, bool isSelect)
        {
            var incrementVector = new Vector3(scalingMultiplier, scalingMultiplier, scalingMultiplier);
            var upperScale = defaultScale + incrementVector;
            var targetScale = isSelect ? upperScale : defaultScale;

            LeanTween.scale(target, targetScale, startTweenDuration).setEase(LeanTweenType.easeOutBack);
        }
        
        private int GetUnselectedIndex(int index)
        {
            return index >= 1 ? index - 1 : index + 1;
        }

        #endregion
    }
}
