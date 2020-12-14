using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Peak.QuixelLogic.Scripts.Game
{
    [ExecuteInEditMode]
    public sealed class CellLocationScript : MonoBehaviour
    {
        public Vector2 cellCoordinates;

#if PREVIEWINEDITOR
        public void OnValidate()
        {
            cellCoordinates = new Vector2(int.Parse(transform.parent.name), int.Parse(name));
        }
#endif
    }
}