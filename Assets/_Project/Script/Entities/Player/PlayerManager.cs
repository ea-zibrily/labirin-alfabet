using System;
using UnityEngine;
using KevinCastejon.MoreAttributes;
using LabirinKata.Gameplay.EventHandler;
using LabirinKata.Item;
using UnityEngine.Serialization;

namespace LabirinKata.Entities.Player
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerManager : MonoBehaviour
    {
        #region Variable
        
        [Header("Health")] 
        [SerializeField] private int healthCount;
        [SerializeField] [ReadOnly] private int currentHealthCount;
        [SerializeField] private GameObject[] healthUIObjects;

        private bool _isPlayerDead;

        public GameObject[] HealthUIObjects => healthUIObjects;
        public int CurrentHealthCount
        {
            get => currentHealthCount;
            set => currentHealthCount = value;
        }
        
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
           InitializeHealth();
        }
        
        #endregion
        
        #region Labirin Kata Callbacks
        
        private void InitializeHealth()
        {
            if (healthCount != healthUIObjects.Length)
            {
                Debug.LogWarning("health count ga sama dgn isi health ui lur");
                return;
            }
            
            currentHealthCount = healthCount;
            _isPlayerDead = false;
        }
        
        //-- Core Functionality
        private void DecreaseHealth()
        {
            var healthIndex = currentHealthCount - 1;
            healthUIObjects[healthIndex].gameObject.SetActive(false);
            currentHealthCount--;
            
            if (currentHealthCount < 0)
            {
                currentHealthCount = 0;
                _isPlayerDead = true;
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
                var interactObject = other.GetComponent<ITakeable>();
                Debug.Log($"take {other.name}");
                interactObject.Taken();
            }
        }
        
        #endregion
    }
}