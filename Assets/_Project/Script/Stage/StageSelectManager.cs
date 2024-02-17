using System;
using System.Collections;
using System.Collections.Generic;
using DanielLochner.Assets.SimpleScrollSnap;
using LabirinKata.Database;
using LabirinKata.Enum;
using LabirinKata.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace LabirinKata.Stage
{
    public class StageSelectManager : MonoBehaviour
    {
        #region Fields & Property

        [Header("UI")]
        [SerializeField] private GameObject selectStagePanelUI;
        [SerializeField] private GameObject stageContentObject;

        [Space]
        [SerializeField] private Button exploreButtonUI;
        [SerializeField] private Button closeButtonUI;

        private int _currentPanelIndex;
        private List<GameObject> _stagePanelObjects;

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
            InitializeStagePanels();
            exploreButtonUI.onClick.AddListener(OnExploreButton);
            closeButtonUI.onClick.AddListener(OnCloseButton);

            _currentPanelIndex = 0;
            selectStagePanelUI.SetActive(false);
        }

        private void InitializeStagePanels()
        {
            _stagePanelObjects = new List<GameObject>();
            var contentCount = stageContentObject.transform.childCount;

            for (var i = 0; i < contentCount; i++)
            {
               var panel = stageContentObject.transform.GetChild(i).gameObject;
                _stagePanelObjects.Add(panel);
            }
        }

        // !-- Core Functionality
        private void SetIndexPanel()
        {
            _currentPanelIndex = simpleScrollSnap.SelectedPanelIndex;
            var stageName = GetStageSceneName(_currentPanelIndex);
            exploreButtonUI.interactable = _currentPanelIndex is 0 || GameDatabase.Instance.LoadLevelConditions(stageName);
        }

        private void OnExploreButton()
        {
            SceneTransitionManager.Instance.LoadSelectedLevel(_currentPanelIndex);
        }

        private void OnCloseButton()
        {
            selectStagePanelUI.SetActive(false);
            ReSetupScrollSnap();
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
                0 => LevelList.Level_01.ToString(),
                1 => LevelList.Level_02.ToString(),
                2 => LevelList.Level_03.ToString(),
                _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
            };
        }

        #endregion

    }
}
