using UnityEngine;

namespace CariHuruf.Data
{
    [CreateAssetMenu(fileName = "NewPlayerData", menuName = "ScriptableObject/Entities/New Player Data", order = 0)]
    public class PlayerData : ScriptableObject
    {
        public string PlayerName;
        [field: SerializeField] public float MoveSpeed { get; private set; }
    }
}