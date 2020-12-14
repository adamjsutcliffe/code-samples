using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Peak.Speedoku.Scripts.Common
{
    public class ConnectionUtil : MonoBehaviour
    {
        //handle on internet ping
        private UnityWebRequest webRequest;

        // TimeOut Coroutine property
        private Coroutine timeOutCoroutine;

        // Internet Check Coroutine property
        private Coroutine internetCheckCoroutine;

        private const string uri = "https://google.com";

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
            timeOutCoroutine = StartCoroutine(CheckTimeOutCoroutine(timeout, failure));
            using (webRequest = UnityWebRequest.Get(uri))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();
                StopCoroutine(timeOutCoroutine);
                string[] pages = uri.Split('/');
                int page = pages.Length - 1;

                if (webRequest.isNetworkError)
                {
                    Debug.Log(pages[page] + ": Error: " + webRequest.error);
                    failure?.Invoke();
                }
                else
                {
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    success?.Invoke();
                }
            }
        }

        private IEnumerator CheckTimeOutCoroutine(float timeout, Action failure = null)
        {
            yield return new WaitForSeconds(timeout);
            StopCoroutine(internetCheckCoroutine);
            internetCheckCoroutine = null;
            webRequest.Dispose();
            failure?.Invoke();
        }
    }
}
