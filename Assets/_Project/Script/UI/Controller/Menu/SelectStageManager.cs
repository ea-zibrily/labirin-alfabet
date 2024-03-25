using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DanielLochner.Assets.SimpleScrollSnap;
using Alphabet.Enum;
using Alphabet.Managers;
using Alphabet.Database;
using TMPro;
using Alphabet.Stage;

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
        [SerializeField] private int _currentPanelIndex;
        private int StageCount =>  System.Enum.GetNames(typeof(Level)).Length;

        [Header("Reference")]
        [SerializeField] private SimpleScrollSnap simpleScrollSnap;

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

            var stageName = StageHelper.GetStageStringDataValue(_currentPanelIndex);
            var stagePanel = stageContents[_currentPanelIndex].stagePanelObject;
            var stageChildCount = stagePanel.transform.childCount;

            var isLevelUnlocked = IsLevelUnlocked(_currentPanelIndex);

            for (var i = 0; i < stageChildCount - 1; i++)
            {
                if (_currentPanelIndex is 0) break;
                
                var stageChild = stagePanel.transform.GetChild(i);
                stageChild.GetComponent<Image>().color = isLevelUnlocked ? Color.white : Color.grey;

                if (i is 1)
                {
                    stageChild.gameObject.SetActive(!isLevelUnlocked);
                }
            }

            ExploreButtonUI.interactable = isLevelUnlocked;
            stageHeadlineText.text = stageName.ToString().ToUpper();
        }

        // !-- Helper/Utilities
        private bool IsLevelUnlocked(int index)
        {
            var stageIndex = index > 0 ? index - 1 : index;
            var stageName = StageHelper.GetStageStringDataValue(stageIndex);

            return GameDatabase.Instance.LoadLevelConditions(stageName);
        }

        #endregion

    }
}
