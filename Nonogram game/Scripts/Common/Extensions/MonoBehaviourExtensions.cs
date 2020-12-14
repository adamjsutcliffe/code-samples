using System;
using System.Collections;
using UnityEngine;

namespace Peak.Quixellogic.Scripts.Common.Extensions
{
    public static class MonoBehaviourExtensions
    {
        /// <summary>
        /// Restarts Monobehaviour's coroutine if any were started.
        /// </summary>
        /// <param name="script">GO that holds a coroutine</param>
        /// <param name="coroutineBody">IEnumerator method to be called by a coroutine</param>
        /// <param name="coroutineObject">Coroutine object to manage</param>
        public static void RestartCoroutine(this MonoBehaviour script,
            Func<IEnumerator> coroutineBody, ref Coroutine coroutineObject)
        {
           script.RestartCoroutine(coroutineBody(),ref coroutineObject);
        }

        /// <summary>
        /// Restarts Monobehaviour's coroutine if any were started.
        /// </summary>
        /// <param name="script">GO that holds a coroutine</param>
        /// <param name="coroutineBody">IEnumerator method to be called by a coroutine</param>
        /// <param name="coroutineObject">Coroutine object to manage</param>
        public static void RestartCoroutine(this MonoBehaviour script,
            IEnumerator coroutineBody, ref Coroutine coroutineObject)
        {
            if (coroutineObject != null)
            {
                script.StopCoroutine(coroutineObject);
            }

            coroutineObject = script.isActiveAndEnabled ? script.StartCoroutine(coroutineBody) : null;
        }
    }
}