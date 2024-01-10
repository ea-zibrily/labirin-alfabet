using System;
using UnityEngine;

namespace CariHuruf.Entities.Player
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerManager : MonoBehaviour
    {
        #region Collider Callbacks
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            // TODO: Drop logic when player triggered with another object here
        }
        
        #endregion
    }
}