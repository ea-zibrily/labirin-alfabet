using System;
using System.Collections.Generic;
using UnityEngine;

namespace Alphabet.Letter
{
    public static class LetterHelper
    {
        public static GameObject GetLetterManagerObject()
        {
            var letterParent = GameObject.Find("Letter").transform;
            return letterParent.GetChild(letterParent.childCount - 1).gameObject;
        }
    }
}