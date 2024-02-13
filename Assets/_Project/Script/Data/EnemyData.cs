using System;
using LabirinKata.Enum;
using UnityEngine;

namespace LabirinKata.Data
{
    [CreateAssetMenu(fileName = "NewEnemyData", menuName = "ScriptableObject/Entities/New Enemy Data", order = 0)]
    public class EnemyData : ScriptableObject
    {
        [SerializeField] private string enemyName;
        public string EnemyName => enemyName;
        [SerializeField] private float enemyMoveSpeed;
        public float EnemyMoveSpeed => enemyMoveSpeed;
    }
}