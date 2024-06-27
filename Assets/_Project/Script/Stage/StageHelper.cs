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
            var stageLenght = System.Enum.GetNames(typeof(StageName)).Length;
            if (stageIndex > stageLenght - 1)
            {
                Debug.LogWarning("indexny kebanyakan kang");
                return null;
            }

            var stageName = (StageName)stageIndex;
            return stageName.ToString();
        }
        
        public static string GetStageNameByEnum(StageName level)
        {
            return level switch
            {
                StageName.Gua_Aksara => "Gua Aksara",
                StageName.Hutan_Abjad => "Hutan Abjad",
                StageName.Kuil_Litera => "Kuil Litera",
                _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
            };
        }

        public static string GetStageNameByInt(int stageIndex)
        {
            var stageLenght = System.Enum.GetNames(typeof(StageName)).Length;
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

        public static int GetStageInt(StageName stageName)
        {
            return stageName switch
            {
                StageName.Gua_Aksara => 1,
                StageName.Hutan_Abjad => 2,
                StageName.Kuil_Litera => 3,
                _ => throw new ArgumentOutOfRangeException(nameof(stageName), stageName, null)
            };
        }
    }
}