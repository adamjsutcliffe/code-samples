using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Peak.QuixelLogic.Scripts.Game.CollectionScripts
{
    public sealed class CollectionWindowScript : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> levelCards = new List<GameObject>();

        [SerializeField]
        private List<GameObject> goldLevelCards = new List<GameObject>();

        private void OnEnable()
        {
            for (int i = 0; i < levelCards.Count; i++)
            {
                levelCards[i].SetActive(true);
            }
            for (int i = 0; i < goldLevelCards.Count; i++)
            {
                goldLevelCards[i].SetActive(true);
            }
            return;
        }

        private void OnDisable()
        {
            for (int i = 0; i < levelCards.Count; i++)
            {
                levelCards[i].SetActive(false);
            }
            for (int i = 0; i < goldLevelCards.Count; i++)
            {
                goldLevelCards[i].SetActive(false);
            }
            return;
        }
    }
}