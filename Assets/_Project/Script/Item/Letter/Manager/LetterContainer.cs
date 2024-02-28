using System.Collections;
using System.Collections.Generic;
using LabirinKata.Data;
using UnityEngine;

namespace LabirinKata.Item
{
    public class LetterContainer : MonoBehaviour
    {
        #region Field & Property

        [SerializeField] private LetterData[] letterDatas;
        public LetterData[] LetterDatas => letterDatas;

        #endregion

        #region MonoBehaviour Callbacks

        // !-- Core Functionality
        public LetterData GetLetterDataById(int id)
        {
            if (id is 0)
            {
                Debug.LogWarning("idmu salah kang, gaada yg 0 awokwok");
                return null;
            }
            
            LetterData selectedData = null;
            foreach (var data in letterDatas)
            {
                if (data.LetterId != id) continue;
                selectedData = data;
                break;
            }

            return selectedData;
        }

        #endregion
    }
}
