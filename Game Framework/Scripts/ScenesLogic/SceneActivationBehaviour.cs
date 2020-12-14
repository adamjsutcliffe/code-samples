using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Peak.Speedoku.Scripts.Common;
using Peak.UnityGameFramework.Scripts.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Peak.UnityGameFramework.Scripts.ScenesLogic
{
    public abstract class SceneActivationBehaviour<TActivator> : MonoBehaviour, ISceneActivationBehaviour
        where TActivator : SceneActivationBehaviour<TActivator>
    {
        [Header("Activator settings")]
        [SerializeField]
        protected Camera canvasCamera;

        [SerializeField]
        protected bool IsStaticCamera;

        [SerializeField]
        protected Canvas canvasRoot;

        [SerializeField] private Animator canvasAnimator;

        [SerializeField, UsedImplicitly]
        private bool areButtonsEnabled;

        // Contains all UI buttons of the selected canvases
        private readonly List<Button> interactables = new List<Button>();

        public static TActivator Instance { get; private set; }

        public Camera RootCamera => canvasCamera;

        private void Awake()
        {
            Instance = (TActivator)this;
            Hide();
        }

        public virtual void Initialize()
        {
            CollectButtons();
        }

        public virtual void Show(bool animated = false)
        {
            //print("[SAC] virtual show");
            if (canvasCamera)
            {
                canvasCamera.gameObject.SetActive(true);
            }

            if (canvasRoot)
            {
                canvasRoot.gameObject.SetActive(true);
            }

            if (canvasAnimator)
            {
                canvasAnimator.SetTrigger(animated ? Constants.Scene.PopScene : Constants.Scene.ShowScene);
            }
        }

        public virtual void Hide()
        {
            if (canvasCamera)
            {
                canvasCamera.gameObject.SetActive(false);
            }

            if (canvasRoot)
            {
                canvasRoot.gameObject.SetActive(false);
            }
        }

        private void CollectButtons()
        {
            if (canvasRoot)
            {
                //Button[] thisSceneButtons = canvasRoot.gameObject.GetComponentsInChildren<Button>(true);
                //allButtons.AddRange(thisSceneButtons);
                Button[] thisSceneInteractables = canvasRoot.gameObject.GetComponentsInChildren<Button>(true);
                interactables.AddRange(thisSceneInteractables);
                //interactables.ForEach(i => i.Initialize());
            }
        }

        public virtual void SetButtonsEnabled(bool isEnabled)
        {
            print($"[SAC] SetButtonsEnabled({isEnabled}) for ({GetType().Name}) ({name})");
            areButtonsEnabled = isEnabled;

            //Debug.Log($"[SAB] : SetButtonsEnabled={isEnabled} at '{name}' ({interactables.Count})", gameObject);

            // disable uGUI buttons
            for (int index = 0; index < interactables.Count; index++)
            {
                //interactables[index].SetInteractability(isEnabled);
                interactables[index].interactable = isEnabled;
            }
        }

        public virtual void StartGame()
        {
            print("[SAC] virtual - Strt game");
        }

        public virtual void StartRound(int difficulty)
        {
            print($"[SAC] virtual - start round -> diff: {difficulty}");
        }

        public virtual void PauseGame()
        {
            print("[SAC] virtual - Pause round");
        }

        public virtual void ResumeGame()
        {
            print("[SAC] virtual - Resume round");
        }

        public virtual void EndGame()
        {
            print("[SAC] virtual - End round");
        }

        public virtual void QuitGame()
        {
            print("[SAC] virtual - Quit game");
        }
    }
}


