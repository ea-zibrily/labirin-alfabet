using System;
using System.Collections.Generic;
using UnityEngine;
using DanielLochner.Assets.SimpleScrollSnap;
using Spine.Unity;
using Alphabet.Database;

namespace Alphabet.Mission
{
    public class MissionAnimation : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Graphics")]
        [SerializeField] private SkeletonGraphic[] skeletonGraphics;
        [SerializeField] private SimpleScrollSnap scrollSnap;

        #endregion

        #region MonoBehaviour Callbacks

        private void OnEnable()
        {
            scrollSnap.OnSnappingEnd += HandleAnimation;
        }

        private void OnDisable()
        {
            scrollSnap.OnSnappingEnd -= HandleAnimation;
        }

        private void Start()
        {
            var playerData = PlayerDatabase.Instance.GetPlayerDatabySelected();
            for (var i = 0; i < skeletonGraphics.Length; i++)
            {
                var graphic = skeletonGraphics[i];
                graphic.freeze = i > 0;
                ChangeIconSkin(graphic, playerData.PlayerSkin);
            }
        }

        #endregion
        
        #region Methods

        private void HandleAnimation()
        {
            var selectedPanel = scrollSnap.SelectedPanel;
            skeletonGraphics[selectedPanel].freeze = false;
        }

        // !- Helpers
        private void ChangeIconSkin(SkeletonGraphic graphic, string skin)
        {
            graphic.Skeleton.SetSkin(skin);
            graphic.Skeleton.SetSlotsToSetupPose();
            graphic.LateUpdate();
        }

        #endregion
    }
}