using System;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

namespace LabirinKata.Entities.Enemy
{
    public class EnemyHelper
    {
        // !-- Helpers
        public bool IsChangeDirection() => Random.value > 0.5f;
    }
}