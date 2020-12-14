using System;
using UnityEngine;

namespace Peak.QuixelLogic.Scripts.Settings
{
    [Serializable]
    public sealed class StoreProductSettings : ScriptableObject
    {
        [Tooltip("Real product ID for mapping")]
        public string ProductId;

        public bool HideInStore;

        public string LocalisableItemNameKey;

        public Sprite ProductImage;

        public int CoinAmount;

        public bool RemovesAds;

        public bool IsMostPopular;
    }
}