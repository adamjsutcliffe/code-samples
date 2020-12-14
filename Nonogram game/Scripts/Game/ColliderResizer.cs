using System.Collections;
using System.Collections.Generic;
using Peak.QuixelLogic.Scripts.ScenesLogic;
using UnityEngine;

namespace Peak.QuixelLogic.Scripts.Game
{
    public sealed class ColliderResizer : MonoBehaviour
    {
        private BoxCollider2D BoxCollider;

        private CellScript cellScript;

#if PREVIEWINEDITOR
        public void OnValidate()
        {
            SetSize();
        }
#endif

        public void Start()
        {
            cellScript = GetComponent<CellScript>();
            Invoke(nameof(SetSize), 0.1f);
        }

        private void SetSize()
        {
            if (isActiveAndEnabled)
            {
                BoxCollider = GetComponent<BoxCollider2D>();

                float sizeX = GetComponent<RectTransform>().sizeDelta.x;
                float sizeY = GetComponent<RectTransform>().sizeDelta.y;

                if (cellScript.CellCoordinates.y.Equals(1))
                {
                    if (cellScript.CellCoordinates.x.Equals(1))
                    {
                        BoxCollider.size = new Vector2(sizeX * 2, sizeY * 2);
                        BoxCollider.offset = new Vector2(-(sizeX / 2), sizeY / 2);
                    }
                    else
                    {
                        BoxCollider.size = new Vector2(sizeX * 2, sizeY);
                        BoxCollider.offset = new Vector2(-(sizeX / 2), 0);
                    }
                }
                else if (cellScript.CellCoordinates.y.Equals(SceneActivationBehaviour<BoardUIActivator>.Instance.GetGridSize()))
                {
                    if (cellScript.CellCoordinates.x.Equals(1))
                    {
                        BoxCollider.size = new Vector2(sizeX * 2, sizeY * 2);
                        BoxCollider.offset = new Vector2(sizeX / 2, sizeY / 2);
                    }
                    else
                    {
                        BoxCollider.size = new Vector2(sizeX * 2, sizeY);
                        BoxCollider.offset = new Vector2(sizeX / 2, 0);
                    }
                }
                else if (cellScript.CellCoordinates.x.Equals(1))
                {
                    BoxCollider.size = new Vector2(sizeX, sizeY * 2);
                    BoxCollider.offset = new Vector2(0, sizeY / 2);
                }
                else BoxCollider.size = new Vector2(sizeX, sizeY);
            }
        }
    }
}