using System;
using UnityEngine;

namespace Alphabet.Gameplay.EventHandler
{
    public class PadlockEventHandler : MonoBehaviour
    {
        public static event Action<bool> OnPadlockAnimate;
        
        public void PadlockBeginEvent() => OnPadlockAnimate?.Invoke(true);
        public void PadlockDoneEvent() => OnPadlockAnimate?.Invoke(false);
    }
}