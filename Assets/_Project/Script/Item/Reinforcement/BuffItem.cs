using System;
using LabirinKata.Entities.Player;
using LabirinKata.Enum;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LabirinKata.Item.Reinforcement
{
    public class BuffItem : MonoBehaviour
    {
        #region Variable

        [Header("Buff")] 
        public BuffType BuffType;
        [SerializeField] private Transform[] spawnPointTransforms;
        
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
        
        //-- Initialization
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
        
        //-- Core Functionality
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