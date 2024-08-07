using System;
using System.Collections.Generic;
using UnityEngine;
using DanielLochner.Assets.SimpleScrollSnap;
using Spine.Unity;
using Alphabet.Database;
using Alphabet.Enum;
using Alphabet.Stage;

namespace Alphabet.Mission
{
    public class MissionAnimation : MonoBehaviour
    {
        #region Struct
        [Serializable]
        public struct GraphicData
        {
            public MissionType MissionType;
            public SkeletonGraphic SkeletonGraphic;
            public ParticleSystem GraphicVfx;
        }
        #endregion

        #region Fields & Properties

        [Header("Graphics")]
        [SerializeField] private GraphicData[] graphicDatas;
        [SerializeField] private SimpleScrollSnap scrollSnap;
        
        private MissionEventReceiver _eventReceiver;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _eventReceiver = GetComponent<MissionEventReceiver>();
        }

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
            for (var i = 0; i < graphicDatas.Length; i++)
            {
                var graphic = graphicDatas[i].SkeletonGraphic;
                var vfx = graphicDatas[i].GraphicVfx;

                graphic.freeze = i > 0;
                if (vfx != null)
                {
                    vfx.Stop();
                    vfx.gameObject.SetActive(false);
                }
                ChangeIconSkin(graphic, playerData.PlayerSkin);
            }
        }

        #endregion
        
        #region Methods

        private void HandleAnimation()
        {
            var selectedPanel = scrollSnap.SelectedPanel;

            var mission = graphicDatas[selectedPanel].MissionType;
            var graphic = graphicDatas[selectedPanel].SkeletonGraphic;
            var vfx = graphicDatas[selectedPanel].GraphicVfx;

            graphic.freeze = false;
            _eventReceiver.SubsGraphicEvent(mission, graphic, vfx);
        }
        
        public void ResetAnimation()
        {
            for (var i = 0; i < graphicDatas.Length; i++)
            {
                var graphic = graphicDatas[i].SkeletonGraphic;
                graphic.freeze = i > 0;
                graphic.gameObject.SetActive(i < 1);
            }
            _eventReceiver.UnsubsGraphicEvent();
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