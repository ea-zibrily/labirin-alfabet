using System;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

namespace Alphabet.Entities.Enemy
{
    public class EnemyHelper
    {
        public bool IsChangeDirection() => Random.value > 0.5f;
    }
}