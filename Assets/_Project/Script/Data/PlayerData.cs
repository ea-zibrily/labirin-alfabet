using UnityEngine;
using Spine;
using Spine.Unity;
using System;

namespace Alphabet.Data
{
    [CreateAssetMenu(fileName = "NewPlayerData", menuName = "ScriptableObject/Entities/New Player Data", order = 0)]
    public class PlayerData : ScriptableObject
    {
        #region Enum
        public enum SpineSkine
        {
            devan,
            rhea
        }
        #endregion

        [Header("Data")]
        [SerializeField] private string playerName;
        [SerializeField] private float playerMoveSpeed;
        [SerializeField] private SpineSkine playerSkin;

        public string PlayerName => playerName;
        public float PlayerMoveSpeed => playerMoveSpeed;
        public string PlayerSkin => playerSkin.ToString().ToLower();
    }
}