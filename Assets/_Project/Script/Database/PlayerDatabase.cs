using UnityEngine;
using Alphabet.Data;
using Alphabet.DesignPattern.Singleton;

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

        private void OnEnable()
        {
            SetPlayerData(0);
        }

        #endregion

        #region Methods

        public void SetPlayerData(int dataIndex)
        {
            _selectedPlayerData = playerData[dataIndex];
        }
        
        public PlayerData GetPlayerDatabyIndex(int index)
        {
            if (index > playerData.Length - 1)
            {
                Debug.Log("index kebanyakan kang!");
                return null;
            }

            return playerData[index];
        }

        public PlayerData GetPlayerDatabySelected()
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
