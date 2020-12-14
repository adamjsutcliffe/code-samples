using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Peak.Speedoku.Scripts.Game.Gameplay
{
    public class KeyButtonScript : MonoBehaviour
    {
        [SerializeField] private int keyNumber;
        [SerializeField] private TextMeshProUGUI KeyLabel;
        [SerializeField] private KeyboardColourSettings colourSettings;
        [SerializeField] private Button buttonScript;

        public int KeyNumber => keyNumber;

        private void Start()
        {
            ColorBlock buttonColours = buttonScript.colors;
            buttonColours.normalColor = colourSettings.backgroundColour;
            buttonColours.highlightedColor = colourSettings.highlightColour;
            buttonColours.pressedColor = colourSettings.pressedColour;
            buttonColours.disabledColor = colourSettings.disabledColour;
            buttonScript.colors = buttonColours;

            KeyLabel.text = $"{keyNumber}";
            KeyLabel.color = colourSettings.labelColour;
            KeyLabel.fontSize = colourSettings.fontSize;
        }
    }
}
