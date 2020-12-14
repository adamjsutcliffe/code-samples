using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Peak.Speedoku.Scripts.Settings
{
    [CreateAssetMenu(fileName = "LocationSettings", menuName = "ScriptableObjects/CreateLocationSettings", order = 3)]
    [Serializable]
    public class LocationSetting : ScriptableObject
    {
        public Sprite locationBackground;

        public string locationName;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(LocationSetting))]
    [CanEditMultipleObjects]
    public class LocationSettingEditor: Editor
    {
        SerializedProperty backgroundImage;
        SerializedProperty locationName;

        private void OnEnable()
        {
            backgroundImage = serializedObject.FindProperty("locationBackground");
            locationName = serializedObject.FindProperty("locationName");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(backgroundImage);
            EditorGUILayout.PropertyField(locationName);
            var texture = AssetPreview.GetAssetPreview(backgroundImage.objectReferenceValue);
            
            GUILayout.Label(texture);
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
