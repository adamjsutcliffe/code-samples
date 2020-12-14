using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Peak.Speedoku.Scripts.Common
{
    public class CheckBoxTextController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textField;
        [SerializeField] private Animator textAnimator;

        public void SetupWithIncorrectCount(int incorrect)
        {
            textField.text = incorrect == 0 ? "CONGRATULATIONS" : $"You got {incorrect} wrong";
            textAnimator.Rebind();
        }

        public void StartAnimation()
        {
            textAnimator.SetTrigger(Constants.Animation.CheckBox.ShowText);
        }
    }
}
