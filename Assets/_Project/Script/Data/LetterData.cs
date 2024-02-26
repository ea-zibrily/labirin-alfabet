using System;
using LabirinKata.Enum;
using UnityEngine;

namespace LabirinKata.Data
{
    [CreateAssetMenu(fileName = "NewLetterData", menuName = "ScriptableObject/Item/New Letter Data", order = 0)]
    public class LetterData : ScriptableObject
    {
        [Header("Data")]
        [SerializeField] private string letterName;
        [SerializeField] private int letterId;
        [SerializeField] private bool isLetterTaken;

        public string LetterName => letterName;
        public int LetterId => letterId;
        public bool IsLetterTaken => isLetterTaken;

        [Header("Reference")]
        [SerializeField] private Sprite letterSprite;
        [SerializeField] private AudioClip letterAudio;
        
        public Sprite LetterSprite => letterSprite;
        public AudioClip LetterAudio => letterAudio;
    }
}