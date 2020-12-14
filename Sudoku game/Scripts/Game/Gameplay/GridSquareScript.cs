﻿using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Peak.Speedoku.Scripts.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Peak.Speedoku.Scripts.Game.Gameplay
{
    public partial class GridSquareScript : MonoBehaviour //INFO: MAIN
    {
        
        [SerializeField] private TextMeshProUGUI gridLabel;
        [SerializeField] private Image background;
        [SerializeField] private Image lineBackground;
        [SerializeField] private GameObject highlightLineHolder;
        [SerializeField] private Image[] lines;
        [SerializeField] private GridSquareSettings settings;
        [SerializeField] private Button targetButton;
        [SerializeField] private Animator gridAnimator;

        private int index;
        public int Index => index;
        private int number;
        public int Number => number;
        private bool isTarget;
        public bool IsTarget => isTarget;
        private Action<GridSquareScript> callback;

        public void SetupGridSquare(int index, int number, bool target, Action<GridSquareScript> callback = null)
        {
            this.index = index;
            this.number = number;
            this.callback = callback;
            isTarget = target;
            gridLabel.text = number == 0 ? "" : $"{number}";
            gridLabel.color = isTarget ? settings.labelTargetColour : settings.labelColour;
            gridLabel.fontSize = settings.fontSize;
            background.color = isTarget ? settings.targetColour : settings.backgroundColour;
            lineBackground.color = settings.lineColour;
            targetButton.interactable = target;
            highlightLineHolder.SetActive(false);
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i].color = settings.lineTargetSelectedColour;
            }
        }

        public void HighlightSquare(int index) //Auto selection at beginning of game or after previous target entered
        {
            background.color = isTarget ? index == this.index ? settings.targetSelectedColour : settings.targetColour : settings.highlightColour;
            highlightLineHolder.SetActive(index == this.index);
        }

        public void ResetSquare()
        {
            background.color = isTarget ? settings.targetColour : settings.backgroundColour;
            gridLabel.color = isTarget ? settings.labelTargetColour : settings.labelColour;
            highlightLineHolder.SetActive(false);
        }

        [UsedImplicitly] //Grid square prefab button, user selection
        public void SelectSquare()
        {
            print($"Select index: {index}");
#if PLATFORM_IOS
            iOSHapticFeedbackHelper.OnSelection();
#endif
            background.color = settings.targetSelectedColour;
            callback?.Invoke(this);
            highlightLineHolder.SetActive(true);
        }

        public void UpdateGridNumber(int selection)
        {
            number = selection;
            gridLabel.text = number == 0 ? "" : $"{number}";
        }

        public bool DoesTargetHaveNumber()
        {
            return isTarget && number != 0;
        }

        public void MarkSquareComplete()
        {
            gridLabel.color = settings.labelColour;
            //todo add some animation??
        }

        public void MarkSquareIncorrect()
        {
            gridLabel.color = settings.failureColour;
        }

        public void MarkSquareCorrect()
        {
            gridLabel.color = settings.successColour;
        }

        public void ScaleTarget()
        {
            gridAnimator.SetTrigger(Constants.Animation.GridSquare.PopTarget);
        }

        //TEST FLASH
        public void StartFlash()
        {
            if (isTarget) return;
            StartCoroutine(Flash());
        }
        private IEnumerator Flash()
        {
            background.color = settings.highlightColour;
            yield return new WaitForSeconds(0.5f);
            background.color = settings.backgroundColour;
        }

        //#region Creator

        //private enum CreatorSquare
        //{
        //    None,
        //    Selected,
        //    Target
        //}

        //private CreatorSquare creatorState;

        //public void SetupCreatorGridSquare(int index, int number, Action<GridSquareScript> callback = null)
        //{
        //    this.index = index;
        //    this.number = number;
        //    this.callback = callback;
        //    creatorState = CreatorSquare.None;
        //    //isCorrect = false;
        //    isTarget = false;
        //    gridLabel.text = number == 0 ? "" : $"{number}";
        //    gridLabel.color = isTarget ? settings.labelTargetColour : settings.labelColour;
        //    background.color = isTarget ? settings.targetColour : settings.backgroundColour;
        //    targetButton.interactable = true;
        //}

        //public void SelectCreatorSquare()
        //{
        //    print($"Select creator index: {index} start state: {creatorState}");
        //    if (creatorState == CreatorSquare.None)
        //    {
        //        background.color = settings.highlightColour;
        //        creatorState = CreatorSquare.Selected;
        //        isTarget = false;
        //        callback?.Invoke(this);
        //    }
        //    else if (creatorState == CreatorSquare.Selected)
        //    {
        //        background.color = settings.targetSelectedColour;
        //        creatorState = CreatorSquare.Target;
        //        isTarget = true;
        //        number = 0;
        //        gridLabel.text = number == 0 ? "" : $"{number}";
        //    }
        //    else
        //    {
        //        background.color = settings.backgroundColour;
        //        creatorState = CreatorSquare.None;
        //        isTarget = false;
        //    }
        //    print($"Finished state = {creatorState} is target: {isTarget}");
        //}

        //public void DeselectCreatorSquare()
        //{
        //    if (creatorState != CreatorSquare.Target)
        //    {
        //        background.color = settings.backgroundColour;
        //        creatorState = CreatorSquare.None;
        //        isTarget = false;
        //    }
        //}

        //public void UpdateCreatorSquare(int number)
        //{
        //    if (creatorState == CreatorSquare.Selected)
        //    {
        //        this.number = number;
        //        gridLabel.text = number == 0 ? "" : $"{number}";
        //    }
        //}

        //#endregion

        #region Testing only
        //Test
        public void SetupTestGridSquare(int index, int number, bool target)
        {
            this.index = index;
            this.number = number;
            isTarget = target;
        }

        public void UpdateTestGridNumber(int selection)
        {
            number = selection;
        }
        #endregion
    }
}
