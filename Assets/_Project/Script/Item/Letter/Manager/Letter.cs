using System;
using System.Collections.Generic;
using UnityEngine;
using Alphabet.Enum;

namespace Alphabet.Letter
{
    [Serializable]
    public struct LetterObject
    {
        public List<GameObject> LetterObjects;
    }
    
    [Serializable]
    public struct LetterSpawns
    {
        public StageNum StageName;
        public int AmountOfLetter;
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