using System.Collections;
using UnityEngine;
using Alphabet.Enum;
using Alphabet.Managers;

namespace Alphabet.Item
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class HealthBuff : BuffItem, ITakeable
    {
        #region Fields & Properties
        
        [Header("Health Buff")]
        [SerializeField] private int maxHealth;
        
        #endregion
        
        #region Methods
        
        // !- Core
        public void Taken()
        {
            if (PlayerManager.CurrentHealthCount >= maxHealth) return;

            ActivateBuff();
            DeactivateBuff();
        }
        
        protected override void ActivateBuff()
        {
            base.ActivateBuff();
            var buffRenderer = GetComponentInChildren<SpriteRenderer>();
            
            buffRenderer.enabled = false;
            PlayerManager.CurrentHealthCount++;
            var healthIndex = PlayerManager.CurrentHealthCount - 1;
            PlayerManager.HealthUIFills[healthIndex].SetActive(true);
            AudioManager.Instance.PlayAudio(Musics.HealSfx);
        }
        
        public override void DeactivateBuff()
        {
            base.DeactivateBuff();
            StartCoroutine(HandleBuffEffect(PlayerManager.HealEffect));
        }
        
        private IEnumerator HandleBuffEffect(GameObject buffEffect)
        {
            if (buffEffect.TryGetComponent<ParticleSystem>(out var healEffect))
            {
                var smokeEffect = buffEffect.transform.GetChild(0).GetComponent<ParticleSystem>();
                var effectDuration = healEffect.main.duration + smokeEffect.main.duration;

                buffEffect.SetActive(true);

                yield return new WaitForSeconds(effectDuration);
                buffEffect.SetActive(false);
                gameObject.SetActive(false);
            }
        }
        
        #endregion
    }
}