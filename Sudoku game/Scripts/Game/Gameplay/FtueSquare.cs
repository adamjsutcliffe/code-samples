using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Peak.Speedoku.Scripts.Game.Gameplay
{
    public enum FtueArrowDirection
    {
        Up = 0,
        Left = 1,
        Down = 2,
        Right = 3
    }

    public class FtueSquare : MonoBehaviour
    {
        [SerializeField] private GameObject disabledImage;
        [SerializeField] private GameObject highlightImage;
        [SerializeField] private GameObject lineHolder;
        [SerializeField] private RectTransform lineTransform;
        [SerializeField] private GameObject arrowHolder;
        [SerializeField] private TextMeshProUGUI disabledNumber;

        public void SetupDisabled(int number)
        {
            disabledImage.gameObject.SetActive(true);
            highlightImage.gameObject.SetActive(false);
            lineTransform.gameObject.SetActive(false);
            arrowHolder.gameObject.SetActive(false);
            disabledNumber.text = $"{number}";
        }

        public void SetupHighlight()
        {
            disabledImage.gameObject.SetActive(false);
            highlightImage.gameObject.SetActive(true);
            lineTransform.gameObject.SetActive(false);
            arrowHolder.gameObject.SetActive(false);
        }

        public void SetupArrow(int arrowLength, FtueArrowDirection direction)
        {
            disabledImage.gameObject.SetActive(false);
            highlightImage.gameObject.SetActive(false);
            lineTransform.gameObject.SetActive(true);
            arrowHolder.gameObject.SetActive(false);
            float lineLength = lineTransform.rect.size.y + (70 * (arrowLength - 1));
            lineTransform.sizeDelta = new Vector2(lineTransform.rect.size.x, lineLength);
            Vector3 rotation = new Vector3(0, 0, 90 *(int)direction);
            lineHolder.transform.Rotate(rotation);
        }

        public void SetupHighlightArrow(FtueArrowDirection direction)
        {
            disabledImage.gameObject.SetActive(false);
            highlightImage.gameObject.SetActive(false);
            lineTransform.gameObject.SetActive(false);
            arrowHolder.gameObject.SetActive(true);
            Vector3 rotation = new Vector3(0, 0, 90 * (int)direction);
            arrowHolder.transform.Rotate(rotation);
        }
    }
}
