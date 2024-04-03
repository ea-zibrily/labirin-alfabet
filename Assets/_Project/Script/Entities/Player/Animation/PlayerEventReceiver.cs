using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace Alphabet.Entities.Player
{
    public class PlayerEventReceiver : MonoBehaviour
    {
        private SkeletonAnimation _skeletonAnimation;
        private PlayerPickThrow _playerPickThrow;

        private void Awake()
        {
            _skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
            _playerPickThrow = transform.parent.GetComponent<PlayerPickThrow>();
        }
        
        private void Start()
        {
            _skeletonAnimation.AnimationState.Event += ShootEvent;
        }
        
        #region Spine Event Method

        private void ShootEvent(TrackEntry trackEntry, Spine.Event e)
        {
            Debug.Log($"event fired! {e.Data.Name}");
            if (e.Data.Name == "shot")
            {
                Debug.Log("shot");
                _playerPickThrow.IsThrowItem = false;
                _playerPickThrow.CallThrowItem();
            }
        }

        #endregion
    }
}
