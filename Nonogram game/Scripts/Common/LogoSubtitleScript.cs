using System.Collections;
using System.Collections.Generic;
using Peak.QuixelLogic.Scripts.Common.Localisation;
using UnityEngine;
using UnityEngine.UI;

namespace Peak.QuixelLogic.Scripts.Common
{
    public sealed class LogoSubtitleScript : MonoBehaviour
    {
        private Image subtitle;

        [SerializeField]
        private Sprite JPsubtitle;

        [SerializeField]
        private Sprite ZHSsubtitle;

        [SerializeField]
        private Sprite ZHTsubtitle;

        private void Awake()
        {
            subtitle = GetComponent<Image>();

            switch (LocalisationSystem.GetSystemLanguage())
            {
                case SystemLanguage.Japanese:
                    if (JPsubtitle != null)
                        subtitle.sprite = JPsubtitle;
                    subtitle.enabled = true;
                    return;
                case SystemLanguage.ChineseSimplified:
                    if (ZHSsubtitle != null)
                        subtitle.sprite = ZHSsubtitle;
                    subtitle.enabled = true;
                    return;
                case SystemLanguage.ChineseTraditional:
                    if (ZHTsubtitle != null)
                        subtitle.sprite = ZHTsubtitle;
                    subtitle.enabled = true;
                    return;
                default:
                    Destroy(subtitle.gameObject); return;
            }
        }
    }
}