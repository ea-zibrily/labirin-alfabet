using System;
using UnityEngine;

namespace Alphabet.Data
{
    [CreateAssetMenu(fileName = "NewEnemyData", menuName = "ScriptableObject/Entities/New Enemy Data", order = 0)]
    public class EnemyData : ScriptableObject
    {
        [SerializeField] private string enemyName;
        [SerializeField] private float enemyMoveSpeed;

        public string EnemyName => enemyName;
        public float EnemyMoveSpeed => enemyMoveSpeed;
    }
}