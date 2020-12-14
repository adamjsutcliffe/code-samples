using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Peak.Speedoku.Scripts.Settings;
using UnityEngine;
using TMPro;
using Peak.Speedoku.Scripts.Common;
using Peak.Speedoku.Scripts.ScenesLogic;

namespace Peak.Speedoku.Scripts.Game.Gameplay
{
    public partial class GridController : MonoBehaviour //INFO: FTUE
    {
        [SerializeField] private Transform ftueHolder;

        public void InitFtueGridWithRules(MainGameData gameData, Action<GridAnswer[]> completion)
        {
            rules = gameData.Ruleset;
            gridSquares = tileContainer.GetComponentsInChildren<GridSquareScript>();
            targets = new List<GridSquareScript>();
            currentSelection = null;
            completionCallback = completion;
            levelTitle.text = $"TUTORIAL LEVEL";
            int[] levelData = rules.levelData.Select(x => Int32.Parse(x.ToString())).ToArray();
            print($"Level data count: {levelData.Length}");

            if (levelData.Length == gridSquares.Length)
            {
                for (int i = 0; i < levelData.Length; i++)
                {
                    bool isTarget = rules.targetIndexes.Contains(i);
                    gridSquares[i].SetupFtueGridSquare(i, levelData[i], isTarget, SquareSelected);
                    if (isTarget)
                    {
                        targets.Add(gridSquares[i]);
                    }
                }
            }
            keyboardController.SetKeyboardEnabled(false);
            SceneActivationBehaviour<GameLogicActivator>.Instance.GameController.StartFtueGame(gameData, this, keyboardController);
        }

        public GridSquareScript GetFtueTargetAtIndex(int index)
        {
            GridSquareScript target = TargetAtIndex(index);
            target.SetAsFtueTarget();
            return target;
        }

        public void ResetFtueGrid()
        {
            for (int i = 0; i < gridSquares.Length; i++)
            {
                gridSquares[i].ResetFtueTarget();
            }
        }

        public bool ValidateFtueTarget(GridSquareScript target)
        {
            return GridSolver.SolveGridAtIndex(gridSquares, target) != GridSolutionType.None;
        }

        public void ShowArrowHighlight(int index, FtueArrowDirection direction)
        {
            gridSquares[index].ShowArrowHighlight(ftueHolder, direction);
        }

        public void ShowCircleHighlight(int index)
        {
            gridSquares[index].ShowCircleSquare(ftueHolder);
        }

        public void ShowCircleArrowHighlight(int index, int length, FtueArrowDirection direction)
        {
            gridSquares[index].ShowSquareArrow(ftueHolder, length, direction);
        }

        public void ShowIgnoreHighlight(int index, int number)
        {
            gridSquares[index].ShowIgnoreSquare(ftueHolder, number);
        }

        public void ClearFtueHighlights()
        {
            foreach (Transform child in ftueHolder.transform)
            {
                Destroy(child.gameObject);
            }
        }

        public void CompleteFtueGame()
        {
            completionCallback?.Invoke(ValidateTargets());
        }
    }
}
