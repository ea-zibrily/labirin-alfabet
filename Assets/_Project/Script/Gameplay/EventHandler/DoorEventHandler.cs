using System;
using UnityEngine;

namespace Alphabet.Gameplay.EventHandler
{
    public class DoorEventHandler : MonoBehaviour
    {
        public event Action OnDoorOpen;
        public event Action OnEffectPlaying;

        public void DoorOpenEvent() => OnDoorOpen?.Invoke();
        public void EffectPlayingEvent() => OnEffectPlaying?.Invoke();
    }
}