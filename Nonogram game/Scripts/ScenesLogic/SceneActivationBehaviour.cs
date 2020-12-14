using System;
using System.Collections.Generic;
using Fabric.Crashlytics;
using JetBrains.Annotations;
using Peak.QuixelLogic.Scripts.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Peak.QuixelLogic.Scripts.ScenesLogic
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

        [SerializeField, UsedImplicitly]
        private bool areButtonsEnabled;

        // Contains all UI buttons of the selected canvases
        private readonly List<ButtonController> buttons = new List<ButtonController>();

        public static TActivator Instance { get; private set; }

        public Camera RootCamera => canvasCamera;

        private bool debugMode;

        private void Awake()
        {
            Instance = (TActivator)this;
#if CHEATS
            debugMode = true;
#else
            debugMode = false;
#endif
            Hide();
        }

        public virtual void Initialize()
        {
            CollectButtons();
        }

        public virtual void Show()
        {
            Crashlytics.Log($"Show scene - {this.name}");

            if (canvasCamera)
            {
                canvasCamera.gameObject.SetActive(true);
            }

            if (canvasRoot)
            {
                canvasRoot.gameObject.SetActive(true);
            }

            if (IsPopupScene()) // if we are showing a pop up
            {
                SceneActivationBehaviour<BoardBlurActivator>.Instance.Show(); // hide blur
            }
        }

        public virtual void Hide()
        {
            Crashlytics.Log($"Hide scene - {this.name}");

            if (canvasCamera)
            {
                canvasCamera.gameObject.SetActive(false);
            }

            if (canvasRoot)
            {
                canvasRoot.gameObject.SetActive(false);
            }

            if (IsPopupScene() && !InterfaceController.Instance.IsAnyPopupSceneActive()) // if we are hiding a pop up & if other pop ups are not active
            {
                SceneActivationBehaviour<BoardBlurActivator>.Instance.Hide(); // hide blur
            }
        }

        public virtual void SetButtons(bool enabled)
        {
            if (enabled)
            {
                for (int index = 0; index < buttons.Count; index++)
                {
                    buttons[index].enabled = true;// SetActive(false);
                }
            }
            else
            {
                for (int index = 0; index < buttons.Count; index++)
                {
                    buttons[index].enabled = false;// SetDisabled();
                }
            }
        }

        public virtual void DebugLog(string message)
        {
            if (!debugMode)
            {
                return;
            }

            Debug.Log(message, gameObject);
        }

        private void CollectButtons()
        {
            if (canvasRoot)
            {
                ButtonController[] thisSceneInteractables = canvasRoot.gameObject.GetComponentsInChildren<ButtonController>(true);
                buttons.AddRange(thisSceneInteractables);
            }
        }

        public bool IsActive()
        {
            return canvasRoot.isActiveAndEnabled;
        }
        
        private bool IsPopupScene()
        {
            if (this.name.IndexOf("popup", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }
            return false;
        }

        //public virtual void SetButtonsEnabled(bool isEnabled)
        //{
        //    //print($"[SAC] SetButtonsEnabled({isEnabled}) for ({GetType().Name}) ({name})");
        //    areButtonsEnabled = isEnabled;

        //    //Debug.Log($"[SAB] : SetButtonsEnabled={isEnabled} at '{name}' ({interactables.Count})", gameObject);

        //    // disable uGUI buttons
        //    for (int index = 0; index < interactables.Count; index++)
        //    {
        //        interactables[index].SetInteractability(isEnabled);
        //    }
        //}
    }
}


