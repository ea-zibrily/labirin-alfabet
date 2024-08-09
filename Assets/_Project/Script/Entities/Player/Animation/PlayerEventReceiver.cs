using UnityEngine;
using Spine;
using Spine.Unity;
using Alphabet.Enum;
using Alphabet.Managers;

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
            _skeletonAnimation.AnimationState.Event += HandleEvent;
        }
        
        #region Spine Event Method

        private void HandleEvent(TrackEntry trackEntry, Spine.Event e)
        {
            switch (e.Data.Name)
            {
                case "shot":
                    _playerPickThrow.IsThrowItem = false;
                    _playerPickThrow.CallThrowItem();
                    break;
                case "footstep":
                    FindObjectOfType<AudioManager>().PlayAudio(Musics.FootstepSfx);
                    break;
            }
        }

        #endregion
    }
}
