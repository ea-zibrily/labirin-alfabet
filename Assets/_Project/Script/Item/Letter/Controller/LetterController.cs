using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using KevinCastejon.MoreAttributes;
using Alphabet.Data;
using Alphabet.Enum;
using Alphabet.Item;
using Alphabet.Managers;

namespace Alphabet.Letter
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
        private readonly float sfxDelayTime = 0.8f;

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

            var letter = GameObject.FindGameObjectWithTag("LetterManager");
            LetterManager = letter.GetComponent<LetterManager>();
            LetterInterfaceManager = letter.GetComponent<LetterInterfaceManager>();
        }
        
        #endregion
        
        #region Methods
        
        // !-- Initialize
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
        
        // !- Core
        public void Taken()
        {
            if (!hasLetterTaken)
            {
                LetterManager.TakeLetterEvent(_letterData);
                hasLetterTaken = true;
            }
            StartCoroutine(HandleSfxRoutine());
            StartCoroutine(HandleVfxRoutine(letterVfx));
            LetterInterfaceManager.TakeLetter(SpawnId);
        }

        public void ReleaseLetter()
        {
            // Release letter ke pool
            _objectPool.Release(this);
        }
        
        private IEnumerator HandleSfxRoutine()
        {
            FindObjectOfType<AudioManager>().PlayAudio(Musics.LetterSfx);
            yield return new WaitForSeconds(sfxDelayTime);
            LetterAudioManager.PlayAudioEvent(_letterData.LetterId);
        }

        private IEnumerator HandleVfxRoutine(GameObject vfx)
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