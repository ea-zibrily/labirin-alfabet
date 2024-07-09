using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DanielLochner.Assets.SimpleScrollSnap;
using Alphabet.Data;
using Alphabet.Enum;
using Alphabet.Stage;
using Alphabet.Database;

namespace Alphabet.Mission
{
    public class TutorialManager : Mission
    {
        #region Internal Fields

        [Header("Tutorial")]
        [SerializeField] private GameObject missionPanelUI;
        [SerializeField] private GameObject tutorialPanelUI;
        [SerializeField] private SimpleScrollSnap simpleScrollSnap;
        [SerializeField] private GameObject[] letterObjectivesUI;

        private bool _isTutorialStage;
        private List<LetterData> _letterDatas;

        #endregion

        #region Methods

        // !- Initialize
        protected override void InitOnStart()
        {
            base.InitOnStart();
            _isTutorialStage = StageManager.Instance.CurrentStage == StageName.Gua_Aksara && 
                StageManager.Instance.CurrentStageNum == StageNum.Stage_1;
        }

        private void InitializeTutorial()
        {
            // Datas
            var spawnedDatas = letterPooler.SpawnedLetterDatas;
            var playerData = PlayerDatabase.Instance.GetPlayerDatabySelected();

            _letterDatas ??= new List<LetterData>();
            _letterDatas.Clear();
            _letterDatas.AddRange(spawnedDatas);
            ChangeIconSkin(playerData.PlayerSkin);
            
            // Other
            foreach (var letter in letterObjectivesUI)
            {
                letter.SetActive(false);
            }
            if (_isTutorialStage)
            {
                tutorialPanelUI.SetActive(true);
            }
            else
            {
                missionPanelUI.SetActive(true);
            }
        }

        // !- Core
        public override void CallTutorial()
        {
            base.CallTutorial();
            InitializeTutorial();
            MissionHandler();
        }

        protected override void MissionHandler()
        {
            base.MissionHandler();
            var j = 0;
            foreach (var letter in letterObjectivesUI)
            {
                if (j > _letterDatas.Count - 1) break;

                var letterImage = letter.GetComponent<Image>();
                letterImage.sprite = _letterDatas[j].LetterSprite;
                letter.SetActive(true);
                j++;
            }
        }

        protected override void OnCloseMission()
        {
            base.OnCloseMission();
            if (_isTutorialStage)
                CloseTutorial();
            else
                CloseMission();
        }

        private void CloseMission()
        {
            missionPanelUI.SetActive(false);
            foreach (var letter in letterObjectivesUI)
            {
                if (!letter.activeSelf) continue;
                letter.SetActive(false);
            }
           _stageMarker.ShowNotification();
        }

        private void CloseTutorial()
        {
            var currentIndex = simpleScrollSnap.SelectedPanel;
            var numOfPanels = simpleScrollSnap.NumberOfPanels;

            if (currentIndex < numOfPanels - 1)
            {
                simpleScrollSnap.GoToNextPanel();
            }
            else
            {
                CloseMission();
            }
        }

        #endregion
    }
}