using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Peak.Speedoku.Scripts.Common
{
    /// <summary>
    /// Manages particle system instances. Destroys after they are stopped playing
    /// </summary>
    public sealed class SoundFactory : MonoBehaviour
    {
        [SerializeField]
        private Transform soundRoot;

        private Dictionary<AudioSource, Queue<AudioSource>> soundPool;

        public static SoundFactory Instance { get; private set; }

        private void Awake()
        {
            if (Instance)
            {
                Debug.LogWarning("[SOUND FACTORY] SoundFactory duplicate instantiation.");
            }
            else
            {
                Instance = this;
            }

            soundPool = new Dictionary<AudioSource, Queue<AudioSource>>();
        }

        /// <summary>
        /// Gets a copy of the audio source that will not be interrupted
        /// </summary>
        /// <param name="sourceKey">Original audio source to copy</param>
        public void PlayAsCopy(AudioSource sourceKey)
        {
            if (!sourceKey)
            {
                Debug.LogWarning("Cannot play null for AudioSource");
                return;
            }

            if (!sourceKey.clip)
            {
                Debug.LogWarning("Cannot play null AudioClip for " + sourceKey.name);
                return;
            }

            Queue<AudioSource> item = GetOrCreateSourceList(sourceKey);
            AudioSource copy = GetOrCreateSourceItem(sourceKey, item);

            StartCoroutine(PlayThenReturnToQueue(sourceKey, copy));
        }

        private AudioSource GetOrCreateSourceItem(AudioSource sourceKey, Queue<AudioSource> item)
        {
            AudioSource copy = item.Count > 0
                ? item.Dequeue()
                : Instantiate(sourceKey, soundRoot);

            copy.name = sourceKey.name + "(Copy)";

            return copy;
        }

        private Queue<AudioSource> GetOrCreateSourceList(AudioSource sourceKey)
        {
            Queue<AudioSource> item = soundPool.ContainsKey(sourceKey)
                ? soundPool[sourceKey]
                : (soundPool[sourceKey] = new Queue<AudioSource>());

            return item;
        }

        private IEnumerator PlayThenReturnToQueue(AudioSource sourceKey, AudioSource copy)
        {
            copy.gameObject.SetActive(true);

            copy.clip = sourceKey.clip;
            copy.pitch = sourceKey.pitch;
            copy.Play();

            do
            {
                yield return new WaitForSeconds(0.1f);

            } while (copy.isPlaying);

            copy.gameObject.SetActive(false);
            soundPool[sourceKey].Enqueue(copy);
        }
    }
}