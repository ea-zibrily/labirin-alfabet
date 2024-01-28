using System;
using System.Collections.Generic;
using KevinCastejon.MoreAttributes;
using LabirinKata.Enum;
using UnityEngine;

namespace LabirinKata.Item.Letter
{
    //-- Letter Struct

    [Serializable]
    public struct LetterObject
    {
        public StageList StageName;
        [ReadOnly] public List<GameObject> LetterObjects;
    }
    
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