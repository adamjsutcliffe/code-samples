using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Peak.Speedoku.Scripts.Settings;
using UnityEngine;
using TMPro;
using Peak.Speedoku.Scripts.Common;

namespace Peak.Speedoku.Scripts.Game.Gameplay
{
    public partial class GridController : MonoBehaviour //INFO: CREATOR
    {
        public void InitCreatorGridWithRules(RuleSettings ruleSettings, Action<GridAnswer[]> completion)
        {
            rules = ruleSettings;
            gridSquares = tileContainer.GetComponentsInChildren<GridSquareScript>();
            targets = new List<GridSquareScript>();
            //allSquares = new int[0];
            currentSelection = null;
            completionCallback = completion;
            int[] levelData = rules.levelData.Select(x => Int32.Parse(x.ToString())).ToArray();
            if (levelData.Length == gridSquares.Length)
            {
                for (int i = 0; i < levelData.Length; i++)
                {
                    gridSquares[i].SetupCreatorGridSquare(i, levelData[i], CreatorSquareSelected);
                }
            }
        }

        private void CreatorSquareSelected(GridSquareScript sender)
        {
            if (currentSelection != null && !currentSelection.Equals(sender))
            {
                currentSelection.DeselectCreatorSquare();
            }
            currentSelection = gridSquares[sender.Index];
        }

        public void CreatorUpdateSquareWithNumber(int number)
        {
            if (currentSelection != null)
            {
                currentSelection.UpdateCreatorSquare(number);
            }
        }

        public string GridString()
        {
            int[] stringNumbers = gridSquares.Select(x => x.Number).ToArray();
            return string.Join("", stringNumbers);
        }

        public int[] GridTargets()
        {
            int[] targetIndices = gridSquares.Where(x => x.IsTarget).Select(x => x.Index).ToArray();
            return targetIndices;
        }
    }
}
