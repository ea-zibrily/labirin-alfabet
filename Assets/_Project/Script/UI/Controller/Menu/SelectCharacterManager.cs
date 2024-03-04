using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Alphabet.Database;

namespace Alphabet.UI
{
    public class SelectCharacterManager : SelectBase
    {
        #region Fields & Property

        [Header("UI")]
        [SerializeField] private GameObject selectCharacterPanelUI;
        [SerializeField] private Button[] playerButtonUI;
        [SerializeField] private GameObject highlightObjectUI;

        private Dictionary<Button, int> _selectButtonNums;
        private int _characterIndex;

        // Const Variable
        private const int MAX_PLAYER_COUNT = 2;

        [Header("Reference")]
        [SerializeField] private SelectStageManager selectStageManager;


        #endregion

        #region Methods

        // !-- Initialization
        protected override void InitialiazeOnStart()
        {
            base.InitialiazeOnStart();

            InitializeButton();
            highlightObjectUI.SetActive(false);
        }

        private void InitializeButton()
        {
            if (playerButtonUI.Length < MAX_PLAYER_COUNT)
            {
                Debug.LogError("player button kurang lekku");
                return;
            }
            
            _selectButtonNums = new Dictionary<Button, int>();
            for (var i = 0; i < playerButtonUI.Length; i++)
            {
                Button btn = playerButtonUI[i];
                _selectButtonNums[btn] = i;
                btn.onClick.AddListener(() => OnSelectPlayer(btn));
            }
        }

        // !-- Core Functionality
        protected override void OnClickExplore()
        {
            base.OnClickExplore();
            PlayerDatabase.Instance.SetPlayerData(_characterIndex);
            selectStageManager.GoToStage();
        }

        protected override void OnClickClose()
        {
            base.OnClickClose();
            selectCharacterPanelUI.SetActive(false);
            highlightObjectUI.SetActive(false);
            _characterIndex = 0;
        }

        private void OnSelectPlayer(Button btn)
        {
            _characterIndex = _selectButtonNums[btn];

            if (!highlightObjectUI.activeSelf)
            {
                highlightObjectUI.SetActive(true);
            }
            highlightObjectUI.GetComponent<RectTransform>().position = playerButtonUI[_characterIndex].transform.position;
        }


        #endregion
    }
}
