using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LabirinKata
{
    public class ItemManager : MonoBehaviour
    {
        #region Fields & Property
            
        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
        
        }

        private void Update()
        {

        }
        
        #endregion

        #region Labirin Kata Callbacks

        public void Taken()
        {
            // Some Logic Here
        } 

        #endregion

        #region Collider Callbacks
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            // !-- Logic Here
        }

        #endregion
    }
}
