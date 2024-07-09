using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using Alphabet.Enum;
using Alphabet.Stage;
using Alphabet.Letter;
using Alphabet.Managers;

namespace Alphabet.Mission
{
    public class Mission : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Base Reference")]
        [SerializeField] protected Button playButtonUI;
        [SerializeField] protected LetterPooler letterPooler;
        [SerializeField] protected SkeletonGraphic skeletonGraphic;
        protected StageMarker _stageMarker;

        #endregion

        #region Methods

        // !- Initialize
        protected virtual void InitOnAwake()
        {
            _stageMarker = GameObject.Find("StageMarker").GetComponent<StageMarker>();
        }

        protected virtual void InitOnStart()
        {
            playButtonUI.onClick.AddListener(OnCloseMission);
        }

        // !- Core
        public virtual void CallTutorial() { }
        protected virtual void MissionHandler() { }
        protected virtual void OnCloseMission()
        {
            FindObjectOfType<AudioManager>().PlayAudio(Musics.ButtonSfx);
        }

        protected void ChangeIconSkin(string skin)
        {
            skeletonGraphic.Skeleton.SetSkin(skin);
            skeletonGraphic.Skeleton.SetSlotsToSetupPose();
            skeletonGraphic.LateUpdate();
        }

        #endregion
    }
}