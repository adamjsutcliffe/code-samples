using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Peak.QuixelLogic.Scripts.Game
{
    public sealed class GridLineScript : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> gridLines = new List<GameObject>();

        public void SetGridLines(int gridSize)
        {
            int i = gridSize - 5;

            gridLines[i].SetActive(true);
        }
    }
}