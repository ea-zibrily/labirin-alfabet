using UnityEngine;
using UnityEditor.Animations;

namespace Alphabet.Data
{
    [CreateAssetMenu(fileName = "NewPlayerData", menuName = "ScriptableObject/Entities/New Player Data", order = 0)]
    public class PlayerData : ScriptableObject
    {
        [Header("Data")]
        [SerializeField] private string playerName;
        [SerializeField] private float playerMoveSpeed;

        public string PlayerName => playerName;
        public float PlayerMoveSpeed => playerMoveSpeed;

        [Header("Assets")]
        [SerializeField] private Sprite playerSprite;
        [SerializeField] private RuntimeAnimatorController  playerAnimatorController;


        public Sprite PlayerSprite => playerSprite;
        public RuntimeAnimatorController  PlayerAnimatorController => playerAnimatorController;
    }
}