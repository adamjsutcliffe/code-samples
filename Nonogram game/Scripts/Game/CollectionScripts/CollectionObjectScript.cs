using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Peak.QuixelLogic.Scripts.Settings;
using Peak.QuixelLogic.Scripts.ScenesLogic;
using TMPro;
using Peak.QuixelLogic.Scripts.Game;
using System.Text;
using System;
using Peak.QuixelLogic.Scripts.Common;

namespace Peak.QuixelLogic.Scripts.Game.CollectionScripts
{
    /// <summary>
    /// Script on collection group object.
    /// </summary>
    public sealed class CollectionObjectScript : MonoBehaviour
    {
        public enum CollectionState
        {
            NotComplete,
            MainLevelsComplete,
            AllLevelsComplete
        }

        [SerializeField]
        private List<GameObject> levelCards = new List<GameObject>();

        [SerializeField]
        private List<GameObject> goldLevelCards = new List<GameObject>();

        [SerializeField]
        private GameObject hangerWire;

        [SerializeField]
        private TextMeshProUGUI collectionName;

        [SerializeField]
        private CollectionWindowScript collectionWindow;

        //List to hold unique IDs of levels in this collection group
        public List<string> uniqueIDs = new List<string>();

        public List<string> uniqueGoldIDs = new List<string>();

        public List<LevelCardScript> cardScripts = new List<LevelCardScript>();

        public List<GoldLevelCardScript> goldCardScripts = new List<GoldLevelCardScript>();

        private LevelGroupingSettings levelGroup;

        [SerializeField]
        private GameObject tickImage;

        [SerializeField]
        private GameObject sparkles;

        [SerializeField]
        private RectTransform rect;

        public void PopulateLevels(LevelGroupingSettings LevelGroup)
        {
            levelGroup = LevelGroup;

            for (int i = 0; i < levelGroup.Levels.Count; i++)
            {
                LevelCardScript activeLevelCard = levelCards[i].GetComponent<LevelCardScript>();
                activeLevelCard.PopulateLevelInformation(levelGroup.Levels[i], int.Parse(levelGroup.Id), levelGroup.boardBackgroundImage);
                uniqueIDs.Add(levelGroup.Levels[i].UniqueID);
                cardScripts.Add(activeLevelCard);
            }

            if (levelGroup.GoldLevels.Count > 0 && goldLevelCards.Count > 0)
            {
                for (int i = 0; i < levelGroup.GoldLevels.Count; i++)
                {
                    GoldLevelCardScript activeGoldLevelCard = goldLevelCards[i].GetComponent<GoldLevelCardScript>();
                    activeGoldLevelCard.PopulateLevelInformation(levelGroup.GoldLevels[i], int.Parse(levelGroup.Id), levelGroup.boardBackgroundImage);
                    uniqueGoldIDs.Add(levelGroup.GoldLevels[i].UniqueID);
                    goldCardScripts.Add(activeGoldLevelCard);
                }
            }
            else return;
        }

        public void SetCollectionWindow(bool show)
        {
            collectionWindow.gameObject.SetActive(show);
        }

        public void Check()
        {
            if (is_rectTransformsOverlap(SceneActivationBehaviour<CollectionScreenActivator>.Instance.RootCamera, rect))
            {
                collectionWindow.gameObject.SetActive(true);
                return;
            }
            else
            {
                collectionWindow.gameObject.SetActive(false);
                return;
            }
        }

        public bool is_rectTransformsOverlap(Camera cam, RectTransform elem = null, RectTransform viewport = null)
        {
            Vector2 viewportMinCorner;
            Vector2 viewportMaxCorner;

            if (viewport != null)
            {
                Vector3[] v_wcorners = new Vector3[4];
                viewport.GetWorldCorners(v_wcorners); //bot left, top left, top right, bot right

                viewportMinCorner = cam.WorldToScreenPoint(v_wcorners[0]);
                viewportMaxCorner = cam.WorldToScreenPoint(v_wcorners[2]);
            }
            else
            {
                viewportMinCorner = new Vector2(0, 0 - (Screen.height / 2));
                viewportMaxCorner = new Vector2(Screen.width, Screen.height + (Screen.height / 2));
            }

            viewportMinCorner += Vector2.one;
            viewportMaxCorner -= Vector2.one;

            Vector3[] e_wcorners = new Vector3[4];
            rect.GetWorldCorners(e_wcorners);

            Vector2 elem_minCorner = cam.WorldToScreenPoint(e_wcorners[0]);
            Vector2 elem_maxCorner = cam.WorldToScreenPoint(e_wcorners[2]);

            if (elem_minCorner.x > viewportMaxCorner.x) { return false; } //completely outside (to the right)
            if (elem_minCorner.y > viewportMaxCorner.y) { return false; } //completely outside (is above)

            if (elem_maxCorner.x < viewportMinCorner.x) { return false; } //completely outside (to the left)
            if (elem_maxCorner.y < viewportMinCorner.y) { return false; } //completely outside (is below)

            return true; //passed all checks, is inside (at least partially)
        }

        public void LoadProgress(List<string> matchingIDs, List<string> matchingGoldIDs, string levelToBePlayedID)
        {
            int possibleCompleteLevels = cardScripts.Count + goldCardScripts.Count;
            int completedLevels = 0;
            int completeGoldLevels = 0;

            for (int i = 0; i < matchingIDs.Count; i++)
            {
                if (int.Parse(matchingIDs[i].Substring(matchingIDs[i].Length - 1)) != 0)
                {
                    completedLevels += 1;
                }

                for (int card = 0; card < cardScripts.Count; card++)
                {
                    if (matchingIDs[i].Contains(cardScripts[card].UniqueID))
                    {
                        cardScripts[card].LoadPlayerLevelProgress(matchingIDs[i], matchingIDs[i].Contains(levelToBePlayedID), levelGroup.Locked, levelGroup.LevelCardSprite);
                    }
                }
            }

            for (int i = 0; i < matchingGoldIDs.Count; i++)
            {
                if (matchingGoldIDs[i].Substring(matchingGoldIDs[i].Length - 1).Equals("1") ||
                    matchingGoldIDs[i].Substring(matchingGoldIDs[i].Length - 1).Equals("2") ||
                    matchingGoldIDs[i].Substring(matchingGoldIDs[i].Length - 1).Equals("3"))
                {
                    completeGoldLevels += 1;
                }

                for (int goldCard = 0; goldCard < goldCardScripts.Count; goldCard++)
                {
                    if (matchingGoldIDs[i].Contains(goldCardScripts[goldCard].UniqueID))
                    {
                        goldCardScripts[goldCard].LoadPlayerLevelProgress(matchingGoldIDs[i], levelGroup.Locked, levelGroup.LevelCardSprite);
                    }
                }
            }

            if (completedLevels.Equals(cardScripts.Count) && completeGoldLevels.Equals(goldLevelCards.Count)) // all main levels done
            {
                Tick();
            }

            completedLevels += completeGoldLevels;
            SetCollectionName(completedLevels, possibleCompleteLevels);
        }

        private void SetCollectionName(int completedLevels, int possibleCompleteLevels)
        {
            StringBuilder builder = new StringBuilder();
            if (levelGroup.Locked)
            {
                builder.Append(string.Concat($"{GameConstants.CollectionView.Locked}", " (", completedLevels.ToString(), "/", possibleCompleteLevels, ")"));
            }
            else
            {
                // TODO: possibly change collection constant to levelgroup.collectionnamekey
                builder.Append(string.Concat($"{GameConstants.CollectionView.Collection} {levelGroup.Id}", " (", completedLevels.ToString(), "/", possibleCompleteLevels, ")"));
            }
            collectionName.text = builder.ToString();
        }

        public void Tick()
        {
            tickImage.SetActive(true);
            hangerWire.SetActive(false);

            foreach (LevelCardScript levelCard in cardScripts)
            {
                levelCard.HideClip();
            }

            foreach (GoldLevelCardScript goldLevelCard in goldCardScripts)
            {
                goldLevelCard.HideClip();
            }
        }

        public void ShowSparkles()
        {
            sparkles.SetActive(true);
        }
    }
}