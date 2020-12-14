using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using SQLite;
using UnityEngine.Profiling;
using Debug = UnityEngine.Debug;
using System.Diagnostics;

#if UNITY_ANDROID && !UNITY_EDITOR
using System.Diagnostics;
#endif

namespace Peak.UnityGameFramework.Scripts.Common //Peak.WordBlitz.Scripts.Common
{
    /// <summary>
    /// API for client-server communication
    /// </summary>
    public sealed class DictionaryHelper : MonoBehaviour
    {
        /// <summary>
        /// Database file name (or path) relative to StreamingAssets folder.
        /// </summary>
        [SerializeField]
        private string databaseFilepath = "Dictionary.sqlite";

#pragma warning disable 0414

        /// <summary>
        /// Max allowed time to load a database file. (1 sec = 1000 ms)
        /// </summary>
        [SerializeField, UsedImplicitly(ImplicitUseKindFlags.Access)]
        private int maxLoadTimeInMilliseconds = 3000;

#pragma warning restore 0414

        /// <summary>
        /// The connection.
        /// </summary>
        private SQLiteConnection connection;

        public static DictionaryHelper Instance { get; private set; }

#if UNITY_EDITOR
        public SQLiteConnection Connection => connection;
#endif


        private void Awake()
        {
            if (Instance)
            {
                Debug.LogWarning("DictionatyHelper duplicate instantiation.");
            }
            else
            {
                StartCoroutine(LoadWordsFromDatabaseCoroutine());
                Instance = this;
            }
        }

        #region Public methods

        /// <summary>
        /// Determines whether the word is valid.
        /// </summary>
        /// <returns><c>true</c> if this instance is word valid the specified word; otherwise, <c>false</c>.</returns>
        /// <param name="word">Word to check</param>
        public bool IsWordValid(string word)
        {
            Profiler.BeginSample("Sqlite.ExecuteScalar");
            word = word.Replace("*", "_");
            
            int result = connection.ExecuteScalar<int>(
                "select count(1) from Dictionary where word like \"" + word.ToLower() + "\" limit 1");
            Profiler.EndSample();
            print($"Is word {word} valid ? {result}");
            return result > 0;
        }

        /// <summary>
        /// Determines whether the word is valid.
        /// </summary>
        /// <returns><c>true</c> if this instance is word valid the specified word; otherwise, <c>false</c>.</returns>
        /// <param name="word">Word to check</param>
        /// <param name="words">List of all words that matches wild pattern</param>
        public bool IsWordValid(string word, out char[][] words)
        {
            Profiler.BeginSample("Sqlite.ExecuteScalar");

            word = word.Replace("*", "_");

            List<WordRecord> list = connection.Query<WordRecord>(
                "select word from Dictionary where word like \"" + word.ToLower() + "\"");
            
            words = new char[word.Length][];

            for (int i = 0; i < words.Length; i++)
            {
                words[i] = list.Select(record => record.word[i]).ToArray();
            }

            Profiler.EndSample();

            return list.Count > 0;
        }

        private class WordRecord
        {
            public string word { get; set; }
        }

        #endregion

        #region Internal methods

        /// <summary>
        /// Loads the dictionary from database.
        /// </summary>
        public IEnumerator LoadWordsFromDatabaseCoroutine()
        {
            string dbPersistentPath = GetPersistentDatabaseFilePath();
            print($"Database path: {dbPersistentPath}");
#if UNITY_ANDROID //&& !UNITY_EDITOR // Check if db file has already been extracted
            if (!File.Exists(dbPersistentPath))
            {
                WWW www = new WWW(GetStreamingAssetsDatabaseUri());

                Stopwatch loadTimer = Stopwatch.StartNew();
                while (!www.isDone)
                {
                    yield return new WaitForSeconds(0.1f);

                    if (!string.IsNullOrEmpty(www.error))
                    {
                        Debug.LogError($"Error loading database file! Details: {www.error}.");
                        yield break;
                    }

                    if (loadTimer.ElapsedMilliseconds > maxLoadTimeInMilliseconds)
                    {
                        Debug.LogError($"Error loading database file! Timeout exceeded.");
                        yield break;
                    }
                }

                // Saving extracted file
                File.WriteAllBytes(dbPersistentPath, www.bytes);
            }

#endif

            // Creating connection
            connection = new SQLiteConnection(dbPersistentPath, SQLiteOpenFlags.ReadOnly);
            yield return null;
        }

#if UNITY_ANDROID //&& !UNITY_EDITOR

        private string GetStreamingAssetsDatabaseUri()
        {
            return "jar:file://" + Application.dataPath + "!/assets/" + databaseFilepath;
        }

#endif

        private string GetPersistentDatabaseFilePath()
        {
            string resourceBasePath =
#if UNITY_ANDROID //&& !UNITY_EDITOR
                Application.persistentDataPath;
#else
                Application.streamingAssetsPath;
#endif
            print($"Get persistant database file path: {resourceBasePath} - {databaseFilepath} or?? {Application.persistentDataPath}");
            return Path.Combine(resourceBasePath, databaseFilepath);
        }

#endregion
    }
}