using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Peak.Speedoku.Scripts.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Peak.Speedoku.Scripts.ScenesLogic
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
        private readonly List<GraphicsColourTint> interactables = new List<GraphicsColourTint>();

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
                GraphicsColourTint[] thisSceneInteractables = canvasRoot.gameObject.GetComponentsInChildren<GraphicsColourTint>(true);
                interactables.AddRange(thisSceneInteractables);
                interactables.ForEach(i => i.Initialize());
            }
        }

        public virtual void SetButtonsEnabled(bool isEnabled)
        {
            //print($"[SAC] SetButtonsEnabled({isEnabled}) for ({GetType().Name}) ({name})");
            areButtonsEnabled = isEnabled;

            //Debug.Log($"[SAB] : SetButtonsEnabled={isEnabled} at '{name}' ({interactables.Count})", gameObject);

            // disable uGUI buttons
            for (int index = 0; index < interactables.Count; index++)
            {
                interactables[index].SetInteractability(isEnabled);
            }
        }
    }
}


