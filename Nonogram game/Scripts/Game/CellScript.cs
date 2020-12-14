using System.Collections;
using System.Collections.Generic;
using Peak.QuixelLogic.Scripts.Common;
using Peak.QuixelLogic.Scripts.ScenesLogic;
using Peak.QuixelLogic.Scripts.Game.Gameplay;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Peak.QuixelLogic.Scripts.Game
{
    public sealed class CellScript : MonoBehaviour
    {
        [SerializeField]
        private Vector2 cellCoordinates;
        public Vector2 CellCoordinates
        {
            get { return cellCoordinates; }
        }

        private bool isSelected;

        private bool isBlocked;

        private bool isFtueBlocked;

        private CellSelectionScript cellSelectionScript;

        private GameplayScript gameplayScript;

        private Animator animator;

        public bool blockMode;

        private void OnEnable()
        {
            cellCoordinates = new Vector2(int.Parse(transform.parent.name), int.Parse(name));
        }

        private void Awake()
        {
            cellSelectionScript = transform.parent.parent.gameObject.GetComponent<CellSelectionScript>();
            gameplayScript = transform.parent.parent.parent.gameObject.GetComponent<GameplayScript>();
            animator = GetComponent<Animator>();
        }

        public Vector2 GetCoordinate()
        {
            return cellCoordinates;
        }

        public void FtueBlock(bool block)
        {
            isFtueBlocked = block;
        }

        public void FadeAnimation(bool active)
        {
            if (active)
            {
                animator?.SetTrigger("StartFadeCycle");
            }
            else
            {
                if (animator.isActiveAndEnabled)
                {
                    animator.SetTrigger("EndFadeCycle");
                }
            }
        }

        public void SetDrawMode(bool mode)
        {
            blockMode = mode;
        }

        private void OnMouseDown()
        {
            if (!isFtueBlocked)
            {
                if (!isSelected)
                {
                    cellSelectionScript.SelectionMode = true;
                }
                else cellSelectionScript.SelectionMode = false;

                if (!isBlocked)
                {
                    cellSelectionScript.MarkMode = true;
                }
                else cellSelectionScript.MarkMode = false;

                OnMouseEnter();
            }
        }

        private void OnMouseUp()
        {
            cellSelectionScript.IncrementSelectionChain(0);
            cellSelectionScript.IncrementMarkChain(0);
        }

        private void OnMouseEnter()
        {
            if (!blockMode)
            {
                if (cellSelectionScript.SelectionMode)
                {
                    if (!isSelected && cellSelectionScript.MouseDown)
                    {
                        SelectCell();
                    }
                }
                else if (!cellSelectionScript.SelectionMode)
                {
                    if (isSelected && cellSelectionScript.MouseDown)
                    {
                        DeselectCell();
                    }
                }
            }

            if (blockMode && !isFtueBlocked)
            {
                if (cellSelectionScript.MarkMode)
                {
                    if (!isBlocked && cellSelectionScript.MouseDown)
                    {
                        BlockCell();
                    }
                }
                else if (!cellSelectionScript.MarkMode)
                {
                    if (isBlocked && cellSelectionScript.MouseDown)
                    {
                        UnblockCell();
                    }
                }
            }
        }

        private void SelectCell()
        {
            if (!isFtueBlocked)
            {
                if (!isBlocked)
                {
                    cellSelectionScript.SaveSelectedCellCoordinates(cellCoordinates, () =>
                    {
                        isSelected = true;
                        animator?.SetTrigger("Paint");
                        iOSHapticFeedback.OnSelection();
                        cellSelectionScript.IncrementSelectionChain(1);
                    });
                }
            }
        }

        private void DeselectCell()
        {
            if (!isFtueBlocked && !gameplayScript.IsFtue)
            {
                if (!isBlocked)
                {
                    cellSelectionScript.UnSaveSelectedCellCoordinates(cellCoordinates, () =>
                    {
                        isSelected = false;
                        animator?.SetTrigger("Unpaint");
                        iOSHapticFeedback.OnSelection();
                        cellSelectionScript.IncrementSelectionChain(1, false);
                    });
                }
            }
        }

        private void BlockCell()
        {
            if (!isFtueBlocked)
            {
                cellSelectionScript.MarkCell(cellCoordinates, () =>
                {
                    if (!isSelected)
                    {
                        isBlocked = true;
                        animator?.SetTrigger("Mark");
                        iOSHapticFeedback.OnSelection();
                        cellSelectionScript.IncrementMarkChain(1);
                    }
                });
            }
        }

        private void UnblockCell()
        {
            if (!isFtueBlocked && !gameplayScript.IsFtue)
            {
                cellSelectionScript.MarkCell(cellCoordinates, () =>
                {
                    isBlocked = false;
                    animator?.SetTrigger("Unmark");
                    iOSHapticFeedback.OnSelection();
                    cellSelectionScript.IncrementMarkChain(1, false);
                });
            }
        }

        public void TrySetCellAsHint(Vector2 hintCoordinates)
        {
            if (cellCoordinates.Equals(hintCoordinates))
            {
                if (isBlocked)
                {
                    UnblockCell();
                }

                isSelected = true;

                SceneActivationBehaviour<BoardUIActivator>.Instance.QuixelController.SpawnHint(gameObject);

                cellSelectionScript.SaveSelectedCellCoordinates(cellCoordinates);

                BoxCollider2D BoxCollider = GetComponent<BoxCollider2D>();
                BoxCollider.enabled = false;
            }
            else return;
        }

        public void DisableAnimator()
        {
            animator.enabled = false;
        }

        public void DisableTouch()
        {
            BoxCollider2D BoxCollider = GetComponent<BoxCollider2D>();
            BoxCollider.enabled = false;
        }
    }
}