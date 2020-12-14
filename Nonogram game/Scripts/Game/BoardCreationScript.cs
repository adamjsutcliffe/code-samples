using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using Peak.QuixelLogic.Scripts.Common;
using Peak.QuixelLogic.Scripts.ScenesLogic;
using Peak.QuixelLogic.Scripts.Settings;
using JetBrains.Annotations;
using Peak.QuixelLogic.Scripts.Common.Localisation;

namespace Peak.QuixelLogic.Scripts.Game
{
    public sealed class BoardCreationScript : MonoBehaviour
    {
        [Tooltip("Tick to edit grid size... Untick when grid size is decided")]
        [SerializeField]
        private bool editableGridSize;

        private int gridSize;
        public int GridSize => gridSize;

        [SerializeField]
        private string levelString;

        [SerializeField]
        private TextMeshProUGUI puzzleNameText;

        [SerializeField]
        private List<GameObject> rowCells_Picture = new List<GameObject>();

        [SerializeField]
        private List<GameObject> gridCells_Picture = new List<GameObject>();

        [SerializeField]
        private List<GameObject> rowCells_TouchCell = new List<GameObject>();

        [SerializeField]
        private List<CellScript> gridCells_TouchCells = new List<CellScript>();

        private List<GameObject> activeCells = new List<GameObject>(100);

        public event Action ClearTargetCells;
        public event Action<Vector2> SendTargetCell;

        private Dictionary<string, Color32> CellColourDictionary = new Dictionary<string, Color32>();

        [SerializeField]
        private GridLabelManager gridLabelManager;
        public GridLabelManager GridLabelManager => gridLabelManager;

        [SerializeField]
        private GridLineScript gridLineScript;

        private ToolbarUIActivator toolbarUIActivator;

        [SerializeField]
        private Animator ftueHighlightAnimator;

        [SerializeField]
        private Animator boardPrefabAnimator;

        [SerializeField]
        private Animator puzzleImageAnimator;

        [SerializeField]
        private Text boardDebugText;

#if PREVIEWINEDITOR
        public void OnValidate()
        {
            InitiateBoard(levelString);
        }
#endif

        public void InitiateBoard(RuleSettings ruleSettings)
        {
            boardDebugText.text = SceneActivationBehaviour<GameLogicActivator>.Instance.GameController.DisplayDebugText ? ruleSettings.Id + ", " + LocalisationSystem.GetLocalisedValue(ruleSettings.PuzzleNameKey) : "";

            puzzleNameText.text = LocalisationSystem.GetLocalisedValue(ruleSettings.PuzzleNameKey);

            levelString = ruleSettings.LevelString;

            activeCells.Clear();
            gridLabelManager.ClearValues();

            CellColourDictionary.Clear();
            CellColourDictionary.Add("x".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_x);
            CellColourDictionary.Add("0".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_0);
            CellColourDictionary.Add("1".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_1);
            CellColourDictionary.Add("2".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_2);
            CellColourDictionary.Add("3".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_3);
            CellColourDictionary.Add("4".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_4);
            CellColourDictionary.Add("5".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_5);
            CellColourDictionary.Add("6".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_6);
            CellColourDictionary.Add("7".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_7);
            CellColourDictionary.Add("8".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_8);
            CellColourDictionary.Add("9".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_9);
            CellColourDictionary.Add("a".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_a);
            CellColourDictionary.Add("b".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_b);
            CellColourDictionary.Add("c".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_c);
            CellColourDictionary.Add("d".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_d);
            CellColourDictionary.Add("e".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_e);
            CellColourDictionary.Add("f".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_f);

            if (levelString != null)
            {
                gridSize = ((int)Mathf.Sqrt(levelString.Length));
                gridLineScript.SetGridLines(gridSize);
            }

            if (editableGridSize)
            {
                ShowAllGrid();
                ShowValidPictureCells();
            }

            ClearTargetCells?.Invoke();

            GatherActiveGridCells();

            toolbarUIActivator = SceneActivationBehaviour<ToolbarUIActivator>.Instance;
            toolbarUIActivator.PassBoardCreationScript(this);

            //SetModeToPencil();

            SceneActivationBehaviour<ToolbarUIActivator>.Instance.ToggleToPaint();

            ftueHighlightAnimator.gameObject.SetActive(false);
        }

        public void SetModeToMarker()
        {
            SoundController.Instance.PlayAudioClip(Settings.Autogenerated.SoundSettingsKey.Toolmarkselected);

            foreach (CellScript cell in gridCells_TouchCells)
            {
                cell.SetDrawMode(true);
            }

            //toolbarUIActivator.OnSetModeToMarker -= SetModeToMarker;
            //toolbarUIActivator.OnSetModeToPencil += SetModeToPencil;
        }

        public void SetModeToPencil()
        {
            if (SceneActivationBehaviour<GameLogicActivator>.Instance.SessionScript.IsInGame)
            {
                SoundController.Instance.PlayAudioClip(Settings.Autogenerated.SoundSettingsKey.Toolpaintselected);
            }

            foreach (CellScript cell in gridCells_TouchCells)
            {
                cell.SetDrawMode(false);
            }

            //toolbarUIActivator.OnSetModeToMarker += SetModeToMarker;
            //toolbarUIActivator.OnSetModeToPencil -= SetModeToPencil;
        }

        public void QuitGame(Action action)
        {
            //toolbarUIActivator.OnSetModeToMarker -= SetModeToMarker;
            //toolbarUIActivator.OnSetModeToPencil -= SetModeToPencil;

            //SceneActivationBehaviour<ToolbarUIActivator>.Instance.ToggleToPaint();

            //toolbarUIActivator.ClickPencil();
            //toolbarUIActivator.SetModeToPencil();

            action?.Invoke();
        }

        private void ShowAllGrid()
        {
            // get all grid cells on all columns, set all to active
            foreach (GameObject row in rowCells_Picture)
            {
                row.gameObject.SetActive(true);
            }

            foreach (GameObject row in rowCells_TouchCell)
            {
                row.gameObject.SetActive(true);
            }

            foreach (GameObject cell in gridCells_Picture)
            {
                cell.gameObject.SetActive(true);
            }

            foreach (CellScript cell in gridCells_TouchCells)
            {
                cell.gameObject.SetActive(true);
            }
        }

        private void ShowValidPictureCells()
        {
            // check grid and column numbers against defined grid size for picture
            foreach (GameObject row in rowCells_Picture)
            {
                RectTransform[] gridChildren = GetComponentsInChildren<RectTransform>();
                foreach (RectTransform gridChild in gridChildren)
                {
                    int i = 0;
                    bool childResult = int.TryParse(gridChild.gameObject.name, out i);
                    if (childResult)
                    {
                        i = int.Parse(gridChild.gameObject.name);
                    }

                    if (i > gridSize)
                    {
                        gridChild.gameObject.SetActive(false);
                    }
                    else gridChild.gameObject.SetActive(true);
                }

                int j = 0;
                bool parentResult = int.TryParse(row.gameObject.name, out j);
                if (parentResult)
                {
                    j = int.Parse(row.gameObject.name);
                }

                if (j > gridSize)
                {
                    row.gameObject.SetActive(false);
                }
                else row.gameObject.SetActive(true);

                Array.Clear(gridChildren, 0, gridChildren.Length);
            }

            // check grid and column numbers against defined grid size for touch cells
            foreach (GameObject row in rowCells_TouchCell)
            {
                RectTransform[] gridChildren = GetComponentsInChildren<RectTransform>();
                foreach (RectTransform gridChild in gridChildren)
                {
                    int i = 0;
                    bool childResult = int.TryParse(gridChild.gameObject.name, out i);
                    if (childResult)
                    {
                        i = int.Parse(gridChild.gameObject.name);
                    }

                    if (i > gridSize)
                    {
                        gridChild.gameObject.SetActive(false);
                    }
                    else
                    {
                        gridChild.gameObject.SetActive(true);
                    }
                }

                int j = 0;
                bool parentResult = int.TryParse(row.gameObject.name, out j);
                if (parentResult)
                {
                    j = int.Parse(row.gameObject.name);
                }

                if (j > gridSize)
                {
                    row.gameObject.SetActive(false);
                }
                else row.gameObject.SetActive(true);

                Array.Clear(gridChildren, 0, gridChildren.Length);
            }
        }

        private void GatherActiveGridCells()
        {
            foreach (GameObject cell in gridCells_Picture)
            {
                if (cell.activeInHierarchy)
                {
                    activeCells.Add(cell);
                }
            }

            for (int i = 0; i < activeCells.Count; i++)
            {
                SetCellColour(levelString.Substring(i, 1), i);
            }
        }

        private void SetCellColour(string ColourReference, int CellNumber)
        {
            activeCells[CellNumber].GetComponent<Image>().color = CellColourDictionary[ColourReference];

            if (ColourReference != "X")
            {
                RecordActiveCellsPositions(activeCells[CellNumber]);
            }
        }

        private void RecordActiveCellsPositions(GameObject activeCell)
        {
            SendTargetCell?.Invoke(activeCell.GetComponent<CellLocationScript>().cellCoordinates);

            gridLabelManager.SetGridLabel(activeCell.GetComponent<CellLocationScript>().cellCoordinates);
        }

        public void DisableTouchGrid()
        {
            foreach (CellScript gridCell in gridCells_TouchCells)
            {
                gridCell.DisableTouch();
            }
        }

        [SerializeField]
        private GameObject poloroidBackground;

        public void HideLabels()
        {
            gridLabelManager.HideGridLabelsOnImageReveal();

            poloroidBackground.SetActive(true);

            foreach (CellScript gridCell in gridCells_TouchCells)
            {
                gridCell.DisableAnimator();
            }

            Invoke(nameof(TriggerBoardPrefab), 0.75f);
        }

        private void TriggerBoardPrefab()
        {
            boardPrefabAnimator.SetTrigger("RevealPicture");
        }

        [UsedImplicitly] // by board prefab animator
        public void RevealImage()
        {
            puzzleImageAnimator.SetTrigger("RevealPicture");
        }

        public void HideImage()
        {
            //puzzleImageAnimator.SetTrigger("Hide");
        }

        public void DeliverHint(Vector2 hintCell)
        {
            foreach (CellScript gridCell in gridCells_TouchCells)
            {
                gridCell.TrySetCellAsHint(hintCell);
            }


        }

        public void BlockCellsToBeSelected(List<Vector2> cells, bool fadeAnim = true)
        {
            foreach (CellScript gridCell in gridCells_TouchCells)
            {
                gridCell.FtueBlock(true);

                for (int i = 0; i < cells.Count; i++)
                {
                    if (cells.Contains(gridCell.GetCoordinate()))
                    {
                        gridCell.FtueBlock(false);
                        gridCell.FadeAnimation(fadeAnim);
                    }
                }
            }
        }

        public void HighlightCells(bool show, string animation = null)
        {
            if (show)
            {
                ftueHighlightAnimator.gameObject.SetActive(true);
                ftueHighlightAnimator.SetTrigger(animation);
            }
            else
            {
                ftueHighlightAnimator.SetTrigger("Stop");
                ftueHighlightAnimator.gameObject.SetActive(false);
            }
        }

        public void UnblockAllCells()
        {
            foreach (CellScript gridCell in gridCells_TouchCells)
            {
                gridCell.FtueBlock(false);
                gridCell.FadeAnimation(false);
            }
        }

        public void BlockAllCells()
        {
            foreach (CellScript gridCell in gridCells_TouchCells)
            {
                gridCell.FtueBlock(true);
            }
        }
    }
}