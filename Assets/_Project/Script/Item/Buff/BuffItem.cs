using System;
using UnityEngine;
using Alphabet.Enum;
using Alphabet.Entities.Player;

using Random = UnityEngine.Random;

namespace Alphabet.Letter
{
    public class BuffItem : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Buff")] 
        public BuffType BuffType;
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

        protected virtual void InitializeOnStart() { }
        
        // !-- Core Functionality
        protected virtual void ActivateBuff()
        {
            IsBuffActive = true;
            PlayerManager.HasBuffEffect = true;
        }

        public virtual void DeactivateBuff()
        {
            IsBuffActive = false;
            PlayerManager.HasBuffEffect = false;
        }
        
        #endregion
        
    }
}