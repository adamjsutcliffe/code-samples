using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Peak.Speedoku.Scripts.Common
{
    /// <summary>
    /// Provides extended behaviour for UI graphics tint
    /// </summary>
    public class GraphicsColourTint : MonoBehaviour
    {
        [SerializeField]
        protected Color imageTintValue = new Color(0.78125f, 0.78125f, 0.78125f, 0.5f);
        [SerializeField]
        protected Color textTintValue = new Color(0.78125f, 0.78125f, 0.78125f, 0.5f);

        [SerializeField]
        protected Image[] images;

        [SerializeField]
        protected TextMeshProUGUI[] texts;

        [Tooltip("Buttons, sliders or other selectables")]
        [SerializeField]
        protected Selectable[] selectables;

        private Color[] imageColors;
        private Color[] textColors;
        private bool[] selectableFlags;

        private bool isEnabled;
        private bool isInitialized;

        public void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (isInitialized)
            {
                return;
            }

            RememberValues();

            isEnabled = gameObject.activeSelf;
            isInitialized = true;
        }

        private void GatherGraphicsReferences()
        {
            if (images == null || images.Length == 0)
            {
                images = GetComponentsInChildren<Image>(true);
            }

            if (texts == null || texts.Length == 0)
            {
                texts = GetComponentsInChildren<TextMeshProUGUI>(true);
            }

            if (selectables == null || selectables.Length == 0)
            {
                selectables = GetComponentsInChildren<Selectable>(true);
            }
        }

        private void RememberValues()
        {
            //  Turns tint on/off for custom graphics
            if (images != null)
            {
                imageColors = new Color[images.Length];

                for (int i = 0; i < images.Length; i++)
                {
                    Image image = images[i];
                    if (image != null)
                    {
                        imageColors[i] = images[i].color;
                    }
                }
            }

            // Turns tint on/off for custom text
            if (texts != null)
            {
                textColors = new Color[texts.Length];

                for (var i = 0; i < texts.Length; i++)
                {
                    TextMeshProUGUI label = texts[i];

                    if (label != null)
                    {
                        textColors[i] = label.color;
                    }
                }
            }
        }

        public void SetEnabled(bool isEnabled)
        {
            gameObject.SetActive(isEnabled);

            SetInteractability(isEnabled);
        }

        public void SetInteractability(bool isInteractable)
        {
            //Debug.LogError($"SET {gameObject.name} := {isInteractable}", gameObject);

            // Turns tint on/off for custom text
            if (texts != null)
            {
                for (var i = 0; i < texts.Length; i++)
                {
                    TextMeshProUGUI label = texts[i];

                    if (label != null)
                    {
                        //Debug.LogError($"SET selectable {label.gameObject.name} := {isInteractable})", label.gameObject);
                        label.color = isInteractable ? textColors[i] : textColors[i] * textTintValue;
                    }
                }
            }

            //  Turns tint on/off for custom graphics
            if (images != null)
            {
                for (int i = 0; i < images.Length; i++)
                {
                    Image image = images[i];

                    if (image != null)
                    {
                        image.color = isInteractable ? imageColors[i] : imageColors[i] * imageTintValue;
                    }
                }
            }

            //  Turns tint on/off for custom graphics
            if (selectables != null)
            {
                for (int i = 0; i < selectables.Length; i++)
                {
                    Selectable selectable = selectables[i];

                    if (selectable != null)
                    {
                        //Debug.LogError($"SET selectable {selectable.gameObject.name} := {isInteractable}", selectable.gameObject);
                        selectable.interactable = isInteractable;
                    }
                }
            }

            isEnabled = isInteractable;
        }

#if UNITY_EDITOR

        private bool flag = true;

        [ContextMenu("Toggle Interactability ON/OFF")]
        private void PreviewDisabled()
        {
            SetInteractability(flag = flag ^ true);
        }

        [ContextMenu("Gather Tint references")]
        private void GatherReferences()
        {
            GatherGraphicsReferences();
        }

#endif
    }
}