using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Peak.UnityGameFramework.Scripts.Common.Dictionary.LetterDistribution
{
    [CreateAssetMenu(fileName = "LetterList", menuName = "ScriptableObjects/Dictionary/LetterList", order = 1)]
    public class LetterList : ScriptableObject
    {
        public List<LetterData> letters;

        private List<string> letterBag = new List<string>();
        private List<string> uniqueLetters = new List<string>();

        public void Init()
        {
            PopulateLetterBag();
        }

        public List<string> GetRandomLetter(int quantity = 1)
        {
            List<string> returnList = new List<string>();
            Debug.Log($"Letterbag length: {letterBag.Count} quantity: {quantity}");
            for (int i = 0; i < quantity; i++)
            {
                if (letterBag.Count <= 0)
                {
                    PopulateLetterBag();
                }
                returnList.Add(letterBag[0]);
                letterBag.RemoveAt(0);
            }
            Debug.Log($"Return list: {String.Join(",", returnList.ToArray())}");
            return returnList;
        }

        public string GetSingleLetterExcludingWildcard()
        {
            if (letterBag.Count <= 0)
            {
                PopulateLetterBag();
            }
            string letter = "*";
            while (letter.Equals("*"))
            {
                letter = letterBag[UnityEngine.Random.Range(0, letterBag.Count)];
            }
            return letter;
        }

        public int GetLetterScore(string letter)
        {
            foreach (LetterData letterData in letters)
            {
                if (letterData.letter.Equals(letter))
                {
                    return letterData.value;
                }
            }
            return 0;
        }

        private void PopulateLetterBag()
        {
            Assert.AreNotEqual(0, letters.Count);
            letterBag = new List<string>();
            uniqueLetters = new List<string>();
            foreach (LetterData letterData in letters)
            {
                for (int i = 0; i < letterData.distribution; i++)
                {
                    letterBag.Add(letterData.letter);
                }
                if (!letterData.letter.Equals("*"))
                {
                    uniqueLetters.Add(letterData.letter);
                }
            }
            letterBag.Shuffle();
        }

        public List<string> GetUniqueLetterListExcludingWildcard()
        {
            if (uniqueLetters.Count <= 0)
            {
                PopulateLetterBag();
            }
            return uniqueLetters;
        }
    }
}
