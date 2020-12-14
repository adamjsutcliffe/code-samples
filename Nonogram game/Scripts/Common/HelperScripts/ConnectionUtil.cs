using System;
using System.Collections;
using UnityEngine;

namespace Peak.QuixelLogic.Scripts.Common
{
    public class ConnectionUtil : MonoBehaviour
    {
        //handle on internet ping
        private WWW www;

        // TimeOut Coroutine property
        private Coroutine timeOutCoroutine;

        // Internet Check Coroutine property
        private Coroutine internetCheckCoroutine;

        private bool IsOnline => Application.internetReachability != NetworkReachability.NotReachable;

        public void CheckInternet(float timeout, Action success = null, Action failure = null)
        {
            if (!IsOnline)
            {
                Debug.LogWarning($"[INTERNET] - Offline - ({Application.internetReachability})");
                failure?.Invoke();
                return;
            }
            internetCheckCoroutine = StartCoroutine(CheckInternetCoroutine(timeout, success, failure));
        }

        private IEnumerator CheckInternetCoroutine(float timeout, Action success = null, Action failure = null)
        {
            www = new WWW("https://google.com");
            timeOutCoroutine = StartCoroutine(CheckTimeOutCoroutine(timeout, failure));
            yield return www;

            StopCoroutine(timeOutCoroutine);

            if (www.isDone && www.bytesDownloaded > 0)
            {
                success?.Invoke();
            }
            else
            {
                failure?.Invoke();
            }
        }

        private IEnumerator CheckTimeOutCoroutine(float timeout, Action failure = null)
        {
            yield return new WaitForSeconds(timeout);
            StopCoroutine(internetCheckCoroutine);
            internetCheckCoroutine = null;
            www.Dispose();
            failure?.Invoke();
        }
    }
}
