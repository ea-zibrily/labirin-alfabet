using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DanielLochner.Assets.SimpleScrollSnap;
using LabirinKata.Enum;
using LabirinKata.Managers;
using LabirinKata.Database;

namespace LabirinKata.Stage
{
    public class StageSelectManager : MonoBehaviour
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
        [SerializeField] private StageContent[] stageContents;

        private int StageCount
        {
            get => System.Enum.GetNames(typeof(Level)).Length;
        }

        [Space]
        [SerializeField] private Button exploreButtonUI;
        [SerializeField] private Button closeButtonUI;

        private int _currentPanelIndex;

        [Header("Reference")]
        [SerializeField] private SimpleScrollSnap simpleScrollSnap;

        #endregion

        #region MonoBehaviour Callbacks

        private void OnEnable()
        {
            simpleScrollSnap.OnSnappingBegin += SetIndexPanel;
        }

        private void OnDisable()
        {
            simpleScrollSnap.OnSnappingBegin -= SetIndexPanel;
        }

        private void Start()
        {
            InitializeSelectStage();
        }

        #endregion

        #region Labirin Kata Callbacks
        
        // !-- Initialization
        private void InitializeSelectStage()
        {
            if (stageContents.Length != StageCount)
            {
                Debug.LogError("stage content kurang brok!");
                return;
            }

            exploreButtonUI.onClick.AddListener(OnExploreButton);
            closeButtonUI.onClick.AddListener(OnCloseButton);

            _currentPanelIndex = 0;
            selectStagePanelUI.SetActive(false);
        }

        // !-- Core Functionality
        private void SetIndexPanel()
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

            exploreButtonUI.interactable = isLevelUnlocked;
        }
        
        private void OnExploreButton()
        {
            if (_currentPanelIndex > 0)
            {
                Debug.LogWarning("level 2-3 under development brok!");
                return;
            }

            var levelSceneIndex = _currentPanelIndex + 1;
            SceneTransitionManager.Instance.LoadSelectedLevel(levelSceneIndex);
        }

        private void OnCloseButton()
        {
            selectStagePanelUI.SetActive(false);
            ReSetupScrollSnap();
            SetIndexPanel();
        }

        private void ReSetupScrollSnap()
        {
            if (simpleScrollSnap.ValidConfig)
            {
                simpleScrollSnap.Setup();
            }
            else
            {
                throw new Exception("Invalid configuration.");
            }
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
