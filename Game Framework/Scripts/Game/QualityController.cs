using JetBrains.Annotations;
using Peak.UnityGameFramework.Scripts.Common;
using UnityEngine;

namespace Peak.Speedoku.Scripts.Game
{
    /// <summary>
    /// Provides basic logic for the game setup
    /// </summary>
    public sealed class QualityController : MonoBehaviour
    {
#if UNITY_IOS

        [SerializeField]
        private UnityEngine.iOS.DeviceGeneration hdStartsFromApple = UnityEngine.iOS.DeviceGeneration.iPadAir2;

        [SerializeField, UsedImplicitly]
        private int hdStartsFromRamMb;

        private void SetupQualityByApple()
        {
            if (UnityEngine.iOS.Device.generation >= hdStartsFromApple)
            {
                QualitySettings.SetQualityLevel((int) UnityQualitySettingsLevel.HD, true);
            }
            else
            {
                QualitySettings.SetQualityLevel((int) UnityQualitySettingsLevel.SD, true);
            }
        }

#else
        [SerializeField, UsedImplicitly]
        private int hdStartsFromApple;

        [SerializeField]
        private int hdStartsFromRamMb = 2 * 1024;

        private void SetupQualityByMemory()
        {
            if (SystemInfo.systemMemorySize >= hdStartsFromRamMb)
            {
                QualitySettings.SetQualityLevel((int)UnityQualitySettingsLevel.HD, true);
            }
            else
            {
                QualitySettings.SetQualityLevel((int)UnityQualitySettingsLevel.SD, true);
            }
        }

#endif

        /// <summary>
        /// Localisation singleton 
        /// </summary>
        public static QualityController Instance { get; private set; }

        private void Awake()
        {
            if (Instance)
            {
                Debug.LogWarning("[QUALITY] QualityController duplicate instantiation.");
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Start()
        {
            //SetupQualityAfterFirstRun();
        }

        private void SetupQualityAfterFirstRun()
        {
            if (PlayerPrefs.HasKey(Constants.SettingKeys.QualitySettingsEnabled))
            {
                QualitySettings.SetQualityLevel(PlayerPrefs.GetInt(Constants.SettingKeys.QualitySettingsEnabled), true);
                Debug.LogWarning($"[QUALITY] Quality restored to: {(UnityQualitySettingsLevel)QualitySettings.GetQualityLevel()}");

                return;
            }

#if UNITY_EDITOR
            QualitySettings.SetQualityLevel((int)UnityQualitySettingsLevel.HD, true);
#elif UNITY_ANDROID
            SetupQualityByMemory();
#elif UNITY_IOS
            SetupQualityByApple();
#endif

            PlayerPrefs.SetInt(Constants.SettingKeys.QualitySettingsEnabled, QualitySettings.GetQualityLevel());
            Debug.LogWarning($"[QUALITY] Quality level set to: {(UnityQualitySettingsLevel)QualitySettings.GetQualityLevel()}");
        }

        /// <summary>
        /// Switches to new quality level
        /// </summary>
        //Call using ... QualityController.Instance.ToggleQuality();
        public string ToggleQuality()
        {
            UnityQualitySettingsLevel previousLevel = (UnityQualitySettingsLevel)QualitySettings.GetQualityLevel();
            UnityQualitySettingsLevel nextLevel = previousLevel == UnityQualitySettingsLevel.SD ? UnityQualitySettingsLevel.HD : UnityQualitySettingsLevel.SD;
            QualitySettings.SetQualityLevel((int)nextLevel, true);
            PlayerPrefs.SetInt(Constants.SettingKeys.QualitySettingsEnabled, (int)nextLevel);
            return nextLevel.ToString();
        }
    }
}