using System.Collections;
using System.Collections.Generic;
using Peak.QuixelLogic.Scripts.Settings;
using UnityEngine;

namespace Peak.QuixelLogic.Scripts.Common.Factory
{
    /// <summary>
    /// Factory script to be reused with objects that require pooling
    /// </summary>
    public sealed class LevelCardFactory : MonoBehaviour
    {
        [SerializeField]
        private GameObject levelCard;

        [SerializeField]
        private Transform prefabCacheTransform;

        public static LevelCardFactory Instance { get; private set; }

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

            //for (int index = 0; index < 30; index++)
            //{
            //    PutInstance(CreatePrefabInstance());
            //}
        }

        public GameObject GetInstance()
        {
            GameObject LevelCard = prefabPool.Count > 0 ? prefabPool.Dequeue() : CreatePrefabInstance();

            LevelCard.SetActive(true);

            return LevelCard;
        }

        public void PutInstance(GameObject levelCard)
        {
            if (levelCard)
            {
                levelCard.SetActive(false);
                levelCard.transform.SetParent(prefabCacheTransform);
                levelCard.transform.localPosition = Vector3.zero;
                levelCard.transform.localRotation = Quaternion.identity;

                prefabPool.Enqueue(levelCard);
            }
        }

        private GameObject CreatePrefabInstance()
        {
            return Instantiate(levelCard, prefabCacheTransform);
        }
    }
}