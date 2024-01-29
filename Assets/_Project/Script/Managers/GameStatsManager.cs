using System;
using UnityEngine;
using TMPro;
using LabirinKata.Item.Letter;
using LabirinKata.Entities.Player;

namespace LabirinKata.Managers
{
    public class GameStatsManager : MonoBehaviour
    {
        #region Variable

        [Header("UI")] 
        [SerializeField] private TextMeshProUGUI speedTextUI;
        [SerializeField] private TextMeshProUGUI healthTextUI;
        [SerializeField] private TextMeshProUGUI objectiveTextUI;
        
        [Header("Reference")] 
        [SerializeField] private PlayerController playerController;
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private LetterUIManager letterUIManager;
        
        #endregion
        
        #region MonoBehaviour Callbacks
        
        private void Update()
        {
            speedTextUI.text = "Speed: " + playerController.CurrentMoveSpeed;
            healthTextUI.text = "Health: " + playerManager.CurrentHealthCount;
            objectiveTextUI.text = "Objective: " + letterUIManager.CurrentTakenLetter;
        }
        
        #endregion
    }
}