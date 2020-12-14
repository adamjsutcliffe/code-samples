using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatioObject
{
    private int totalRatioCount;
    private int[] ratioArray;

    public RatioObject(int[] ratios)
    {
        totalRatioCount = 0;
        ratioArray = ratios;
        for (int i = 0; i < ratios.Length; i++)
        {
            totalRatioCount += ratios[i];
        }
    }

    public int returnRandomValue()
    {
        int random = Random.Range(0, totalRatioCount);
        int boundary = 0;
        for (int i = 0; i < ratioArray.Length; i++)
        {
            boundary += ratioArray[i];
            if (random < boundary)
            {
                return i;
            }
        }
        return 0;
    }
}
