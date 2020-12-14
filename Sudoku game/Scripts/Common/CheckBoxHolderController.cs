using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Peak.Speedoku.Scripts.Game.Gameplay;
using UnityEngine;

namespace Peak.Speedoku.Scripts.Common
{
    public class CheckBoxHolderController : MonoBehaviour
    {
        [SerializeField] private CheckBoxController checkBoxOne;
        [SerializeField] private CheckBoxController checkBoxTwo;
        [SerializeField] private CheckBoxController checkBoxThree;
        [SerializeField] private CheckBoxTextController textController;

        private const float animDelay = 0.5f;

        public void SetupWithAnswers(GridAnswer[] answers)
        {
            //TODO cycle through answers (what format?) and set check boxes correctly
            if (answers.Length != 3)
            {
                Debug.LogError($"Invalid number of reponses for checkmarks - {answers.Length}");
                return;
            }
            //sort by correct??
            GridAnswer[] orderedAnswers = answers.OrderByDescending(x => x.correct).ToArray();
            int incorrectCount = answers.Where(x => !x.correct).ToArray().Length;
            checkBoxOne.SetupWithAnswer(orderedAnswers[0].correct);
            checkBoxTwo.SetupWithAnswer(orderedAnswers[1].correct);
            checkBoxThree.SetupWithAnswer(orderedAnswers[2].correct);
            textController.SetupWithIncorrectCount(incorrectCount);
        }

        public void ShowAnswers(Action completion = null)
        {
            StartCoroutine(AnimateChecks(completion));
        }

        private IEnumerator AnimateChecks(Action completion = null)
        {
            yield return new WaitForSeconds(animDelay);

            checkBoxOne.StartAnimation();

            yield return new WaitForSeconds(animDelay);

            checkBoxTwo.StartAnimation();

            yield return new WaitForSeconds(animDelay);

            checkBoxThree.StartAnimation();

            yield return new WaitForSeconds(animDelay);

            textController.StartAnimation();

            yield return new WaitForSeconds(animDelay);

            completion?.Invoke();
        }
    }
}
