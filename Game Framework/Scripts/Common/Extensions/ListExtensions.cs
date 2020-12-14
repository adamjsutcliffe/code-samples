using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Peak.UnityGameFramework.Scripts
{
    public static class ListExtensions
    {
        public static void Shuffle<T>(this List<T> self)
        {
            for (int i = 0; i < self.Count; i++)
            {
                T temp = self[i];
                int randomIndex = Random.Range(i, self.Count);
                self[i] = self[randomIndex];
                self[randomIndex] = temp;
            }
        }
    }
}
