using System;
using System.Collections.Generic;
using UnityEngine;

namespace Alphabet.Gameplay.EventHandler
{
    public class CameraEventHandler
    {
        public static event Action OnCameraShake;
        public static event Action OnCameraShiftIn;
        public static event Action OnCameraShiftOut;

        public static void CameraShakeEvent() => OnCameraShake?.Invoke();
        public static void CameraShiftInEvent() => OnCameraShiftIn?.Invoke();
        public static void CameraShiftOutEvent() => OnCameraShiftOut?.Invoke();
    }
}