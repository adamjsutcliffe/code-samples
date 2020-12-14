using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Peak.UnityGameFramework.Scripts.Common.Dictionary.LetterDistribution
{
    [CreateAssetMenu(fileName = "LetterData", menuName = "ScriptableObjects/Dictionary/LetterData", order = 1)]
    public class LetterData : ScriptableObject
    {
        public string letter;
        public int value;
        public int distribution;
    }
}