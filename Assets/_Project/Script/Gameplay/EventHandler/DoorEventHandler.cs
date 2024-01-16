using System;
using UnityEngine;

namespace LabirinKata.Gameplay.EventHandler
{
    public class DoorEventHandler : MonoBehaviour
    {
        public event Action OnDoorOpen;
        public void DoorOpenEvent() => OnDoorOpen?.Invoke();
    }
}