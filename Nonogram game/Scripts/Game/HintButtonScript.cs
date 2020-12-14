using System.Collections;
using System.Collections.Generic;
using Peak.QuixelLogic.Scripts.Common;
using Peak.QuixelLogic.Scripts.ScenesLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Peak.QuixelLogic.Scripts.Game
{
    public class HintButtonScript : MonoBehaviour
    {
        [SerializeField]
        private Animator shakeAnimator;

        [SerializeField]
        private ButtonController hintButton;

        [SerializeField]
        private Button button;

        // free

        [SerializeField]
        private GameObject containerFree;

        [SerializeField]
        private TextMeshProUGUI hintNumber;

        // cost

        [SerializeField]
        private GameObject containerCost;

        [SerializeField]
        private TextMeshProUGUI costLabel;

        private HintState thisState;

        public HintState state
        {
            get
            {
                return thisState;
            }
            set
            {
                thisState = value;

                Player player = SceneActivationBehaviour<GameLogicActivator>.Instance.GameController.Player;

                hintNumber.text = player.Hints.ToString();
                costLabel.text = SceneActivationBehaviour<GameLogicActivator>.Instance.GameController.GlobalSettings.Hints.HintCost.ToString();

                containerFree.SetActive(state == HintState.freeHints && hintButton.gameObject.activeInHierarchy);
                hintNumber.gameObject.transform.parent.gameObject.SetActive(state == HintState.freeHints && hintButton.gameObject.activeInHierarchy);

                containerCost.SetActive(state == HintState.costHints && hintButton.gameObject.activeInHierarchy);
                costLabel.gameObject.transform.parent.gameObject.SetActive(state == HintState.costHints && hintButton.gameObject.activeInHierarchy);

                if (thisState == HintState.costHints)
                {
                    if (player.FtuePassed)
                    {
                        hintButton.SetInteractability(true);
                    }
                    else hintButton.SetInteractability(false);
                }
                else
                {
                    hintButton.SetInteractability(true);
                }
            }
        }

        public enum HintState
        {
            freeHints,
            costHints
        }

        public void FtueForceHintOn()
        {
            hintButton.SetInteractability(true);
        }

        public void EnableHint()
        {
            hintButton.SetInteractability(true);
        }

        public void DisableHint()
        {
            hintButton.SetInteractability(false);
        }

        public void ShakeHint(bool shake)
        {
            if (!hintButton.enabled)
            {
                if (shake)
                {
                    shakeAnimator.SetTrigger("StartShake");
                    shakeAnimator.ResetTrigger("StopShake");
                }
                else
                {
                    shakeAnimator.SetTrigger("StopShake");
                    shakeAnimator.ResetTrigger("StartShake");
                }
            }
            else return;
        }
    }
}