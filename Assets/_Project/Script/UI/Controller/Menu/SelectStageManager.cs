using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DanielLochner.Assets.SimpleScrollSnap;
using Alphabet.Enum;
using Alphabet.Stage;
using Alphabet.Managers;
using Alphabet.Database;

namespace Alphabet.UI
{
    public class SelectStageManager : SelectBase
    {
        #region Struct
        [Serializable]
        private struct StageContent
        {
            public Level levelName;
            public GameObject stagePanelObject;
        }
        #endregion

        #region Fields & Property

        [Header("UI")]
        [SerializeField] private GameObject selectStagePanelUI;
        [SerializeField] private GameObject selectCharacterPanelUI;
        [SerializeField] private TextMeshProUGUI stageHeadlineText;
        [SerializeField] private StageContent[] stageContents;
        
        // TODO: Pas udah fix, panel index iki gausa di serializ
        private int _currentPanelIndex;
        private int StageCount =>  System.Enum.GetNames(typeof(Level)).Length;

        [Header("Image Data")]
        [SerializeField] private Color lockedStageColor;
        [SerializeField] private Material imageMaterial;

        [Header("Reference")]
        [SerializeField] private SimpleScrollSnap simpleScrollSnap;
        public SimpleScrollSnap SimpleScrollSnap => simpleScrollSnap;

        #endregion

        #region MonoBehaviour Callbacks

        private void OnEnable()
        {
            simpleScrollSnap.OnSnappingBegin += SetSelectedPanel;
        }

        private void OnDisable()
        {
            simpleScrollSnap.OnSnappingBegin -= SetSelectedPanel;
        }

        #endregion

        #region Methods

        // !-- Initialization
        protected override void InitialiazeOnStart()
        {
            base.InitialiazeOnStart();
            InitializeSelectStage();
            SetSelectedPanel();
        }

        private void InitializeSelectStage()
        {
            if (stageContents.Length != StageCount)
            {
                Debug.LogError("stage content kurang brok!");
                return;
            }

            _currentPanelIndex = 0;
            selectStagePanelUI.SetActive(false);
        }

        // !-- Core Functionality
        public void GoToStage()
        {
            var levelSceneIndex = _currentPanelIndex + 1;
            Debug.Log($"explore level {levelSceneIndex}");
            SceneTransitionManager.Instance.LoadSelectedLevel(levelSceneIndex);
        }

        protected override void OnClickExplore()
        {
            base.OnClickExplore();
            if (_currentPanelIndex > 1)
            {
                Debug.LogWarning("ruins stage under development brok!");
                return;
            }
            
            selectCharacterPanelUI.SetActive(true);
            ClosePanel();
        }

        protected override void OnClickClose()
        {
            base.OnClickClose();

            ClosePanel();
            SetSelectedPanel();
        }

        private void ClosePanel()
        {
            selectStagePanelUI.SetActive(false);
            simpleScrollSnap.Setup();
        }

        private void SetSelectedPanel()
        {
            _currentPanelIndex = simpleScrollSnap.SelectedPanelIndex;
            
            var stageName = StageHelper.GetStageStringValue(_currentPanelIndex);
            var stagePanel = stageContents[_currentPanelIndex].stagePanelObject;
            var isLevelUnlocked = IsLevelUnlocked(_currentPanelIndex);
            var isActivateButton = isLevelUnlocked || _currentPanelIndex is 0;
            var isAnimate = GameDatabase.Instance.LoadLevelClearIndex() <= _currentPanelIndex 
                        && GameDatabase.Instance.IsAnimateUnlock;

            SetHeadlineText(stageName.ToUpper());
            SetExploreButtonState(isActivateButton, isAnimate, ExploreButtonUI);
            Debug.LogWarning(isLevelUnlocked);
            
            if (_currentPanelIndex <= 0 || GameDatabase.Instance.IsAnimateUnlock) return;
            SetStageThumbnailState(isLevelUnlocked, stagePanel);   
        }

        private void SetExploreButtonState(bool isUnlocked, bool isAnimate, Button button)
        {
            var buttonImage = button.GetComponent<Image>();
            var canvasGroup = button.GetComponent<CanvasGroup>();

            if (isAnimate)
            {
                ExploreButtonUI.interactable = false;
                canvasGroup.enabled = false;
                buttonImage.material = imageMaterial;
            }
            else
            {
                ExploreButtonUI.interactable = isUnlocked;
                canvasGroup.enabled = !isUnlocked;
                buttonImage.material = isUnlocked ? null : imageMaterial;
            }
        }

        private void SetStageThumbnailState(bool isUnlocked, GameObject stagePanel)
        {
            var stageThumbnail = stagePanel.transform.GetChild(0).GetComponent<Image>();
            var stagePadlock = stagePanel.transform.GetChild(1).gameObject;

            stageThumbnail.color = isUnlocked ? Color.white : lockedStageColor;
            stageThumbnail.material = isUnlocked ? null : imageMaterial;
            stagePadlock.SetActive(!isUnlocked);
        }
        
        // !-- Helper/Utilities
        private void SetHeadlineText(string text) => stageHeadlineText.text = text;
        private bool IsLevelUnlocked(int index)
        {
            var stageIndex = index > 0 ? index - 1 : index;
            var stageName = StageHelper.GetStageDataKey(stageIndex);

            return GameDatabase.Instance.LoadLevelConditions(stageName);
        }

        #endregion

    }
}
