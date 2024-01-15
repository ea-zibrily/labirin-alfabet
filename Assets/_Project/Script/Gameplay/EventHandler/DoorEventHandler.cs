using System;
using UnityEngine;

namespace CariHuruf.Gameplay.EventHandler
{
    public class DoorEventHandler : MonoBehaviour
    {
        public event Action OnDoorOpen;
        public void DoorOpenEvent() => OnDoorOpen?.Invoke();
    }
}