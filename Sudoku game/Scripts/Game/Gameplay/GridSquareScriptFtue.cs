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
    public partial class GridSquareScript : MonoBehaviour //INFO: FTUE
    {
        [SerializeField] private FtueSquare ftuePrefab;
        //private FtueSquare instantiatedPrefab;

        public void SetupFtueGridSquare(int index, int number, bool target, Action<GridSquareScript> callback = null)
        {
            this.index = index;
            this.number = number;
            this.callback = callback;
            isTarget = target;
            gridLabel.text = number == 0 ? "" : $"{number}";
            gridLabel.color = settings.labelColour;  //isTarget ? settings.labelTargetColour : settings.labelColour;
            gridLabel.fontSize = settings.fontSize;
            background.color = settings.backgroundColour;  //isTarget ? settings.targetColour : settings.backgroundColour;
            lineBackground.color = settings.lineColour;
            targetButton.interactable = false; //target;
            highlightLineHolder.SetActive(false);
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i].color = settings.lineTargetSelectedColour;
            }
        }

        public void SetAsFtueTarget()
        {
            //isTarget = true;
            //targetButton.interactable = true;
            highlightLineHolder.SetActive(true);
            gridLabel.color = settings.labelTargetColour;
            background.color = settings.targetColour;
        }

        public void ResetFtueTarget()
        {
            //targetButton.interactable = false;
            highlightLineHolder.SetActive(false);
            gridLabel.color = settings.labelColour;
            background.color = settings.backgroundColour;
        }

        public void ShowIgnoreSquare(Transform parent, int number)
        {
            FtueSquare instantiatedPrefab = Instantiate(ftuePrefab, this.gameObject.transform.position, Quaternion.identity, parent);
            instantiatedPrefab.SetupDisabled(number);
        }

        public void ShowCircleSquare(Transform parent)
        {
            FtueSquare instantiatedPrefab = Instantiate(ftuePrefab, this.gameObject.transform.position, Quaternion.identity, parent);
            instantiatedPrefab.SetupHighlight();
        }

        public void ShowSquareArrow(Transform parent, int squareLength, FtueArrowDirection direction)
        {
            FtueSquare instantiatedPrefab = Instantiate(ftuePrefab, this.gameObject.transform.position, Quaternion.identity, parent);
            instantiatedPrefab.SetupArrow(squareLength, direction);
        }

        public void ShowArrowHighlight(Transform parent, FtueArrowDirection direction)
        {
            FtueSquare instantiatedPrefab = Instantiate(ftuePrefab, this.gameObject.transform.position, Quaternion.identity, parent);
            instantiatedPrefab.SetupHighlightArrow(direction);
        }

        //public void ResetFtueSquare()
        //{
        //    Destroy(instantiatedPrefab);
        //    instantiatedPrefab = null;
        //}
    }
}
