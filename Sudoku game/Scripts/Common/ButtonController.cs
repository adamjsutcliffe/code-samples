﻿using System.Collections.Generic;
using Peak.Speedoku.Scripts.Settings.Autogenerated;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Peak.Speedoku.Scripts.Common
{
    /// <summary>
    /// Helps to play settings for button down & up events
    /// </summary>
    public sealed class ButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
    {
        [SerializeField]
        private SoundSettingsKey soundOnButtonDown;

        [SerializeField]
        private SoundSettingsKey soundOnButtonUp;

        private Button button;

        [SerializeField]
        private List<GameObject> buttonChildren = new List<GameObject>();

        private bool pointerOnButton;
        private bool pointerDown;

        [SerializeField]
        private float iconActiveOffset = 10f;

        private void Awake()
        {
            button = GetComponent<Button>();

            if (transform.childCount > 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    buttonChildren.Add(transform.GetChild(i).gameObject);
                }
            }

            if (!button)
            {
                Debug.LogError("ButtonController should be attached to UnityEngine.UI.Button!", this);
                gameObject.SetActive(false);
            }
        }

        public void SetInteractability(bool interactable)
        {
            if (button != null)
            {
                button.interactable = interactable;
            }
            else return;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            pointerOnButton = true;

            if (pointerDown)
            {
                OffsetTextOrIcon();
                if (soundOnButtonDown != SoundSettingsKey.NotSet)
                {
                    LocalisationController.Instance.PlayAudioClip(soundOnButtonDown);
                }
#if PLATFORM_IOS
                iOSHapticFeedbackHelper.OnSelection();
#endif
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            pointerOnButton = false;

            if (pointerDown && button.interactable)
            {
                ResetTextOrIcon();
#if PLATFORM_IOS
                iOSHapticFeedbackHelper.OnSelection();
#endif
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            pointerDown = true;

            if (button.interactable)
            {
                OffsetTextOrIcon();

                if (soundOnButtonDown != SoundSettingsKey.NotSet)
                {
                    LocalisationController.Instance.PlayAudioClip(soundOnButtonDown);
                }
#if PLATFORM_IOS
                iOSHapticFeedbackHelper.OnSelection();
#endif
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            pointerDown = false;

            if (button.interactable)
            {
                if (pointerOnButton)
                {
                    ResetTextOrIcon();
#if PLATFORM_IOS
                    iOSHapticFeedbackHelper.OnSuccess();
#endif
                }

                if (soundOnButtonDown != SoundSettingsKey.NotSet && pointerOnButton)
                {
                    LocalisationController.Instance.PlayAudioClip(soundOnButtonUp);
                }
            }
        }

        private void OffsetTextOrIcon()
        {
            if (buttonChildren != null)
            {
                foreach (GameObject child in buttonChildren)
                {
                    child.transform.localPosition = new Vector3(child.transform.localPosition.x, child.transform.localPosition.y - iconActiveOffset, child.transform.localPosition.z);
                }
            }
        }

        private void ResetTextOrIcon()
        {
            if (buttonChildren != null)
            {
                foreach (GameObject child in buttonChildren)
                {
                    child.transform.localPosition = new Vector3(child.transform.localPosition.x, child.transform.localPosition.y + iconActiveOffset, child.transform.localPosition.z);
                }
            }
        }
    }
}