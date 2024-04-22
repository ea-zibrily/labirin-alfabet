using UnityEngine;

namespace Alphabet.Item
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class HealthBuff : BuffItem, ITakeable
    {
        #region Fields & Properties
        
        [Header("Health Buff")]
        [SerializeField] private int maxHealth;
        
        #endregion
        
        #region Labirin Kata Callbacks
        
        // !-- Core Functionality
        public void Taken()
        {
            if (PlayerManager.CurrentHealthCount > maxHealth || CheckActiveHealthUI()) return;
            
            ActivateBuff();
            DeactivateBuff();
        }
        
        protected override void ActivateBuff()
        {
            base.ActivateBuff();
            PlayerManager.CurrentHealthCount++;
            var healthIndex = PlayerManager.CurrentHealthCount - 1;
            PlayerManager.HealthUIFills[healthIndex].SetActive(true);
        }
        
        public override void DeactivateBuff()
        {
            base.DeactivateBuff();
            gameObject.SetActive(false);
        }
        
        // !-- Helper/Utilites
        private bool CheckActiveHealthUI()
        {
            var activeCount = 0;
            foreach (var healthUI in PlayerManager.HealthUIFills)
            {
                if (healthUI.activeSelf)
                {
                    activeCount++;
                }
            }

            return activeCount >= maxHealth;
        }
        
        #endregion
    }
}