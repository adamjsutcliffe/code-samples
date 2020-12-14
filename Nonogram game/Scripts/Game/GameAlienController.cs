using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Peak.QuixelLogic.Scripts.ScenesLogic;

namespace Peak.QuixelLogic.Scripts.Game
{
    public sealed class GameAlienController : MonoBehaviour
    {
        [SerializeField]
        private Animator quixelAnimator;

        private const string triggerCelebrate = "Celebrate";
        private const string triggerHint = "Hint";

        public bool shouldShowCharacter;

        [SerializeField]
        private GameObject hintProjectile;

        [SerializeField]
        private Transform hintorigin;

        private GameObject hintTargetCell;

        public AnimationStates quixelState { get; set; }

        public enum AnimationStates
        {
            idle,
            celebrating,
            hinting,
            resting
        }

        private void Start()
        {
            quixelState = AnimationStates.idle;
        }

        public void ToggleAnimator(bool toggle)
        {
            quixelAnimator.enabled = toggle;
        }

        public void ShowHideCharacter(bool show)
        {
            if (!show)
            {
                print($"[QUIXEL] Try show character not show -> {show}");
                gameObject.SetActive(false);
                quixelAnimator.enabled = false;
            }
            else
            {
                print($"[QUIXEL] Try show character else should show? -> {show}");
                gameObject.SetActive(shouldShowCharacter);
                quixelAnimator.enabled = shouldShowCharacter;
            }
        }

        public void SpawnHint(GameObject targetCell)
        {
            hintTargetCell = targetCell;
            SceneActivationBehaviour<OverlayUISceneActivator>.Instance.ToggleBlocker(true);

            if (quixelState == AnimationStates.idle || quixelState == AnimationStates.resting || quixelState == AnimationStates.celebrating)
            {
                quixelAnimator.SetTrigger(triggerHint);
                quixelState = AnimationStates.hinting;

                StartCoroutine(HintCountdown());
            }
        }

        public void ShootHintProjectile()
        {
            GameObject spawnedHint = Instantiate(hintProjectile, hintorigin);
            spawnedHint.GetComponent<HintProjectileScript>().target = hintTargetCell;

            SceneActivationBehaviour<OverlayUISceneActivator>.Instance.ToggleBlocker(false);
            hintTargetCell = null;
        }

        public void TryToCelebrate(bool postGame)
        {
            if (postGame) // end of game
            {
                if (quixelState == AnimationStates.idle || quixelState == AnimationStates.resting)
                {
                    quixelAnimator.SetTrigger(triggerCelebrate);
                    quixelState = AnimationStates.celebrating;
                    return;
                }
                return;
            }
            else
            {
                if (quixelState == AnimationStates.idle)
                {
                    quixelAnimator.SetTrigger(triggerCelebrate);
                    quixelState = AnimationStates.celebrating;

                    StartCoroutine(CelebrationCountdown());
                    return;
                }
                return;
            }
        }

        private IEnumerator CelebrationCountdown()
        {
            yield return new WaitForSeconds(2);
            quixelState = AnimationStates.resting;
            yield return new WaitForSeconds(8);
            quixelState = AnimationStates.idle;
            yield break;
        }

        private IEnumerator HintCountdown()
        {
            yield return new WaitForSeconds(1);
            ShootHintProjectile();
            quixelState = AnimationStates.idle;
            yield break;
        }
    }
}