using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DanielLochner.Assets.SimpleScrollSnap;
using Alphabet.Enum;
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
            simpleScrollSnap.OnSnappingBegin += SetPanelIndex;
        }

        private void OnDisable()
        {
            simpleScrollSnap.OnSnappingBegin -= SetPanelIndex;
        }

        #endregion

        #region Methods

        // !-- Initialization
        protected override void InitialiazeOnStart()
        {
            InitializeSelectStage();
            base.InitialiazeOnStart();
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
            if (_currentPanelIndex > 0)
            {
                Debug.LogWarning("level 2-3 under development brok!");
                return;
            }

            selectCharacterPanelUI.SetActive(true);
            ClosePanel();
        }

        protected override void OnClickClose()
        {
            base.OnClickClose();

            ClosePanel();
            SetPanelIndex();
        }

        private void ClosePanel()
        {
            selectStagePanelUI.SetActive(false);
            simpleScrollSnap.Setup();
        }


        private void SetPanelIndex()
        {
            _currentPanelIndex = simpleScrollSnap.SelectedPanelIndex;

            var stageName = GetStageSceneName(_currentPanelIndex);
            var stagePanel = stageContents[_currentPanelIndex].stagePanelObject;
            var stageChildCount = stagePanel.transform.childCount;

            var isLevelUnlocked = GameDatabase.Instance.LoadLevelUnlocked(stageName);

            for (var i = 0; i < stageChildCount - 1; i++)
            {
                var stageChild = stagePanel.transform.GetChild(i);
                stageChild.GetComponent<Image>().color = isLevelUnlocked ? Color.white : Color.grey;

                if (i is 1)
                {
                    stageChild.gameObject.SetActive(!isLevelUnlocked);
                }
            }

            ExploreButtonUI.interactable = isLevelUnlocked;
        }

        // !-- Helper/Utilities
        private string GetStageSceneName(int index)
        {
            return index switch
            {
                0 => Level.Cave.ToString(),
                1 => Level.Forest.ToString(),
                2 => Level.Ruins.ToString(),
                _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
            };
        }

        #endregion

    }
}
