using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Peak.QuixelLogic.Scripts.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Peak.QuixelLogic.Scripts.Game
{
    public class HowToPlayWindowScript : MonoBehaviour
    {
        [SerializeField]
        private ButtonController prevButton;

        [SerializeField]
        private ButtonController nextButton;

        [SerializeField]
        private TextMeshProUGUI instructionsText;

        private List<string> instructionStrings = new List<string>();

        [SerializeField]
        private Image pictureHolder;

        [SerializeField]
        private List<Sprite> howToPlayImages = new List<Sprite>();

        public int instructionStage;

        private void Start()
        {
            for (int i = 0; i < GameConstants.HowToPlay.steps.Count; i++)
            {
                instructionStrings.Add(GameConstants.HowToPlay.steps[i]);
            }

            instructionStage = 0;
            pictureHolder.sprite = howToPlayImages[instructionStage];
            instructionsText.text = instructionStrings[instructionStage];

            CheckButtons();
        }

        private void CheckButtons()
        {
            if (instructionStage.Equals(0))
            {
                prevButton.SetInteractability(false);
                nextButton.SetInteractability(true);
            }
            else if (instructionStage.Equals(7))
            {
                prevButton.SetInteractability(true);
                nextButton.SetInteractability(false);
            }
            else
            {
                prevButton.SetInteractability(true);
                nextButton.SetInteractability(true);
            }
        }

        [UsedImplicitly]
        public void HowToPlayNext()
        {
            if (instructionStage < 7)
            {
                instructionStage += 1;

                pictureHolder.sprite = howToPlayImages[instructionStage];
                instructionsText.text = instructionStrings[instructionStage];
            }

            CheckButtons();
        }

        [UsedImplicitly]
        public void HowToPlayPrev()
        {
            if (instructionStage >= 0)
            {
                instructionStage -= 1;

                pictureHolder.sprite = howToPlayImages[instructionStage];
                instructionsText.text = instructionStrings[instructionStage];
            }

            CheckButtons();
        }

        [UsedImplicitly]
        public void ExitWindow()
        {
            gameObject.SetActive(false);

            instructionStage = 0;
            pictureHolder.sprite = howToPlayImages[instructionStage];
            instructionsText.text = instructionStrings[instructionStage];

            CheckButtons();
        }
    }
}