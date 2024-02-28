using System;
using UnityEngine;
using LabirinKata.Enum;
using LabirinKata.Entities.Player;

using Random = UnityEngine.Random;

namespace LabirinKata.Item
{
    public class BuffItem : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Buff")] 
        public BuffType BuffType;
        [SerializeField] private Transform[] spawnPointTransforms;
        [SerializeField] private bool isBuffActive;

        public bool IsBuffActive
        {
            get => isBuffActive;
            protected set => isBuffActive = value;
        }
        
        [Header("Reference")]
        protected PlayerController PlayerController;
        protected PlayerManager PlayerManager;
        
        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            InitializeOnAwake();
        }

        private void Start()
        {
            InitializeOnStart();
        }

        #endregion

        #region Labirin Kata Callbacks
        
        // !-- Initialization
        protected virtual void InitializeOnAwake()
        {
            var playerObject = GameObject.FindGameObjectWithTag("Player");
            
            PlayerController = playerObject.GetComponent<PlayerController>();
            PlayerManager = playerObject.GetComponentInChildren<PlayerManager>();
        }

        protected virtual void InitializeOnStart()
        {
            RandomizeBuffPosition();
        }
        
        // !-- Core Functionality
        protected virtual void ActivateBuff()
        {
            IsBuffActive = true;
        }

        public virtual void DeactivateBuff()
        {
            IsBuffActive = false;
        }
        
        private void RandomizeBuffPosition()
        {
            var randomSpawnIndex = Random.Range(0, spawnPointTransforms.Length - 1);
            transform.position = spawnPointTransforms[randomSpawnIndex].position;
            foreach (var spawnPoint in spawnPointTransforms)
            {
                spawnPoint.gameObject.SetActive(false);
            }
        }
        
        #endregion
        
    }
}