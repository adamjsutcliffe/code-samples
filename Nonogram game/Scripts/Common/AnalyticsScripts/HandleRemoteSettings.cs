using Peak.QuixelLogic.Scripts.Common;
using UnityEngine;

/// <summary>
/// This script is used to get the remote settings that are needed for an A/B test for a release
/// Including: 
/// - AB test string key
/// - int, float or bool value needed to adjust a setting
/// </summary>

public class HandleRemoteSettings : MonoBehaviour
{
    [SerializeField]
    private bool ABTestingInEffect;

    [SerializeField]
    private bool RemoteSettingsInEffect;

    public string AB_IDString => AB_IDstring;
    private string AB_IDstring;

    // custom values for features to be tested or changed via remote settings

    private int DefaultFilmValue = 10;

    public int VariantFilmValue => variantFilmValue;
    private int variantFilmValue;

    private void Awake()
    {
        if (ABTestingInEffect)
        {
            RemoteSettings.Updated += ABTestsRemoteSettingsUpdated;
        }

        if (RemoteSettingsInEffect)
        {
            RemoteSettings.Updated += RemoteSettingsUpdated;
        }
    }

    private void ABTestsRemoteSettingsUpdated()
    {
        Debug.Log("[REMOTE] ***** GOT NEW AB TEST REMOTE SETTINGS ******");

        // get current AB test key - set to default if can't retrieve
        AB_IDstring = RemoteSettings.GetString("ABTestKey", Constants.ABTesting.DefaultAB_IDstring);

        GetCustomABTestKeyValues(); // get current AB test remote setting(s)

        RemoteSettings.Updated -= ABTestsRemoteSettingsUpdated;
        return;
    }

    private void GetCustomABTestKeyValues()
    {
        // TODO: change this function to retrive needed values for AB tests

        variantFilmValue = RemoteSettings.GetInt("TotalFilmCount", DefaultFilmValue);
    }

    private void RemoteSettingsUpdated()
    {
        // TODO: change this function to retrive needed values for remote settings

        GetCustomRemoteSettingsKeyValues(); // get current AB test remote setting(s)

        RemoteSettings.Updated -= RemoteSettingsUpdated;
        return;
    }

    private void GetCustomRemoteSettingsKeyValues()
    {
        // TODO: change this function to retrive needed values for remote settings


    }
}