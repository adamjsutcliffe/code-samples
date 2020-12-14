using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Peak.Speedoku.Scripts.Common;

namespace Peak.Speedoku.Scripts.Game.Gameplay
{
    public class KeyboardController : MonoBehaviour
    {
        [Header("For FTUE")]
        [SerializeField] private FtueSquare ftuePrefab;
        [SerializeField] private Transform ftueParent;
        [SerializeField] private GameObject ftuePositioner;
        [SerializeField] private Animator keyboardAnimator;

        [Header("For Creator")]
        [SerializeField] GridController grid;

        private Button[] buttons;

        private void Awake()
        {
            //Collect buttons
            buttons = gameObject.GetComponentsInChildren<Button>();
            print($"Keyboard -> got {buttons.Length} buttons");
        }

        public void SetKeyboardEnabled(bool enabled)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].interactable = enabled;
            }
        }

        public void SetKeyboardClickHandler(Action<int> action)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                Button button = buttons[i];
                button.onClick.RemoveAllListeners();
                KeyButtonScript key = button.GetComponent<KeyButtonScript>();
                button.onClick.AddListener(() => action(key.KeyNumber));
            }
        }

        public void KeyboardPressed(KeyButtonScript sender)
        {
            print($"Sender - {sender.KeyNumber}");
            grid.CreatorUpdateSquareWithNumber(sender.KeyNumber);
        }


        #region - FTUE helpers

        public void HighlightKeyboard()
        {
            StartCoroutine(ShowHighlightAfterDelay());
        }

        private IEnumerator ShowHighlightAfterDelay()
        {
            yield return new WaitForSeconds(0.25f);
            FtueSquare instantiatedPrefab = Instantiate(ftuePrefab, ftuePositioner.transform.position, Quaternion.identity, ftueParent);
            instantiatedPrefab.SetupHighlightArrow(FtueArrowDirection.Left);
        }

        public void ShowKeyboard()
        {
            keyboardAnimator.SetTrigger(Constants.Animation.Keyboard.ShowKeyboard);
            SetKeyboardEnabled(true);
        }


        public void HideKeyboard()
        {
            SetKeyboardEnabled(false);
            keyboardAnimator.SetTrigger(Constants.Animation.Keyboard.HideKeyboard);
        }
        #endregion
    }
}
