using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Peak.Speedoku.Scripts.Settings;

#if UNITY_EDITOR
using UnityEditorInternal;
using UnityEditor;
#endif

namespace Peak.UnityGameFramework.Scripts.Settings
{
    public sealed class GlobalSettings : ScriptableObject
    {
        [Header("SPE")]
        //[Tooltip("Coin specific settings")]
        //public CoinSettings Coins;

        //[Tooltip("location Boundaries, last value is looped")]
        //public int[] locationBoundaries;

        [Tooltip("Rule list")]
        public List<RuleSettings> RulesList;

        //[Tooltip("Location backgrounds, list is looped")]
        //public LocationSetting[] LocationSettings;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(GlobalSettings))]
    [CanEditMultipleObjects]
    public class GlobalSettingsEditor : Editor
    {
        //SerializedProperty coinsProperty;
        //SerializedProperty boundaryProperty;
        ReorderableList rulesProperty;
        //ReorderableList locationProperty;

        private void OnEnable()
        {
            //coinsProperty = serializedObject.FindProperty("Coins");
            //boundaryProperty = serializedObject.FindProperty("locationBoundaries");
            rulesProperty = new ReorderableList(serializedObject, serializedObject.FindProperty("RulesList"), true, true, true, true);
            //locationProperty = new ReorderableList(serializedObject, serializedObject.FindProperty("LocationSettings"), true, true, true, true);

            rulesProperty.drawHeaderCallback = (Rect rect) =>
            {
                rulesProperty.serializedProperty.isExpanded = EditorGUI.ToggleLeft(rect, rulesProperty.serializedProperty.displayName, rulesProperty.serializedProperty.isExpanded, EditorStyles.boldLabel);
            };

            rulesProperty.onRemoveCallback = (ReorderableList l) => {
                if (EditorUtility.DisplayDialog("Warning!",
                    "Are you sure you want to delete this rule?", "Yes", "No"))
                {
                    ReorderableList.defaultBehaviours.DoRemoveButton(l);
                    ReorderableList.defaultBehaviours.DoRemoveButton(l); //Double because single only deletes the rule not hte array placement
                }
            };
            rulesProperty.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = rulesProperty.serializedProperty.GetArrayElementAtIndex(index);
                if (element.objectReferenceValue != null)
                {
                    RuleSettings rule = (RuleSettings)element.objectReferenceValue;
                    rect.y += 4;
                    
                    EditorGUI.LabelField(new Rect(rect.x + 60, rect.y, 150, EditorGUIUtility.singleLineHeight), $"Diff: {rule.difficultyRating} \t FTUE: { rule.IsFtueGame}");
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, new GUIContent($"Rule {index}: ")); //new Rect(rect.x, rect.y, 250, EditorGUIUtility.singleLineHeight)
                }
            };

            //locationProperty.drawHeaderCallback = (Rect rect) =>
            //{
            //    locationProperty.serializedProperty.isExpanded = EditorGUI.ToggleLeft(rect, locationProperty.serializedProperty.displayName, locationProperty.serializedProperty.isExpanded, EditorStyles.boldLabel);
            //};

            //locationProperty.onRemoveCallback = (ReorderableList l) => {
            //    if (EditorUtility.DisplayDialog("Warning!",
            //        "Are you sure you want to delete this location?", "Yes", "No"))
            //    {
            //        ReorderableList.defaultBehaviours.DoRemoveButton(l);
            //        ReorderableList.defaultBehaviours.DoRemoveButton(l); //Double because single only deletes the rule not hte array placement
            //    }
            //};
            //locationProperty.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            //{
            //    var element = locationProperty.serializedProperty.GetArrayElementAtIndex(index);
            //    if (element.objectReferenceValue != null)
            //    {
            //        LocationSetting location = (LocationSetting)element.objectReferenceValue;
            //        rect.y += 4;

            //        EditorGUI.LabelField(new Rect(rect.x + 30, rect.y, 200, EditorGUIUtility.singleLineHeight), $"Name: {location.locationName}");
            //        EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, new GUIContent($"{index}:")); //new Rect(rect.x, rect.y, 500, EditorGUIUtility.singleLineHeight)
            //    }
            //};
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            //EditorGUILayout.PropertyField(coinsProperty, true);
            EditorGUILayout.Space();
            //EditorGUILayout.PropertyField(boundaryProperty, true);
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (rulesProperty.serializedProperty.isExpanded)
            {
                rulesProperty.DoLayoutList();
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                rulesProperty.serializedProperty.isExpanded = EditorGUILayout.ToggleLeft(string.Format("{0}[]", rulesProperty.serializedProperty.displayName), rulesProperty.serializedProperty.isExpanded, EditorStyles.boldLabel);
                EditorGUILayout.LabelField(string.Format("size: {0}", rulesProperty.serializedProperty.arraySize));
                EditorGUILayout.EndHorizontal();
            }
            
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            //if (locationProperty.serializedProperty.isExpanded)
            //{
            //    locationProperty.DoLayoutList();
            //}
            //else
            //{
            //    EditorGUILayout.BeginHorizontal();
            //    locationProperty.serializedProperty.isExpanded = EditorGUILayout.ToggleLeft(string.Format("{0}[]", locationProperty.serializedProperty.displayName), locationProperty.serializedProperty.isExpanded, EditorStyles.boldLabel);
            //    EditorGUILayout.LabelField(string.Format("size: {0}", locationProperty.serializedProperty.arraySize));
            //    EditorGUILayout.EndHorizontal();
            //}

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }
    }
#endif
}


