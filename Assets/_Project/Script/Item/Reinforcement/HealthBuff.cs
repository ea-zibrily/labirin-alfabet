using UnityEngine;

namespace LabirinKata.Item.Reinforcement
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class HealthBuff : BuffItem, ITakeable
    {
        #region Variable

        [Header("Health Buff")]
        [SerializeField] private int maxHealth;
        
        #endregion
        
        #region Labirin Kata Callbacks
        
        //-- Core Functionality
        public void Taken()
        {
            if (PlayerManager.CurrentHealthCount > maxHealth || CheckActiveHealthUI()) return;
            
            IncreaseHealth();
            DeactivateBuff();
        }

        private void DeactivateBuff()
        {
            gameObject.SetActive(false);
        }
        
        private void IncreaseHealth()
        {
            PlayerManager.CurrentHealthCount++;
            var healthIndex = PlayerManager.CurrentHealthCount - 1;
            PlayerManager.HealthUIObjects[healthIndex].SetActive(true);
        }
        
        //-- Helper/Utilites
        private bool CheckActiveHealthUI()
        {
            var activeCount = 0;
            foreach (var healthUI in PlayerManager.HealthUIObjects)
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