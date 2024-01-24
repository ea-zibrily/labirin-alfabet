using System;
using UnityEngine;
using LabirinKata.Enum;

namespace LabirinKata.Entities.Item
{
    //-- Letter Struct
    
    [Serializable]
    public struct LetterSpawns
    {
        public StageList StageName;
        public int AmountOfLetter;
        public Transform SpawnParentTransform;
        public Transform[] SpawnPointTransforms;
    }

    [Serializable]
    public struct LetterImages
    {
        public StageList StageName;
        public int AmountOfLetter;
        public Sprite[] LetterSprites;
    }
}