using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Peak.Speedoku.Scripts.Common.Factory
{
    /// <summary>
    /// Factory script to be reused with objects that require pooling
    /// </summary>
    public sealed class FactoryScript : MonoBehaviour
    {
        [SerializeField]
        private GameObject prefab;

        [SerializeField]
        private Transform prefabCacheTransform;

        public static FactoryScript Instance { get; private set; }

        private Queue<GameObject> prefabPool;

        private void Awake()
        {
            if (Instance)
            {
                Debug.LogWarning("[FACTORY] Factory duplicate instantiation.");
            }
            else
            {
                Instance = this;
            }

            prefabPool = new Queue<GameObject>();

            for (int index = 0; index < Constants.DefaultPoolSize; index++)
            {
                PutInstance(CreatePrefabInstance());
            }
        }

        public GameObject GetInstance()
        {
            GameObject coin = prefabPool.Count > 0 ? prefabPool.Dequeue() : CreatePrefabInstance();

            coin.SetActive(true);

            return coin;
        }

        public void PutInstance(GameObject coin)
        {
            if (coin)
            {
                coin.SetActive(false);
                coin.transform.SetParent(prefabCacheTransform);
                coin.transform.localPosition = Vector3.zero;
                coin.transform.localRotation = Quaternion.identity;

                prefabPool.Enqueue(coin);
            }
        }

        private GameObject CreatePrefabInstance()
        {
            return Instantiate(prefab, prefabCacheTransform);
        }
    }
}


