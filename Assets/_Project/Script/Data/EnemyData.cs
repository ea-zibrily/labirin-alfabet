using System;
using CariHuruf.Enum;
using UnityEngine;

namespace CariHuruf.Data
{
    [CreateAssetMenu(fileName = "NewEnemyData", menuName = "ScriptableObject/Entities/New Enemy Data", order = 0)]
    public class EnemyData : ScriptableObject
    {
        public string EnemyName;
        public EnemyPattern EnemyPattern;
        [field: SerializeField] public float EnemyMove { get; private set; }
        public int MaxEnemyPoint
        {
            get
            {
                var maxPoint = EnemyPattern switch
                {
                    EnemyPattern.Line => 2,
                    EnemyPattern.Elbow => 3,
                    EnemyPattern.Box => 4,
                    _ => throw new ArgumentOutOfRangeException()
                };

                return maxPoint;
            }
        }
    }
}