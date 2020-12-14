using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Peak.QuixelLogic.Scripts.Game
{
    public sealed class GridLabelManager : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> RowLabels = new List<GameObject>();

        [SerializeField]
        private List<GameObject> ColumnLabels = new List<GameObject>();

        [SerializeField]
        private List<GameObject> ftueArrows = new List<GameObject>();

        public void ClearValues()
        {
            foreach (GameObject go in RowLabels)
            {
                go.GetComponent<GridLabelScript>().ClearAll();
            }

            foreach (GameObject go in ColumnLabels)
            {
                go.GetComponent<GridLabelScript>().ClearAll();
            }
        }

        public void SetGridLabel(Vector2 activeCellCoordinate)
        {
            RowLabels[(int)activeCellCoordinate.x - 1].GetComponent<GridLabelScript>().SetGridLabel((int)activeCellCoordinate.y);
            ColumnLabels[(int)activeCellCoordinate.y - 1].GetComponent<GridLabelScript>().SetGridLabel((int)activeCellCoordinate.x);
        }

        public void HideGridLabelsOnImageReveal()
        {
            foreach (GameObject label in RowLabels)
            {
                label.gameObject.SetActive(false);
            }

            foreach (GameObject label in ColumnLabels)
            {
                label.gameObject.SetActive(false);
            }
        }

        public void ShowSpecificGridLabelsOnFtue(List<int> RowLabelsToShow, List<int> ColumnLabelsToShow)
        {
            // row labels
            foreach (GameObject label in RowLabels)
            {
                label.gameObject.SetActive(false);
            }
            for (int i = 0; i < RowLabelsToShow.Count; i++)
            {
                RowLabels[RowLabelsToShow[i]].SetActive(true);
            }

            // column labels
            foreach (GameObject label in ColumnLabels)
            {
                label.gameObject.SetActive(false);
            }
            for (int i = 0; i < ColumnLabelsToShow.Count; i++)
            {
                ColumnLabels[ColumnLabelsToShow[i]].SetActive(true);
            }
        }

        public void ShowFtueArrows(List<int> ToShow)
        {
            foreach (GameObject arrow in ftueArrows)
            {
                arrow.gameObject.SetActive(false);
            }
            for (int i = 0; i < ToShow.Count; i++)
            {
                ftueArrows[ToShow[i]].SetActive(true);
            }
        }

        public void HideSpecificFtueArrow(int i)
        {
            ftueArrows[i].SetActive(false);
        }
    }
}