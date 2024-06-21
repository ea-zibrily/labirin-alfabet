using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using KevinCastejon.MoreAttributes;
using Alphabet.Data;

namespace Alphabet.Item
{
    public class LetterController : MonoBehaviour, ITakeable
    {
        #region Fields & Properties

        [Header("Data")] 
        [SerializeField] [ReadOnly] private int spawnId;
        [SerializeField] private bool hasLetterTaken;
        [SerializeField] private GameObject letterVfx;

        private LetterData _letterData;
        private string _letterName;

        public int SpawnId
        {
            get => spawnId;
            set => spawnId = value;
        }

        // Pool
        private ObjectPool<LetterController> _objectPool;
        public ObjectPool<LetterController> ObjectPool {set => _objectPool = value; }

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
        
        #endregion
        
        #region Labirin Kata Callbacks
        
        // !-- Initialization
        public void InitializeLetterData(LetterData data, int spawnNum)
        {
            // Data
            _letterData = data;
            spawnId = spawnNum;

            // Component
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
            LetterInterfaceManager.TakeLetterEvent(SpawnId);
            StartCoroutine(ActivateVfxRoutine(letterVfx));
        }

        public void ReleaseLetter()
        {
            // Release letter ke pool
            _objectPool.Release(this);
        }
        
        private IEnumerator ActivateVfxRoutine(GameObject vfx)
        {
            var vfxDuration = vfx.GetComponent<ParticleSystem>().main.duration;

            vfx.SetActive(true);
            _spriteRenderer.enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;

            yield return new WaitForSeconds(vfxDuration);
            vfx.SetActive(false);
            _spriteRenderer.enabled = true;
            GetComponent<BoxCollider2D>().enabled = true;
            gameObject.SetActive(false);
        }

        
        #endregion
    }
}