using System;
using System.Collections;
using System.Collections.Generic;
using Peak.QuixelLogic.Scripts.ScenesLogic;
using Peak.QuixelLogic.Scripts.Settings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Peak.QuixelLogic.Scripts.Common.IAP
{
    public sealed class StoreItemScript : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI priceLabel;

        [SerializeField]
        private TextMeshProUGUI productName;

        [SerializeField]
        private Image productImage;

        [SerializeField]
        private ButtonController purchaseButton;

        [SerializeField]
        private GameObject mostPopularBanner;

        [SerializeField]
        private GameObject removesAdsText;

        [SerializeField]
        private GameObject removesAdsBackground;

        //public event Action<int, Vector3> CoinsPurchaseConfirmed;

        private StoreProductSettings settings;

        public bool isMostPopular => settings != null && settings.IsMostPopular;

        public string purchaseId => settings.ProductId;

        public void SetButtonEnabled(bool enabled)
        {
            purchaseButton.SetInteractability(enabled);
        }

        public void SetUpValues(StoreProductSettings productSettings, Purchaser purchaser)
        {
            this.settings = productSettings;

            priceLabel.text = purchaser.GetItemPrice(productSettings.ProductId);

            productName.text = settings.LocalisableItemNameKey; // TODO: = LocalisationSystem.GetKey (settings.LocalisableItemNameKey)
            productImage.sprite = settings.ProductImage;
            mostPopularBanner.gameObject.SetActive(settings.IsMostPopular);
            removesAdsText.SetActive(settings.RemovesAds);
            removesAdsBackground.SetActive(settings.RemovesAds);
        }

        public void TryPurchaseItem()
        {
            if (settings.ProductId != null)
            {
                Purchaser.Instance.BuyConsumable(settings.ProductId);
                SceneActivationBehaviour<StoreUIActivator>.Instance.SetButtons(false);
#if UNITY_EDITOR
                return;
#endif
                SceneActivationBehaviour<StoreUIActivator>.Instance.EnableLoadingAnimation(true);
            }
        }





    }
}