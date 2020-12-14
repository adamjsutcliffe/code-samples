#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

class CustomBuildProcessor : IPreprocessBuild
{
    public int callbackOrder { get { return 0; } }
    public void OnPreprocessBuild(BuildTarget target, string path)
    {
        int currentiOSBuildNumber;
        int currentAndroidBuildNumber;

        if (target == BuildTarget.iOS)
        {
            int.TryParse(PlayerSettings.iOS.buildNumber, out currentiOSBuildNumber);

            if (currentiOSBuildNumber == 0) // if we have entered a specific build name
            {
                return;
            }
            else
            {
                currentAndroidBuildNumber = PlayerSettings.Android.bundleVersionCode;

                if (currentiOSBuildNumber == currentAndroidBuildNumber) // if android build has come first and iOS build number is the same
                {
                    PlayerSettings.Android.bundleVersionCode = currentAndroidBuildNumber;
                    PlayerSettings.iOS.buildNumber = currentiOSBuildNumber.ToString();
                    EditorApplication.ExecuteMenuItem("File/Save Project");

                    return;
                }
                else
                {
                    if (currentiOSBuildNumber > currentAndroidBuildNumber) // if iOS is higher
                    {
                        currentAndroidBuildNumber = currentiOSBuildNumber;
                    }
                    else if (currentAndroidBuildNumber > currentiOSBuildNumber) // if android is higher
                    {
                        currentiOSBuildNumber = currentAndroidBuildNumber;
                    }

                    currentiOSBuildNumber++;
                    currentAndroidBuildNumber++;

                    PlayerSettings.Android.bundleVersionCode = currentAndroidBuildNumber;
                    PlayerSettings.iOS.buildNumber = currentiOSBuildNumber.ToString();
                    EditorApplication.ExecuteMenuItem("File/Save Project");

                    return;
                }
            }
        }
        else if (target == BuildTarget.Android)
        {
            currentAndroidBuildNumber = PlayerSettings.Android.bundleVersionCode;
            currentAndroidBuildNumber++;

            PlayerSettings.Android.bundleVersionCode = currentAndroidBuildNumber;
            PlayerSettings.iOS.buildNumber = currentAndroidBuildNumber.ToString();
            EditorApplication.ExecuteMenuItem("File/Save Project");

            return;
        }
    }
}
#endif