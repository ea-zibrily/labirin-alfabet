using System.Collections;
using System.Collections.Generic;
using Alphabet.Data;
using Alphabet.Database;
using Alphabet.Gameplay.EventHandler;
using Alphabet.Item;
using Alphabet.Stage;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace Alphabet.Gameplay.Controller
{
    public class TutorialController : MonoBehaviour
    {
        #region Fields & Properties

        [Header("UI")]
        [SerializeField] private GameObject tutorialPanelUI;
        [SerializeField] private GameObject[] letterObjectivesUI;
        [SerializeField] private Button closeButtonUI;

        private List<LetterData> _letterDatas;

        [Header("References")]
        [SerializeField] private LetterPooler letterPooler;
        [SerializeField] private SkeletonGraphic skeletonGraphic;
        private StageMarker _stageMarker;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _stageMarker = GameObject.Find("StageMarker").GetComponent<StageMarker>();
        }

        private void Start()
        {
            closeButtonUI.onClick.AddListener(OnCloseTutorial);
        }

        #endregion

        #region Methods

        // !-- Initialization
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
            tutorialPanelUI.SetActive(true);
        }

        private void InitializeIcon()
        {
           var playerData = PlayerDatabase.Instance.GetPlayerDatabySelected();
           ChangeIconSkin(playerData.PlayerSkin);
        }

        // !-- Core Functionality
        public void CallTutorial()
        {
            InitializeTutorial();
            MissionHandler();
        }

        private void MissionHandler()
        {
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

        private void OnCloseTutorial()
        {
            tutorialPanelUI.SetActive(false);
            foreach (var letter in letterObjectivesUI)
            {
                if (!letter.activeSelf) continue;
                letter.SetActive(false);
            }
           _stageMarker.ShowNotification();
        }

        private void ChangeIconSkin(string skin)
        {
            skeletonGraphic.Skeleton.SetSkin(skin);
            skeletonGraphic.Skeleton.SetSlotsToSetupPose();
            skeletonGraphic.LateUpdate();
        }

        #endregion
    }
}
