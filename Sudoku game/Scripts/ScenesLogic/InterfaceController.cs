﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Peak.Speedoku.Scripts.Autogenerated;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Peak.Speedoku.Scripts.ScenesLogic
{
    public sealed class InterfaceController : MonoBehaviour
    {
        [SerializeField]
        private float loadSleepTimeout;

        [SerializeField]
        private GameWindow startSceneName;

        [SerializeField]
        private bool showLogs;

        public static InterfaceController Instance { get; private set; }

        private readonly List<GameObject> temp = new List<GameObject>(16);

        private readonly Dictionary<GameWindow, List<ISceneActivationBehaviour>> activators =
            new Dictionary<GameWindow, List<ISceneActivationBehaviour>>
            {
                { GameWindow.NoWindow, new List<ISceneActivationBehaviour>(0) }
            };

        private List<GameWindow> currentlyLoading = new List<GameWindow>();

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Load(new[] { startSceneName }, () => Show(startSceneName));
        }

        public void Load(GameWindow[] scenes, Action callback = null)
        {
            StartCoroutine(LoadAsyncCoroutine(scenes, callback));
        }

        public void LoadWithProgress(GameWindow[] scenes, Action callback = null, Action<int> progress = null)
        {
            StartCoroutine(LoadAsyncCoroutine(scenes, callback, progress));
        }

        public void Unload(GameWindow scene)
        {
            activators.Remove(scene);

            string sceneName = GameWindowNames.Mapping[scene];
            Scene sceneObject = SceneManager.GetSceneByName(sceneName);

            if (sceneObject.isLoaded)
            {
                var unloadAsyncOperation = SceneManager.UnloadSceneAsync(sceneName);
                StartCoroutine(FreeResourcesAfterUnloadCompleteCoroutine(unloadAsyncOperation));
            }
        }

        private IEnumerator FreeResourcesAfterUnloadCompleteCoroutine(AsyncOperation unloadAsyncOperation)
        {
            yield return unloadAsyncOperation;
            yield return Resources.UnloadUnusedAssets();
            GC.Collect();
        }

        public void Show(GameWindow scene, bool animated = false)
        {
            if (activators.ContainsKey(scene))
            {
                activators[scene].ForEach(x =>
                {
                    if (showLogs) Debug.Log($"[LOAD] Show call to {x.name}");
                    x.Show(animated);
                });
            }
        }

        public void ShowAndSetActive(GameWindow scene)
        {
            Show(scene);

            string sceneName = GameWindowNames.Mapping[scene];
            Scene sceneObject = SceneManager.GetSceneByName(sceneName);

            Debug.Log($"[LOAD] SHOW Scene {sceneName}, obj {sceneObject}");

            if (SceneManager.GetActiveScene() == sceneObject)
            {
                print($"[LOAD][ACTIVE] scene: {sceneName} is already active");
                return;
            }

            try
            {
                SceneManager.SetActiveScene(sceneObject);
            }
            catch (Exception e)
            {
                Debug.LogException(new Exception($"[LOAD] Cannot set active scene '{scene}' with name '{sceneName}'", e), gameObject);
                throw;
            }
        }

        public void Hide(GameWindow scene)
        {
            if (activators.ContainsKey(scene))
            {
                activators[scene].ForEach(x =>
                {
                    if (showLogs) Debug.Log($"[LOAD] Hide call to {x.name}");
                    x.Hide();
                });
            }
        }

        public void SetButtonsEnabled(bool isEnabled)
        {
            foreach (var activator in activators)
            {
                for (int activatorIndex = 0; activatorIndex < activator.Value.Count; activatorIndex++)
                {
                    ISceneActivationBehaviour activationBehaviour = activator.Value[activatorIndex];
                    activationBehaviour.SetButtonsEnabled(isEnabled);
                }
            }
        }

        private IEnumerator LoadAsyncCoroutine(GameWindow[] loadingScenes, Action callback, Action<int> progress = null)
        {
            if (showLogs) Debug.Log("[LOAD] Start loading routine");

            for (int sceneIndex = 0; sceneIndex < loadingScenes.Length; sceneIndex++)
            {
                GameWindow scene = loadingScenes[sceneIndex];
                string sceneName = GameWindowNames.Mapping[scene];

                if (activators.ContainsKey(scene))
                {
                    print($"[LOAD] scene {scene} is already loaded!!");
                    continue;
                }

                if (currentlyLoading.Contains(scene))
                {
                    print($"[LOAD] scene {scene} is currently loading!!");
                    continue;
                }
                if (showLogs) Debug.Log($"[LOAD] Set currently loading for {sceneName}");
                currentlyLoading.Add(scene);

                if (showLogs) Debug.Log($"[LOAD] Loading scene: {sceneName}");

                AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

                if (loadOperation != null)
                {
                    while (!loadOperation.isDone)
                    {
                        yield return new WaitForSeconds(loadSleepTimeout);
                    }
                }

                if (showLogs) Debug.Log($"[LOAD] Gathering activators: {sceneName}");

                SceneManager.GetSceneByName(sceneName).GetRootGameObjects(temp);

                if (showLogs) Debug.Log($"[LOAD] Set real data for {sceneName}");
                activators[scene] = temp
                    .Select(x => x.GetComponent<ISceneActivationBehaviour>())
                    .Where(x => x != null)
                    .ToList();

                progress?.Invoke(sceneIndex);
                if (showLogs) Debug.Log($"[LOAD] Remove currently loading for {sceneName}");
                currentlyLoading.Remove(scene);
            }

            foreach (GameWindow scene in loadingScenes)
            {
                foreach (ISceneActivationBehaviour activator in activators[scene])
                {
                    if (showLogs) Debug.Log($"[LOAD] Initialize: {activator.name}");
                    activator.Initialize();
                }
            }

            foreach (GameWindow scene in loadingScenes)
            {
                foreach (ISceneActivationBehaviour activator in activators[scene])
                {
                    if (showLogs) Debug.Log($"[LOAD] Hide: {activator.name}");
                    activator.Hide();
                }
            }

            if (showLogs) Debug.Log("[LOAD] Callback");
            callback?.Invoke();

            if (showLogs) Debug.Log("[LOAD] End");
        }
    }
}

