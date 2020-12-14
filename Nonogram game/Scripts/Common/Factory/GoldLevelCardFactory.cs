using System.Collections;
using System.Collections.Generic;
using Peak.QuixelLogic.Scripts.Settings;
using UnityEngine;

namespace Peak.QuixelLogic.Scripts.Common.Factory
{
    /// <summary>
    /// Factory script to be reused with objects that require pooling
    /// </summary>
    public sealed class GoldLevelCardFactory : MonoBehaviour
    {
        [SerializeField]
        private GameObject goldLevelCard;

        [SerializeField]
        private Transform prefabCacheTransform;

        public static GoldLevelCardFactory Instance { get; private set; }

        private Queue<GameObject> prefabPool;

        [SerializeField]
        private GlobalSettings globalSettings;

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

            //for (int index = 0; index < 6; index++)
            //{
            //    PutInstance(CreatePrefabInstance());
            //}
        }

        public GameObject GetInstance()
        {
            GameObject GoldLevelCard = prefabPool.Count > 0 ? prefabPool.Dequeue() : CreatePrefabInstance();

            GoldLevelCard.SetActive(true);

            return GoldLevelCard;
        }

        public void PutInstance(GameObject goldLevelCard)
        {
            if (goldLevelCard)
            {
                goldLevelCard.SetActive(false);
                goldLevelCard.transform.SetParent(prefabCacheTransform);
                goldLevelCard.transform.localPosition = Vector3.zero;
                goldLevelCard.transform.localRotation = Quaternion.identity;

                prefabPool.Enqueue(goldLevelCard);
            }
        }

        private GameObject CreatePrefabInstance()
        {
            return Instantiate(goldLevelCard, prefabCacheTransform);
        }
    }
}