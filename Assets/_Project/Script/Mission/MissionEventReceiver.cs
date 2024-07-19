using System;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
using Alphabet.Enum;
using Alphabet.Managers;

namespace Alphabet.Mission
{
    public class MissionEventReceiver : MonoBehaviour
    {
        private MissionType _currentMission;
        private SkeletonGraphic _currentGraphic;
        private ParticleSystem _currentVfx;

        #region Spine Event Method

        public void SubsGraphicEvent(MissionType mission, SkeletonGraphic graphic, ParticleSystem particle)
        {
            if (_currentGraphic != null) 
            {
                _currentVfx.gameObject.SetActive(false);
                _currentGraphic.AnimationState.Event -= HandleEvent;
                _currentGraphic.AnimationState.Complete -= HandleComplete;
            }

            _currentMission = mission;
            _currentGraphic = graphic;
            _currentVfx = particle;

            _currentGraphic.AnimationState.Event += HandleEvent;
            _currentGraphic.AnimationState.Complete += HandleComplete;
        }

        public void UnsubsGraphicEvent()
        {
            _currentGraphic.AnimationState.Event -= HandleEvent;
            _currentGraphic.AnimationState.Complete -= HandleComplete;

            _currentMission = MissionType.None;
            _currentGraphic = null;
            _currentVfx = null;
        }

        private void HandleEvent(TrackEntry trackEntry, Spine.Event e)
        {
            if (e.Data.Name == "vfx")
            {
                var sfx = GetSfx(_currentMission);
                FindObjectOfType<AudioManager>().PlayAudio(sfx);
                _currentVfx.gameObject.SetActive(true);
                _currentVfx.Play();
            }
        }

        private void HandleComplete(TrackEntry trackEntry)
        {
            Debug.Log(trackEntry);
            if (_currentMission == MissionType.Speed)
            {
                _currentVfx.Stop();
                _currentVfx.gameObject.SetActive(false);
            };   
        }

        // !- Helpers
        private Musics GetSfx(MissionType type)
        {
            return type switch
            {
                MissionType.Rocks => Musics.StonebreakSfx,
                MissionType.Health => Musics.HealSfx,
                MissionType.Speed => Musics.SpeedSfx,
                _ => throw new NotImplementedException()
            };
        }

        #endregion
    }
}