using System;
using System.Collections.Generic;
using UnityEngine;

namespace LabirinKata.Gameplay.EventHandler
{
    public class CameraEventHandler
    {
        public static event Action OnCameraShake;
        public static void CameraShakeEvent() => OnCameraShake?.Invoke();
    }
}