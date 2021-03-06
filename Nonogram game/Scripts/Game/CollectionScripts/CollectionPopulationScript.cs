using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Peak.QuixelLogic.Scripts.Settings;
using Peak.QuixelLogic.Scripts.Common;
using Peak.QuixelLogic.Scripts.Common.AnalyticsScripts;
using Peak.QuixelLogic.Scripts.ScenesLogic;
using Peak.QuixelLogic.Scripts.Game.Gameplay;
using Peak.QuixelLogic.Scripts.Autogenerated;
using System.Linq;
using System;
using Peak.QuixelLogic.Scripts.Game;
using System.Text;
using UnityEngine.UI;

namespace Peak.QuixelLogic.Scripts.Game.CollectionScripts
{
    public class CollectionPopulationScript : MonoBehaviour
    {
        [SerializeField]
        private GlobalSettings globalSettings;

        [SerializeField]
        private GameObject bottomSpaceFiller;

        private List<CollectionObjectScript> collectionObjects = new List<CollectionObjectScript>();

        [SerializeField]
        private ScrollRect scrollRect;

        [SerializeField]
        private RectTransform contentPanel;

        private List<string> _matchingIDs = new List<string>(1000);

        private List<string> _matchingGoldIDs = new List<string>(1000);

        public void Awake()
        {
            for (int i = 0; i < globalSettings.levelGroupingSettings.Length; i++)
            {
                CollectionObjectScript spawnedCollection = Instantiate((globalSettings.levelGroupingSettings[i].CollectionPrefab), this.transform).GetComponent<CollectionObjectScript>();

                collectionObjects.Add(spawnedCollection);
                spawnedCollection.gameObject.name += "" + i.ToString();

                spawnedCollection.PopulateLevels(globalSettings.levelGroupingSettings[i]); // run level population function on collection object

                bottomSpaceFiller.transform.SetSiblingIndex(i + 1);
            }
        }

        private float timeSinceLastCalled;
        private float delay = 0.1f;

        private void Update()
        {
            timeSinceLastCalled += Time.deltaTime;

            if (timeSinceLastCalled > delay)
            {
                foreach (CollectionObjectScript collection in collectionObjects)
                {
                    collection.Check();
                }
                timeSinceLastCalled = 0f;
                return;
            }
        }

        public void StopScrollMovement()
        {
            scrollRect.StopMovement();
        }

        public void ShowGoldCardsUnlocking(int groupCompleted)
        {
            SoundController.Instance.PlayAudioClip(Settings.Autogenerated.SoundSettingsKey.Goldcardsreveal);

            SceneActivationBehaviour<OverlayUISceneActivator>.Instance.ToggleBlocker(true);

            foreach (GoldLevelCardScript goldLevel in collectionObjects[groupCompleted].goldCardScripts)
            {
                goldLevel.thisGoldCardState = GoldLevelCardScript.GoldCardState.Unlocked;
            }
        }

        public void CheckGoldsForCollection(int groupFromWhichGoldCardWasCompleted)
        {
            int completeGolds = 0;

            for (int i = 0; i < collectionObjects[groupFromWhichGoldCardWasCompleted].goldCardScripts.Count; i++)
            {
                if (collectionObjects[groupFromWhichGoldCardWasCompleted].goldCardScripts[i].thisGoldCardState == GoldLevelCardScript.GoldCardState.Complete)
                {
                    completeGolds += 1;
                }
            }

            if (completeGolds.Equals(collectionObjects[groupFromWhichGoldCardWasCompleted].goldCardScripts.Count))
            {
                SceneActivationBehaviour<CollectionScreenActivator>.Instance.ShowCollectionEvent(CollectionScreenActivator.CollectionEvent.CollectionComplete);
                return;
            }
            else return;
        }

        public IEnumerator SnapViewport(float groupIndex, float levelIndex, bool animate = false, bool completeCollection = false)
        {
            yield return new WaitForSeconds(0.25f);

            StopScrollMovement();
            Canvas.ForceUpdateCanvases();
            float finalScrollValue = 0;

            if (!completeCollection)
            {
                float row = Mathf.Ceil(levelIndex / 3);
                finalScrollValue = ((GameConstants.CollectionView.LevelRowSize * row) + (GameConstants.CollectionView.CollectionHeaderSize * groupIndex)) - (Screen.height / 5);
                collectionObjects[(int)groupIndex].SetCollectionWindow(true);
            }
            else
            {
                // complete collection
                levelIndex -= (globalSettings.levelGroupingSettings[(int)groupIndex].Levels.Count + globalSettings.levelGroupingSettings[(int)groupIndex].GoldLevels.Count);

                float row = Mathf.Ceil(levelIndex / 3);
                finalScrollValue = ((GameConstants.CollectionView.LevelRowSize * row) + (GameConstants.CollectionView.CollectionHeaderSize * groupIndex));// - (GameConstants.CollectionView.LevelRowSize * 2);
                collectionObjects[(int)groupIndex].SetCollectionWindow(true);
            }

            if (!animate)
            {
                contentPanel.anchoredPosition = new Vector2(0, finalScrollValue);
            }
            else
            {
                if (!completeCollection && groupIndex > 0)
                {
                    float currentViewportYpos = contentPanel.localPosition.y;
                    StartCoroutine(animatedScrollDown(finalScrollValue, currentViewportYpos));
                }
                else
                {
                    float currentViewportYpos = contentPanel.localPosition.y;
                    StartCoroutine(animatedScrollUp(finalScrollValue, currentViewportYpos, collectionObjects[(int)groupIndex]));
                }
            }

            yield break;
        }

        private IEnumerator animatedScrollUp(float newYValue, float currentViewportYpos, CollectionObjectScript collection)
        {
            yield return new WaitForSeconds(1f);

            while (true)
            {
                contentPanel.anchoredPosition = new Vector2(0, currentViewportYpos);
                currentViewportYpos -= 10 * (Time.deltaTime * 200);

                yield return new WaitForEndOfFrame();

                if (currentViewportYpos <= newYValue)
                {
                    // show sparkles & tick
                    collection.Tick();
                    collection.ShowSparkles();

                    yield return new WaitForSeconds(2);

                    SceneActivationBehaviour<OverlayUISceneActivator>.Instance.ToggleBlocker(false);
                    yield break;
                }
            }
        }

        private IEnumerator animatedScrollDown(float newYValue, float currentViewportYpos)
        {
            yield return new WaitForSeconds(1f);

            while (true)
            {
                contentPanel.anchoredPosition = new Vector2(0, currentViewportYpos);
                currentViewportYpos += 10 * (Time.deltaTime * 200);

                yield return new WaitForEndOfFrame();

                if (currentViewportYpos >= newYValue)
                {
                    SceneActivationBehaviour<OverlayUISceneActivator>.Instance.ToggleBlocker(false);
                    yield break;
                }
            }
        }

        // split progress into unique IDs
        List<string> uniqueIDs = new List<string>(1000);
        List<string> goldUniqueIDs = new List<string>(1000);
        List<string> matchingIDs = new List<string>(1000);
        List<string> matchingGoldIDs = new List<string>(1000);

        public void LoadProgress(Player player)
        {
            int levelIndex = player.CurrentLevelInGroupIndex;
            int currentGroup = player.GroupIndex;

            string s = player.LevelProgress;
            string[] values = s.Split(',');

            uniqueIDs.Clear();
            goldUniqueIDs.Clear();

            for (int i = 0; i < values.Length; i++)
            {
                if (globalSettings.LevelOrderSettings.RuleSettings[i].IsGold)
                {
                    goldUniqueIDs.Add(values[i]);
                }
                else
                {
                    uniqueIDs.Add(values[i]);
                }
            }

            // send collection objects the valid IDs and next level to be played
            string currentLevelID = globalSettings.levelGroupingSettings[currentGroup].Levels[levelIndex].UniqueID; // get uniqueID of next level to be played

            matchingIDs.Clear();
            matchingGoldIDs.Clear();

            for (int i = 0; i < collectionObjects.Count; i++)
            {
                matchingIDs = GetMatchingIDs(uniqueIDs, i);
                matchingGoldIDs = GetMatchingGoldIDs(goldUniqueIDs, i);

                collectionObjects[i].LoadProgress(matchingIDs, matchingGoldIDs, currentLevelID);
            }
            InterfaceController.Instance.Hide(GameWindow.CollectionScreen);
        }

        private List<string> GetMatchingIDs(List<string> _uniqueIDs, int collection)
        {
            _matchingIDs.Clear();
            for (int i = 0; i < _uniqueIDs.Count; i++)
            {
                for (int j = 0; j < collectionObjects[collection].uniqueIDs.Count; j++)
                {
                    if (_uniqueIDs[i].Contains(collectionObjects[collection].uniqueIDs[j]))
                    {
                        _matchingIDs.Add(_uniqueIDs[i]);
                    }
                }
            }

            return _matchingIDs;
        }

        private List<string> GetMatchingGoldIDs(List<string> _uniqueGoldIDs, int collection)
        {
            matchingGoldIDs.Clear();

            for (int i = 0; i < _uniqueGoldIDs.Count; i++)
            {
                for (int j = 0; j < collectionObjects[collection].uniqueGoldIDs.Count; j++)
                {
                    if (_uniqueGoldIDs[i].Contains(collectionObjects[collection].uniqueGoldIDs[j]))
                    {
                        _matchingGoldIDs.Add(_uniqueGoldIDs[i]);
                    }
                }
            }

            return _matchingGoldIDs;
        }
    }
}
