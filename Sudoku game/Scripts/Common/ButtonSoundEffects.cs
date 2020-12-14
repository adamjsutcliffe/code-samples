using Peak.Speedoku.Scripts.Settings.Autogenerated;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Peak.Speedoku.Scripts.Common
{
    /// <summary>
    /// Helps to play settings for button down & up events
    /// </summary>
    public sealed class ButtonSoundEffects : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField]
        private SoundSettingsKey soundOnButtonDown;

        [SerializeField]
        private SoundSettingsKey soundOnButtonUp;

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();

            if (!button)
            {
                Debug.LogError("ButtonSoundEffects should be attached to UnityEngine.UI.Button!", this);
                gameObject.SetActive(false);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (button.interactable && soundOnButtonDown != SoundSettingsKey.NotSet)
            {
                LocalisationController.Instance.PlayAudioClip(soundOnButtonDown);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (button.interactable && soundOnButtonDown != SoundSettingsKey.NotSet)
            {
                LocalisationController.Instance.PlayAudioClip(soundOnButtonUp);
            }
        }
    }
}