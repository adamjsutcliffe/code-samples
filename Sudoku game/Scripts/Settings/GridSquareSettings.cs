using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GridSquareSettings", menuName = "ScriptableObjects/CreateGridSquareSettings", order = 1)]
public class GridSquareSettings : ScriptableObject
{
    [Header("Background")]
    public Color backgroundColour;
    public Color highlightColour;
    public Color targetColour;
    public Color targetSelectedColour;
    [Header("Line")]
    public Color lineTargetSelectedColour;
    public Color lineColour;
    [Header("Label")]
    public Color labelColour;
    public Color labelTargetColour;
	public Color successColour;
	public Color failureColour;
    public float fontSize;
}
