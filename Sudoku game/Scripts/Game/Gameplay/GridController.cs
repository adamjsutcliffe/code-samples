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
    public struct GridAnswer
    {
        public int index;
        public int number;
        public bool correct;
    }
    public partial class GridController : MonoBehaviour //INFO: MAIN
    {
        [SerializeField] private GameObject tileContainer;
        [SerializeField] private TextMeshProUGUI levelTitle;
        [SerializeField] private KeyboardController keyboardController;
        [SerializeField] private Animator questionMarkAnimator;

        private RuleSettings rules;
        private GridSquareScript[] gridSquares;
        private GridSquareScript currentSelection;
        //private int[] allSquares;
        private List<GridSquareScript> targets;

        private Action<GridAnswer[]> completionCallback;

        #region public API

        public void InitGridWithRules(MainGameData gameData, Action<GridAnswer[]> completion)
        {
            rules = gameData.Ruleset;
            gridSquares = tileContainer.GetComponentsInChildren<GridSquareScript>();
            targets = new List<GridSquareScript>();
            //allSquares = new int[0];
            currentSelection = null;
            completionCallback = completion;
            levelTitle.text = $"LEVEL {gameData.Level}";
            int[] levelData = rules.levelData.Select(x => Int32.Parse(x.ToString())).ToArray();
            print($"Level data count: {levelData.Length}");

            if (levelData.Length == gridSquares.Length)
            {
                for (int i = 0; i < levelData.Length; i++)
                {
                    bool isTarget = rules.targetIndexes.Contains(i);
                    gridSquares[i].SetupGridSquare(i, levelData[i], isTarget, SquareSelected);
                    if (isTarget)
                    {
                        targets.Add(gridSquares[i]);
                    }
                }
            }
            keyboardController.SetKeyboardClickHandler(KeyboardInput);
            keyboardController.SetKeyboardEnabled(false);
            keyboardController.ShowKeyboard();
            if (gameData.answers != null)
            {
                GridAnswer[] errors = gameData.answers.Where(x => x.correct == false).ToArray();
                GridAnswer[] corrects = gameData.answers.Where(x => x.correct == true).ToArray();
                if (errors.Length > 0)
                {
                    StartCoroutine(ShowPreviousErrors(errors, ShowNextTarget));
                    if (corrects.Length == 0)
                    {
                        return;
                    }
                }
                if (corrects.Length > 0)
                {
                    StartCoroutine(ShowPreviousCorrects(corrects));
                    return;
                }
            }
            ShowNextTarget();
        }

        public void SquareSelected(GridSquareScript sender)
        {
            if (!sender.Equals(currentSelection))
            {
                for (int i = 0; i < gridSquares.Length; i++)
                {
                    gridSquares[i].ResetSquare();
                }
                currentSelection = TargetAtIndex(sender.Index);
                HighlightSquare(currentSelection.Index);
                print($"New selection!! {sender.Index}");
            }
        }

        public void KeyboardInput(int number)
        {
#if PLATFORM_IOS
            iOSHapticFeedbackHelper.OnSelection();
#endif
            currentSelection.UpdateGridNumber(number);
            ShowNextTarget();
        }
        #endregion

        private IEnumerator ShowPreviousErrors(GridAnswer[] errors, Action completion)
        {
            for (int i = 0; i < errors.Length; i++)
            {
                GridSquareScript gridSquare = gridSquares[errors[i].index];
                gridSquare.UpdateGridNumber(errors[i].number);
                gridSquare.MarkSquareIncorrect();
            }
            yield return new WaitForSeconds(1.5f);
            for (int i = 0; i < errors.Length; i++)
            {
                GridSquareScript gridSquare = gridSquares[errors[i].index];
                gridSquare.UpdateGridNumber(0);
                gridSquare.ResetSquare();
            }
            yield return new WaitForSeconds(0.25f);
            completion?.Invoke();
        }

        private IEnumerator ShowPreviousCorrects(GridAnswer[] corrects)
        {
            for (int i = 0; i < corrects.Length; i++)
            {
                GridSquareScript gridSquare = gridSquares[corrects[i].index];
                gridSquare.UpdateGridNumber(corrects[i].number);
                gridSquare.MarkSquareCorrect();
            }
            yield return new WaitForSeconds(1.5f);
            for (int i = 0; i < corrects.Length; i++)
            {
                GridSquareScript gridSquare = gridSquares[corrects[i].index];
                gridSquare.ResetSquare();
            }
            yield return new WaitForSeconds(0.25f);
        }

        private void ShowNextTarget()
        {
            if (IsGameComplete())
            {
                //game over
                print("GAME COMPLETE");
                keyboardController.SetKeyboardEnabled(false);
                keyboardController.HideKeyboard();
                for (int i = 0; i < targets.Count; i++)
                {
                    targets[i].ScaleTarget();
                }
                questionMarkAnimator.SetTrigger(Constants.Animation.QuestionMark.ShowQuestionMark);
                LocalisationController.Instance.PlayAudioClip(Settings.Autogenerated.SoundSettingsKey.PopupSound);
                completionCallback?.Invoke(ValidateTargets());
            }
            else
            {
                keyboardController.SetKeyboardEnabled(true);
                SquareSelected(NextTargetSquare());
            }
        }

        private GridAnswer[] ValidateTargets()
        {
            List<GridAnswer> answers = new List<GridAnswer>();
            for (int i = 0; i < targets.Count; i++)
            {
                GridSquareScript target = targets[i];
                bool correct = GridSolver.SolveGridAtIndex(gridSquares, target) != GridSolutionType.None;
                target.MarkSquareComplete(); //MarkSquareIncorrect();
                print($"Target {target.Index} ({target.Number}) is correct? {correct}");
                answers.Add(new GridAnswer
                {
                    index = target.Index,
                    number = target.Number,
                    correct = correct
                });
            }
            return answers.ToArray();
        }

        private GridSquareScript NextTargetSquare()
        {
            return targets.First(x => !x.DoesTargetHaveNumber());
        }

        private GridSquareScript TargetAtIndex(int index)
        {
            return targets.First(x => x.Index == index);
        }

        private bool IsGameComplete()
        {
            return targets.All(x => x.DoesTargetHaveNumber());
        }

        #region UI updates

        public void HighlightSquare(int index)
        {
            HighlightRow(index);
            HighlightColumn(index);
            HighLightBigSquare(index);
        }

        public void HighlightRow(int index)
        {
            int row = GridMaths.RowForSquare(index);
            int[] rowSquares = GridMaths.GridRowIndices(row);
            for (int i = 0; i < rowSquares.Length; i++)
            {
                gridSquares[rowSquares[i]].HighlightSquare(index);
            }
        }

        public void HighlightColumn(int index)
        {
            int col = GridMaths.ColumnForSquare(index);
            int[] colSquares = GridMaths.GridColumnIndices(col);
            for (int i = 0; i < colSquares.Length; i++)
            {
                gridSquares[colSquares[i]].HighlightSquare(index);
            }
        }

        public void HighLightBigSquare(int index)
        {
            int bigSquare = GridMaths.BigSquareForSquare(index);
            int[] bigSquares = GridMaths.GridBigSquareIndices(bigSquare);
            for (int i = 0; i < bigSquares.Length; i++)
            {
                gridSquares[bigSquares[i]].HighlightSquare(index);
            }
        }

        #endregion
    }
}
