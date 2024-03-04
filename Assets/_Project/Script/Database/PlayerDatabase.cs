using System.Collections;
using System.Collections.Generic;
using Alphabet.Data;
using Alphabet.DesignPattern.Singleton;
using UnityEngine;

namespace Alphabet.Database
{
    public class PlayerDatabase : MonoDDOL<PlayerDatabase>
    {
        #region Fields & Property

        [Header("Data")]
        [SerializeField] private PlayerData[] playerData;

        private PlayerData _selectedPlayerData;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            SetPlayerData(0);
        }

        #endregion

        #region Methods

        public void SetPlayerData(int dataIndex)
        {
            _selectedPlayerData = playerData[dataIndex];
        }

        public PlayerData GetPlayerData()
        {
            if (_selectedPlayerData == null)
            {
                Debug.LogError("player data null lekku");
                return null;
            }

            return _selectedPlayerData;
        }

        #endregion
    }
}
