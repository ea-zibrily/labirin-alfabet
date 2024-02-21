using System;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.MoreAttributes;
using LabirinKata.Enum;

namespace LabirinKata.Item
{
    [Serializable]
    public struct LetterObject
    {
        public List<GameObject> LetterObjects;
    }
    
    [Serializable]
    public struct LetterSpawns
    {
        public Enum.StageNum StageName;
        public int AmountOfLetter;
        public Transform SpawnParentTransform;
        public Transform[] SpawnPointTransforms;
    }

    [Serializable]
    public struct LetterImages
    {
        public Enum.StageNum StageName;
        public int AmountOfLetter;
        public Sprite[] LetterSprites;
    }
}