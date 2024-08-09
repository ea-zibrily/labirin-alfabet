using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DanielLochner.Assets.SimpleScrollSnap;
using Alphabet.Enum;
using Alphabet.Stage;
using Alphabet.Managers;
using Alphabet.Database;
using Alphabet.Gameplay.EventHandler;

namespace Alphabet.UI
{
    public class SelectStageManager : SelectBase
    {
        #region Struct
        [Serializable]
        private struct StageContent
        {
            public StageName levelName;
            public GameObject stagePanelUI;
        }
        #endregion

        #region Fields & Property

        [Header("UI")]
        [SerializeField] private GameObject selectStagePanelUI;
        [SerializeField] private GameObject selectCharacterPanelUI;
        [SerializeField] private TextMeshProUGUI stageHeadlineText;
        [SerializeField] private StageContent[] stageContents;
        
        private int _currentPanelIndex;
        private bool _canExplore;
        private const string PADLOCK_TRIGGERED = "LockTap";

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
            // Snapping
            simpleScrollSnap.OnSnappingBegin += SetSelectedPanel;

            // Padlock
            PadlockEventHandler.OnPadlockAnimate += value => ExploreButtonUI.interactable = !value;
        }

        private void OnDisable()
        {
            // Snapping
            simpleScrollSnap.OnSnappingBegin -= SetSelectedPanel;

            // Padlock
            PadlockEventHandler.OnPadlockAnimate -= value => ExploreButtonUI.interactable = !value;
        }
        
        #endregion

        #region Methods

        // !- Initialize
        protected override void InitOnStart()
        {
            base.InitOnStart();
            InitSelectStage();
            SetSelectedPanel();
        }

        private void InitSelectStage()
        {
            if (stageContents.Length != GetStageCount())
            {
                Debug.LogError("stage content kurang brok!");
                return;
            }

            _currentPanelIndex = 0;
            selectStagePanelUI.SetActive(false);
        }

        // !- Core
        public void GoToStage()
        {
            var levelSceneIndex = _currentPanelIndex + 1;
            SceneTransitionManager.Instance.LoadSelectedLevel(levelSceneIndex);
        }

        protected override void OnClickExplore()
        {
            base.OnClickExplore();

            if (_canExplore)
            {
                FindObjectOfType<AudioManager>().PlayAudio(Musics.ButtonSfx);
                selectCharacterPanelUI.SetActive(true);
                ClosePanel();
            }
            else
            {
                var stagePanel = stageContents[_currentPanelIndex].stagePanelUI;
                var stagePadlock = stagePanel.transform.GetChild(1).GetComponent<Animator>();

                stagePadlock.SetTrigger(PADLOCK_TRIGGERED);
                FindObjectOfType<AudioManager>().PlayAudio(Musics.LockedStageSfx);
            }
           
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

            var isLevelCleared = IsLevelUnlocked(_currentPanelIndex);
            var stageName = StageHelper.GetStageNameByInt(_currentPanelIndex);
            
            _canExplore = isLevelCleared || _currentPanelIndex is 0;

            SetHeadlineText(stageName.ToUpper());
            SetExploreButtonState(_canExplore, ExploreButtonUI);
            SetThumbnailState(isLevelCleared, stageContents[_currentPanelIndex].stagePanelUI);   
        }

        private void SetHeadlineText(string value)
        {
            stageHeadlineText.text = value;
        }

        private void SetExploreButtonState(bool isUnlocked, Button button)
        {
            var buttonImage = button.GetComponent<Image>();
            var canvasGroup = button.GetComponent<CanvasGroup>();

            canvasGroup.enabled = !isUnlocked;
            buttonImage.material = isUnlocked ? null : imageMaterial;
        }

        private void SetThumbnailState(bool isUnlocked, GameObject stagePanel)
        {
            if (_currentPanelIndex < 1) return;

            var stageThumbnail = stagePanel.transform.GetChild(0).GetComponent<Image>();
            var stagePadlock = stagePanel.transform.GetChild(1).gameObject;

            stageThumbnail.color = isUnlocked ? Color.white : lockedStageColor;
            stageThumbnail.material = isUnlocked ? null : imageMaterial;
            stagePadlock.SetActive(!isUnlocked);
        }
        
        // !- Helper
        private bool IsLevelUnlocked(int index)
        {
            var stageIndex = index > 0 ? index - 1 : index;
            var stageName = StageHelper.GetStageDataKey(stageIndex);

            return GameDatabase.Instance.LoadLevelConditions(stageName);
        }

        private int GetStageCount() => System.Enum.GetNames(typeof(StageName)).Length;

        #endregion

    }
}
