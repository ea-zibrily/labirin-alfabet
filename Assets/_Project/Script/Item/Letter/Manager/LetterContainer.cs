using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alphabet.Data;

namespace Alphabet.Letter
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
