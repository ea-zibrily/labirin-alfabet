using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Alphabet.Data;
using Alphabet.Enum;
using Alphabet.Stage;
using Alphabet.Letter;
using Alphabet.Managers;
using DanielLochner.Assets.SimpleScrollSnap;

namespace Alphabet.Mission
{
    public class MissionManager : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Mission")]
        [SerializeField] private GameObject missionPanelUI;
        [SerializeField] private GameObject[] missionObjectivesUI;
        [SerializeField] private string[] missionTextUI;
        [SerializeField] private Button missionButtonUI;

        private bool _isTutorialStage;
        private bool _canPlay;
        private List<LetterData> _letterDatas;

        [Header("Reference")]
        [SerializeField] private LetterPooler letterPooler;
        [SerializeField] private MissionAnimation missionAnimation;
        [SerializeField] private SimpleScrollSnap scrollSnap;
        private StageMarker _stageMarker;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _stageMarker = GameObject.Find("StageMarker").GetComponent<StageMarker>();
        }

        private void Start()
        {
            missionPanelUI.SetActive(true);
            missionButtonUI.onClick.AddListener(OnCloseMission);
        }

        #endregion

        #region Methods

        // !- Initialize
        private void InitializeTutorial()
        {
            // Datas
            var spawnedDatas = letterPooler.SpawnedLetterDatas;

            _letterDatas ??= new List<LetterData>();
            _letterDatas.Clear();
            _letterDatas.AddRange(spawnedDatas);

            _isTutorialStage = StageManager.Instance.CurrentStage == StageName.Gua_Aksara && 
                StageManager.Instance.CurrentStageNum == StageNum.Stage_1;
            _canPlay = !_isTutorialStage;

            // Other
            foreach (var letter in missionObjectivesUI)
            {
                letter.SetActive(false);
            }

            // Activate
            var buttonText = _canPlay ? missionTextUI[0] : missionTextUI[1];
            missionButtonUI.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
            missionPanelUI.SetActive(true);
        }

        // !- Core
        public void CallTutorial()
        {
            InitializeTutorial();
            MissionHandler();
        }

        private void MissionHandler()
        {
            for (int i = 0; i < _letterDatas.Count; i++)
            {
                var letter = missionObjectivesUI[i];
                var letterImage = letter.GetComponent<Image>();
                letterImage.sprite = _letterDatas[i].LetterSprite;
                letter.SetActive(true);
            }
        }

        private void OnCloseMission()
        {
            FindObjectOfType<AudioManager>().PlayAudio(Musics.ButtonSfx);

            // Scroll tutorial
            if (!_canPlay)
            {
                ScrollTutorial();
                return;
            }

            // Close mission
            missionPanelUI.SetActive(false);
            scrollSnap.Setup();
            if (_isTutorialStage) missionAnimation.ResetAnimation();
            foreach (var letter in missionObjectivesUI)
            {
                if (!letter.activeSelf) continue;
                letter.SetActive(false);
            }
           _stageMarker.ShowNotification();

        }

        private void ScrollTutorial()
        {
            FindObjectOfType<AudioManager>().PlayAudio(Musics.ButtonSfx);

            var currentIndex = scrollSnap.SelectedPanel;
            var numOfPanels = scrollSnap.NumberOfPanels;

            if (currentIndex < numOfPanels - 1)
            {
                scrollSnap.GoToNextPanel();
                _canPlay = currentIndex >= numOfPanels - 2;
                var buttonText = _canPlay? missionTextUI[0] : missionTextUI[1];
                missionButtonUI.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
            }
        }
        
        #endregion
    }   
}