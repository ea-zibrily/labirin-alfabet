using System;
using System.Collections.Generic;
using KevinCastejon.MoreAttributes;
using LabirinKata.Enum;
using UnityEngine;

namespace LabirinKata.Item.Letter
{
    [Serializable]
    public struct LetterObject
    {
        public List<GameObject> LetterObjects;
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