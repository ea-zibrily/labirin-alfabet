using System;
using System.Collections.Generic;
using UnityEngine;
using Alphabet.Enum;

namespace Alphabet.Stage
{
    public static class StageHelper
    {
        public static string GetStageDataKey(int stageIndex)
        {
            var stageLenght = System.Enum.GetNames(typeof(Level)).Length;
            if (stageIndex > stageLenght - 1)
            {
                Debug.LogWarning("indexny kebanyakan kang");
                return null;
            }

            var stageName = (Level)stageIndex;
            return stageName.ToString();
        }

        public static string GetStageStringValue(int stageIndex)
        {
            var stageLenght = System.Enum.GetNames(typeof(Level)).Length;
            if (stageIndex > stageLenght - 1)
            {
                Debug.LogWarning("indexny kebanyakan kang");
                return null;
            }

            return stageIndex switch
            {
                0 => "Gua Aksara",
                1 => "Hutan Abjad",
                2 => "Kuil Litera",
                _ => throw new ArgumentOutOfRangeException(nameof(stageIndex), stageIndex, null)
            };
        }

        public static int GetStageIntValue(Level stageName)
        {
            return stageName switch
            {
                Level.Gua_Aksara => 0,
                Level.Hutan_Abjad => 1,
                Level.Kuil_Litera => 2,
                _ => throw new ArgumentOutOfRangeException(nameof(stageName), stageName, null)
            };
            
        }
    }
}