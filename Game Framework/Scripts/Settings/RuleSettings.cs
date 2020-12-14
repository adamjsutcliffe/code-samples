using System;
using System.Collections.Generic;
using System.Linq;
using Peak.Speedoku.Scripts.Game.Gameplay;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Peak.Speedoku.Scripts.Settings
{
    /// <summary>
    /// Rules - to become a scripted object - used to hold round, level, board or other rulesets for a game
    /// </summary>

    [CreateAssetMenu(fileName = "LevelSettings", menuName = "ScriptableObjects/CreateLevelRuleSettings", order = 2)]
    public sealed class RuleSettings : ScriptableObject
    {
        [Header("Non-localisable identifier. Do not change.")]
        public string Id;

        public string levelData;

        public int[] targetIndexes;

        public GridSolutionType[] targetSolutions;

        public int difficultyRating;

        [Tooltip("Shows if the game has FTUE, pre-game window will be skipped!")]
        public bool IsFtueGame;

        public bool IsRuleValid()
        {
            int errorCount = 0;
            int[] levelDataArray = levelData.Select(x => Int32.Parse(x.ToString())).ToArray();
            if (levelDataArray.Length != 81)
            {
                //Debug.LogError($"Rule id: {Id} length is invalid ({levelDataArray.Length})");
                Debug.LogError($"Rule id: {Id} length is invalid ({levelDataArray.Length})");
                errorCount += 1;
            }

            if (targetIndexes.Length < 1 && targetIndexes.Length != 3)
            {
                Debug.LogError($"Rule id: {Id} target length is invalid");
                errorCount += 1;
            }
            GameObject gameObject = new GameObject();

            for (int i = 0; i < levelDataArray.Length; i++)
            {
                gameObject.AddComponent<GridSquareScript>();
            }
            GridSquareScript[] gridSquares = gameObject.GetComponentsInChildren<GridSquareScript>();

            for (int i = 0; i < gridSquares.Length; i++)
            {
                bool isTarget = targetIndexes.Contains(i);
                GridSquareScript gridSquare = gridSquares[i];
                gridSquare.SetupTestGridSquare(i, levelDataArray[i], isTarget);
            }
            Debug.LogWarning($"************* RULE {Id} *************");
            targetSolutions = new GridSolutionType[targetIndexes.Length];
            difficultyRating = 0;
            for (int i = 0; i < targetIndexes.Length; i++)
            {

                int targetCount = 0;
                GridSquareScript target = gridSquares[targetIndexes[i]];
                Debug.LogWarning($"---------- TARGET {target.Index} ---------");
                for (int j = 1; j <= 9; j++)
                {
                    target.UpdateTestGridNumber(j);
                    Debug.LogWarning($"---------- Number {target.Number} ---------");
                    GridSolutionType solution = GridSolver.SolveGridAtIndex(gridSquares, target);
                    if (solution != GridSolutionType.None)
                    {
                        targetCount += 1;
                        Debug.LogWarning($"Rule id: {Id} target {targetIndexes[i]} has solution with {j} ({solution})");
                        targetSolutions[i] = solution;
                    }
                }

                //RESET TARGET AFTER TEST SO DOESN"T EFFECT NEXT TARGET!!!
                target.UpdateTestGridNumber(0); 

                if (targetCount != 1)
                {
                    Debug.LogError($"Rule id: {Id} target {targetIndexes[i]} has {targetCount} solutions");
                    errorCount += 1;
                }
                else
                {
                    difficultyRating += (int)targetSolutions[i];
                }
            }
            DestroyImmediate(gameObject);
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
            return errorCount == 0;
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(RuleSettings))]
    public class RuleSettingsEditor : Editor
    {
        SerializedProperty levelData;
        SerializedProperty targetData;

        private void OnEnable()
        {
            levelData = serializedObject.FindProperty("levelData");
            targetData = serializedObject.FindProperty("targetIndexes");
        }
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            string dataString = levelData.stringValue;
            Debug.Log($"Data string {dataString}");
            EditorGUILayout.LabelField("Grid ref");

            List<int> targets = new List<int>();
            for (int i = 0; i < targetData.arraySize; i++)
            {
                targets.Add(targetData.GetArrayElementAtIndex(i).intValue);
            }

            for (int i = 0; i < 9; i++)
            {
                string lineString = dataString.Substring(9 * i, 9);
                EditorGUILayout.BeginHorizontal();
                for (int j = 0; j < 9; j++)
                {
                    int index = i * 9 + j;
                    string labelStr = lineString[j].Equals('0') && !targets.Contains(index) ? "-" : $"{lineString[j]}";
                    EditorGUILayout.LabelField($"{labelStr}", targets.Contains(index) ? EditorStyles.whiteLargeLabel : EditorStyles.largeLabel, GUILayout.Width(20), GUILayout.Height(20));
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
#endif
}
