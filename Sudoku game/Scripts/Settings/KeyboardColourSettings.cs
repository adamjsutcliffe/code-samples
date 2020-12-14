using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KeyboardColourSettings", menuName = "ScriptableObjects/KeyboardColourSettings", order = 4)]
public class KeyboardColourSettings : ScriptableObject
{
    [Header("Background Colours")]
    public Color backgroundColour;
    public Color highlightColour;
    public Color pressedColour;
    public Color disabledColour;

    [Header("Label")]
    public Color labelColour;
    public float fontSize;
}
