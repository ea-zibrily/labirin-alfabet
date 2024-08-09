using UnityEngine;

namespace Alphabet.Entities.Enemy
{
    public class EnemyHelper
    {
        public bool IsChangeDirection() => Random.value > 0.5f;
    }
}