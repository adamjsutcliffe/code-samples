using UnityEngine;
using UnityEngine.UI;

namespace Peak.Speedoku.Scripts.Common.Extensions
{
    public static class ButtonExtensions
    {
        /// <summary>
        /// Checks Button for ButtonTextColourTint script and enables it (active GO & interactable)
        /// </summary>
        public static void SetEnabled(this Button self, bool isEnabled)
        {
            ButtonTextColourTint buttonTint = self.GetComponent<ButtonTextColourTint>();
            if (buttonTint)
            {
                buttonTint.SetEnabled(isEnabled);
            }
            else
            {
                Debug.LogWarning($"{self.gameObject.name} has NO '{nameof(ButtonTextColourTint)}' script");

                if (!self.enabled)
                {
                    self.gameObject.SetActive(isEnabled);
                }

                self.interactable = isEnabled;
            }
        }

        /// <summary>
        /// Checks Button for ButtonTextColourTint script and sets interactable
        /// </summary>
        public static void SetInteractability(this Button self, bool isInteractable)
        {
            ButtonTextColourTint buttonTint = self.GetComponent<ButtonTextColourTint>();

            if (buttonTint)
            {
                buttonTint.SetInteractability(isInteractable);
            }
            else
            {
                Debug.LogWarning($"{self.gameObject.name} has NO '{nameof(ButtonTextColourTint)}' script");

                self.interactable = isInteractable;
            }
        }
    }
}