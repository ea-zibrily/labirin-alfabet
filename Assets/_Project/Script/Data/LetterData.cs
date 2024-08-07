using System;
using Alphabet.Enum;
using UnityEngine;

namespace Alphabet.Data
{
    [CreateAssetMenu(fileName = "NewLetterData", menuName = "ScriptableObject/Item/New Letter Data", order = 0)]
    public class LetterData : ScriptableObject
    {
        [Header("Data")]
        [SerializeField] private string letterName;
        [SerializeField] private int letterId;
        [SerializeField] private bool hasTaken;

        public string LetterName => letterName;
        public int LetterId => letterId;
        public bool HasTaken => hasTaken;

        [Header("Assets")]
        [SerializeField] private Sprite letterSprite;
        [SerializeField] private AudioClip letterAudio;
        
        public Sprite LetterSprite => letterSprite;
        public AudioClip LetterAudio => letterAudio;
    }
}