using System;
using KevinCastejon.MoreAttributes;
using LabirinKata.Entities.Item;
using LabirinKata.Gameplay.EventHandler;
using UnityEngine;

namespace LabirinKata.Entities.Player
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerManager : MonoBehaviour
    {
        #region Variable
        
        [Header("Health")] 
        [SerializeField] [ReadOnlyOnPlay] private int currentHealthCount;
        [SerializeField] private GameObject[] healthUI;

        private bool _isPlayerDead;

        [Header("Reference")] 
        private PlayerController _playerController;
        
        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }

        private void Start()
        {
            var maxHealthCount = healthUI.Length;
            currentHealthCount = maxHealthCount - 1;
            
            _isPlayerDead = false;
        }

        #endregion

        #region CariHuruf Callbacks
        
        private void DecreaseHealth()
        {
            healthUI[currentHealthCount].gameObject.SetActive(false);
            currentHealthCount--;
            
            if (currentHealthCount < 0)
            {
                currentHealthCount = 0;
                _isPlayerDead = true;
                Debug.LogWarning("hp habis boszz");
                GameEventHandler.GameOverEvent();
            }
        }
        
        #endregion
        
        #region Collider Callbacks
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isPlayerDead || !_playerController.CanMove) return;
            
            if (other.CompareTag("Enemy"))
            {
                DecreaseHealth();
            }
            else if (other.CompareTag("Item"))
            {
                if (!other.TryGetComponent<LetterController>(out var letter)) return;
                letter.TakeLetter();
            }
        }

        #endregion
    }
}