using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Peak.Speedoku.Scripts.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Peak.Speedoku.Scripts.Game.Gameplay
{
    public partial class GridSquareScript : MonoBehaviour //INFO: CREATOR
    {
    
        #region Creator

        private enum CreatorSquare
        {
            None,
            Selected,
            Target
        }

        private CreatorSquare creatorState;

        public void SetupCreatorGridSquare(int index, int number, Action<GridSquareScript> callback = null)
        {
            this.index = index;
            this.number = number;
            this.callback = callback;
            creatorState = CreatorSquare.None;
            //isCorrect = false;
            isTarget = false;
            gridLabel.text = number == 0 ? "" : $"{number}";
            gridLabel.color = isTarget ? settings.labelTargetColour : settings.labelColour;
            background.color = isTarget ? settings.targetColour : settings.backgroundColour;
            targetButton.interactable = true;
        }

        public void SelectCreatorSquare()
        {
            print($"Select creator index: {index} start state: {creatorState}");
            if (creatorState == CreatorSquare.None)
            {
                background.color = settings.highlightColour;
                creatorState = CreatorSquare.Selected;
                isTarget = false;
                callback?.Invoke(this);
            }
            else if (creatorState == CreatorSquare.Selected)
            {
                background.color = settings.targetSelectedColour;
                creatorState = CreatorSquare.Target;
                isTarget = true;
                number = 0;
                gridLabel.text = number == 0 ? "" : $"{number}";
            }
            else
            {
                background.color = settings.backgroundColour;
                creatorState = CreatorSquare.None;
                isTarget = false;
            }
            print($"Finished state = {creatorState} is target: {isTarget}");
        }

        public void DeselectCreatorSquare()
        {
            if (creatorState != CreatorSquare.Target)
            {
                background.color = settings.backgroundColour;
                creatorState = CreatorSquare.None;
                isTarget = false;
            }
        }

        public void UpdateCreatorSquare(int number)
        {
            if (creatorState == CreatorSquare.Selected)
            {
                this.number = number;
                gridLabel.text = number == 0 ? "" : $"{number}";
            }
        }

        #endregion

        //#region Testing only
        ////Test
        //public void SetupTestGridSquare(int index, int number, bool target)
        //{
        //    this.index = index;
        //    this.number = number;
        //    isTarget = target;
        //}

        //public void UpdateTestGridNumber(int selection)
        //{
        //    number = selection;
        //}
        //#endregion
    }
}
