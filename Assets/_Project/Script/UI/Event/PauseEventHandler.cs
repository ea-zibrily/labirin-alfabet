using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alphabet.UI
{
    public class PauseEventHandler : MonoBehaviour
    {
        public event Action OnGamePause;
        public void GamePauseEvent() => OnGamePause?.Invoke();
    }
}
