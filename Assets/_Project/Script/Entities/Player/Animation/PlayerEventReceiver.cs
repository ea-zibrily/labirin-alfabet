using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alphabet.Entities.Player
{
    public class PlayerEventReceiver : MonoBehaviour
    {
        private PlayerPickThrow _playerPickThrow;

        private void Awake()
        {
            _playerPickThrow = transform.parent.GetComponent<PlayerPickThrow>();    
        }

        #region Mecanim Event Method

        private void shot()
        {
            _playerPickThrow.CallThrowItem();
        }

        #endregion
    }
}
