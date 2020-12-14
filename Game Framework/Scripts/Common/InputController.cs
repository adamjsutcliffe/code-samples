using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;
using System.Linq;

namespace Peak.Speedoku.Scripts.Common
{
    public class InputController : MonoBehaviour
    {
        [UsedImplicitly]
        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        public void Initialise()
        {
            print("[   ] Initialising input controller ");
        }



    }
}


