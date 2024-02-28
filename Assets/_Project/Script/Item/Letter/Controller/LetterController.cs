using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using KevinCastejon.MoreAttributes;
using LabirinKata.Data;

using Random = UnityEngine.Random;
using Unity.VisualScripting;

namespace LabirinKata.Item
{
    public class LetterController : MonoBehaviour, ITakeable
    {
        #region Fields & Properties

        [Header("Data")] 
        [SerializeField] [ReadOnly] private int spawnId;
        [SerializeField] private bool hasLetterTaken;

        private LetterData _letterData;
        private string _letterName;

        public int SpawnId
        {
            get => spawnId;
            set => spawnId = value;
        }

        public ObjectPool<LetterController> ObjectPool { get; set; }

        [Header("Reference")]
        private SpriteRenderer _spriteRenderer;

        public LetterManager LetterManager { get; private set; }
        public LetterInterfaceManager LetterInterfaceManager { get; private set; }
        
        #endregion
        
        #region MonoBehaviour Callbacks
        
        private void Awake()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            var letter = LetterHelper.GetLetterManagerObject();
            LetterManager = letter.GetComponent<LetterManager>();
            LetterInterfaceManager = letter.GetComponent<LetterInterfaceManager>();
        }
        
        private void Start()
        {
            InitializeLetter();
        }
        
        #endregion
        
        #region Labirin Kata Callbacks
        
        // !-- Initialization
        public void InitializeData(LetterData data, int spawnNum)
        {
            _letterData = data;
            spawnId = spawnNum;
        }

        private void InitializeLetter()
        {
            _letterName = _letterData.LetterName;
            hasLetterTaken = _letterData.HasTaken;
            
            gameObject.name = _letterName;
            _spriteRenderer.sprite = _letterData.LetterSprite;
        }
        
        // !-- Core Functionality
        public void Taken()
        {
            if (!hasLetterTaken)
            {
                LetterManager.TakeLetterEvent(_letterData);
                hasLetterTaken = true;
            }
            LetterManager.AddAvailableSpawnPoint(transform);
            LetterInterfaceManager.TakeLetterEvent(SpawnId);

            // Release obstacle back to the pool
            Debug.LogWarning("take item");            
            ObjectPool.Release(this);
        }
        
        #endregion
    }
}