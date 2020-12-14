using System;
using System.Collections;
using System.Collections.Generic;
using Peak.QuixelLogic.Scripts.ScenesLogic;
using Peak.QuixelLogic.Scripts.Settings;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;

namespace Peak.QuixelLogic.Scripts.Common.IAP
{
    public class Purchaser : MonoBehaviour, IStoreListener
    {
        private static IStoreController m_StoreController;          // The Unity Purchasing system.
        private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

        // Product identifiers for all products capable of being purchased: 
        // "convenience" general identifiers for use with Purchasing, and their store-specific identifier 
        // counterparts for use with and outside of Unity Purchasing. Define store-specific identifiers 
        // also on each platform's publisher dashboard (iTunes Connect, Google Play Developer Console, etc.)

        // General product identifiers for the consumable, non-consumable, and subscription products.
        // Use these handles in the code to reference which product to purchase. Also use these values 
        // when defining the Product Identifiers on the store. Except, for illustration purposes, the 
        // kProductIDSubscription - it has custom Apple and Google identifiers. We declare their store-
        // specific mapping to Unity Purchasing's AddProduct, below.

        //public static string kProductIDConsumable = "consumable";
        public static string kProductIDNonConsumable = "nonconsumable";
        public static string kProductIDSubscription = "subscription";

        // Apple App Store-specific product identifier for the subscription product.
        private static string kProductNameAppleSubscription = "com.unity3d.subscription.new";

        // Google Play Store-specific product identifier subscription product.
        private static string kProductNameGooglePlaySubscription = "com.unity3d.subscription.original";

        private StoreSettings storeSettings;

        public static Purchaser Instance { get; private set; }

        public event Action<string> PurchaseCompleted;

        public event Action<string, PurchaseFailureReason> PurchaseFailed;

        private event Action StoreLoadComplete;
        private event Action StoreLoadError;

        // Maximum time out time in seconds
        private float maxTimeOut = 5.0f;

        //handle on internet ping
        private WWW www;

        // TimeOut Coroutine property
        private Coroutine timeOutCoroutine;

        // Internet Check Coroutine property
        private Coroutine internetCheckCoroutine;

        [SerializeField] private ConnectionUtil purchaseUtilities;

        /// <summary>
        /// Gets a value indicating whether the application is online.
        /// </summary>
        /// <value><c>true</c> if is online; otherwise, <c>false</c>.</value>
        private bool IsOnline => Application.internetReachability != NetworkReachability.NotReachable;

        private void Awake()
        {
            if (Instance) { }
            else
            {
                Instance = this;
            }
        }

        public void InitializePurchasing(Action completion, Action error)
        {

            //if (!IsOnline)
            //{
            //    Debug.Log("[IAP] NOT ONLINE");
            //    error?.Invoke();
            //    return;
            //}
            StoreLoadComplete = completion;
            StoreLoadError = error;
            purchaseUtilities.CheckInternet(maxTimeOut, LoadStore, ShowConnectionFailureMessage);
            Debug.Log("[IAP] Check connection");

            //if (www.isDone && www.bytesDownloaded > 0)
            //{
            //    LoadStore();
            //}
            //else
            //{
            //    ShowConnectionFailureMessage();
            //}
            //if (www != null && (www.uploadProgress > 0 || www.progress > 0))
            //{
            //    www.Dispose();
            //}
            //if (internetCheckCoroutine != null)
            //{
            //    StopCoroutine(internetCheckCoroutine);
            //}
            //internetCheckCoroutine = StartCoroutine(CheckInternetCoroutine());
        }

        private void LoadStore()
        {
            // If we have already connected to Purchasing ...
            if (IsInitialized())
            {
                // ... we are done here.
                return;
            }

            Debug.Log("[IAP] OnInitialized: START");
            storeSettings = SceneActivationBehaviour<StoreUIActivator>.Instance.StoreSettings;
            // Create a builder, first passing in a suite of Unity provided stores.
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            // Add a product to sell / restore by way of its identifier, associating the general identifier
            // with its store-specific identifiers.

            foreach (StoreProductSettings storeProduct in storeSettings.StoreProducts)
            {
                builder.AddProduct(storeProduct.ProductId, ProductType.Consumable);
            }

            //builder.AddProduct(kProductIDConsumable, ProductType.Consumable);

            // Continue adding the non-consumable product.

            //builder.AddProduct(kProductIDNonConsumable, ProductType.NonConsumable);

            // And finish adding the subscription product. Notice this uses store-specific IDs, illustrating
            // if the Product ID was configured differently between Apple and Google stores. Also note that
            // one uses the general kProductIDSubscription handle inside the game - the store-specific IDs 
            // must only be referenced here. 

            //builder.AddProduct(kProductIDSubscription, ProductType.Subscription, new IDs(){
            //{ kProductNameAppleSubscription, AppleAppStore.Name },
            //{ kProductNameGooglePlaySubscription, GooglePlay.Name },
            //});

            // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
            // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
            UnityPurchasing.Initialize(this, builder);
        }

        public bool IsInitialized()
        {
            // Only say we are initialized if both the Purchasing references are set.
            return m_StoreController != null && m_StoreExtensionProvider != null;
        }

        private string PurchasedItemID;

        public string GetItemPrice(string productId)
        {
            if (IsInitialized())
            {
                Product product = m_StoreController.products.WithID(productId);
                if (product != null && product.availableToPurchase)
                {
                    return product.metadata.localizedPriceString;
                }
            }
            else
            {
                Debug.LogError("[STORE] GetItemPrice FAIL. Not initialized.");
            }
            return "N/A";
        }

        public void BuyConsumable(string productID)
        {
            print($"Purchaser - Buy consumable: {productID}");
            PurchasedItemID = productID;

            // Buy the consumable product using its general identifier. Expect a response either 
            // through ProcessPurchase or OnPurchaseFailed asynchronously.
            BuyProductID(productID);
        }


        public void BuyNonConsumable()
        {
            // Buy the non-consumable product using its general identifier. Expect a response either 
            // through ProcessPurchase or OnPurchaseFailed asynchronously.
            BuyProductID(kProductIDNonConsumable);
        }


        public void BuySubscription()
        {
            // Buy the subscription product using its the general identifier. Expect a response either 
            // through ProcessPurchase or OnPurchaseFailed asynchronously.
            // Notice how we use the general product identifier in spite of this ID being mapped to
            // custom store-specific identifiers above.
            BuyProductID(kProductIDSubscription);
        }

        void BuyProductID(string productId)
        {
            // If Purchasing has been initialized ...
            if (IsInitialized())
            {
                // ... look up the Product reference with the general product identifier and the Purchasing system's products collection.
                Product product = m_StoreController.products.WithID(productId);
                //QLPurchaseSource source = SceneActivationBehaviour<StoreUIActivator>.Instance.StoreOpenSource;
                SceneActivationBehaviour<GameLogicActivator>.Instance.AnalyticsController.SendPurchaseEvent(AnalyticsScripts.QLPurchaseState.QLPurchaseStateStarted,
                    product.definition.id,
                    (float)product.metadata.localizedPrice,
                    product.metadata.isoCurrencyCode,
                    SceneActivationBehaviour<StoreUIActivator>.Instance.CoinsForProduct(product.definition.id),
                    SceneActivationBehaviour<StoreUIActivator>.Instance.StoreOpenSource);
                // If the look up found a product for this device's store and that product is ready to be sold ... 
                if (product != null && product.availableToPurchase)
                {
                    Debug.Log(string.Format("[IAP] Purchasing product asychronously: '{0}'", product.definition.id));
                    // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                    // asynchronously.
                    m_StoreController.InitiatePurchase(product);
                }
                // Otherwise ...
                else
                {
                    // ... report the product look-up failure situation  
                    Debug.Log("[IAP] BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                }
            }
            // Otherwise ...
            else
            {
                // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
                // retrying initiailization.
                Debug.Log("[IAP] BuyProductID FAIL. Not initialized.");
            }
        }

        // Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
        // Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
        public void RestorePurchases()
        {
            // If Purchasing has not yet been set up ...
            if (!IsInitialized())
            {
                // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
                Debug.Log("RestorePurchases FAIL. Not initialized.");
                return;
            }

            // If we are running on an Apple device ... 
            if (Application.platform == RuntimePlatform.IPhonePlayer ||
                Application.platform == RuntimePlatform.OSXPlayer)
            {
                // ... begin restoring purchases
                Debug.Log("[IAP] RestorePurchases started ...");

                // Fetch the Apple store-specific subsystem.
                var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
                // Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
                // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
                apple.RestoreTransactions((result) =>
                {
                    // The first phase of restoration. If no more responses are received on ProcessPurchase then 
                    // no purchases are available to be restored.
                    Debug.Log("[IAP] RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
                });
            }
            // Otherwise ...
            else
            {
                // We are not running on an Apple device. No work is necessary to restore purchases.
                Debug.Log("[IAP] RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
            }
        }

        //  
        // --- IStoreListener
        //

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            // Purchasing has succeeded initializing. Collect our Purchasing references.
            Debug.Log("[IAP] OnInitialized: PASS");

            // Overall Purchasing system, configured with products for this application.
            m_StoreController = controller;
            // Store specific subsystem, for accessing device-specific store features.
            m_StoreExtensionProvider = extensions;
            StoreLoadComplete?.Invoke();
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
            Debug.Log("[IAP] OnInitializeFAILED InitializationFailureReason: " + error);
            StoreLoadError?.Invoke();
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            bool validPurchase = true; // Presume valid for platforms with no R.V.

            // Unity IAP's validation logic is only included on these platforms.
#if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX
            // Prepare the validator with the secrets we prepared in the Editor
            // obfuscation window.
            var validator = new CrossPlatformValidator(GooglePlayTangle.Data(),
                AppleTangle.Data(), Application.identifier);

            try
            {
                // On Google Play, result has a single product ID.
                // On Apple stores, receipts contain multiple products.
                var result = validator.Validate(args.purchasedProduct.receipt);
                // For informational purposes, we list the receipt(s)
                Debug.Log("Receipt is valid. Contents:");
                foreach (IPurchaseReceipt productReceipt in result)
                {
                    Debug.Log(productReceipt.productID);
                    Debug.Log(productReceipt.purchaseDate);
                    Debug.Log(productReceipt.transactionID);
                }
            }
            catch (IAPSecurityException)
            {
                Debug.Log($"Invalid receipt, not unlocking content ");
                validPurchase = false;
            }
#elif UNITY_EDITOR
            Debug.Log("Receipt is valid. EDITOR:");
            validPurchase = true;
#endif

            // A consumable product has been purchased by this user.
            if (String.Equals(args.purchasedProduct.definition.id, PurchasedItemID, StringComparison.Ordinal) && validPurchase)
            {
                PurchaseCompleted?.Invoke(args.purchasedProduct.definition.id);
                SceneActivationBehaviour<GameLogicActivator>.Instance.AnalyticsController.SendPurchaseEvent(AnalyticsScripts.QLPurchaseState.QLPurchaseStateSucceded,
                    args.purchasedProduct.definition.id,
                    (float)args.purchasedProduct.metadata.localizedPrice,
                    args.purchasedProduct.metadata.isoCurrencyCode,
                    SceneActivationBehaviour<StoreUIActivator>.Instance.CoinsForProduct(args.purchasedProduct.definition.id),
                    SceneActivationBehaviour<StoreUIActivator>.Instance.StoreOpenSource);
                Debug.Log(string.Format("[IAP] ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                // The consumable item has been successfully purchased, add 100 coins to the player's in-game score.
                //ScoreManager.score += 100;
            }
            // Or ... a non-consumable product has been purchased by this user.
            //else if (String.Equals(args.purchasedProduct.definition.id, kProductIDNonConsumable, StringComparison.Ordinal))
            //{
            //    Debug.Log(string.Format("[IAP] ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            //    // TODO: The non-consumable item has been successfully purchased, grant this item to the player.
            //}
            // Or ... a subscription product has been purchased by this user.
            //else if (String.Equals(args.purchasedProduct.definition.id, kProductIDSubscription, StringComparison.Ordinal))
            //{
            //    Debug.Log(string.Format("[IAP] ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            //    // TODO: The subscription item has been successfully purchased, grant this to the player.
            //}
            // Or ... an unknown product has been purchased by this user. Fill in additional products here....
            else
            {
                Debug.Log(string.Format("[IAP] ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
            }

            // Return a flag indicating whether this product has completely been received, or if the application needs 
            // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
            // saving purchased products to the cloud, and when that save is delayed. 
            return PurchaseProcessingResult.Complete;
        }


        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            PurchaseFailed?.Invoke(product.definition.id, failureReason);
            SceneActivationBehaviour<GameLogicActivator>.Instance.AnalyticsController.SendPurchaseEvent(AnalyticsScripts.QLPurchaseState.QLPurchaseStateFailed,
                   product.definition.id,
                   (float)product.metadata.localizedPrice,
                   product.metadata.isoCurrencyCode,
                   SceneActivationBehaviour<StoreUIActivator>.Instance.CoinsForProduct(product.definition.id),
                   SceneActivationBehaviour<StoreUIActivator>.Instance.StoreOpenSource);
            // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
            // this reason with the user to guide their troubleshooting actions.
            Debug.Log(string.Format("[IAP] OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
        }

        private IEnumerator CheckInternetCoroutine()
        {
            www = new WWW("google.com");
            timeOutCoroutine = StartCoroutine(CheckTimeOutCoroutine());

            yield return www;

            StopCoroutine(timeOutCoroutine);

            if (www.isDone && www.bytesDownloaded > 0)
            {
                LoadStore();
            }
            else
            {
                ShowConnectionFailureMessage();
            }
        }

        private IEnumerator CheckTimeOutCoroutine()
        {
            yield return new WaitForSeconds(maxTimeOut);
            StopCoroutine(internetCheckCoroutine);
            internetCheckCoroutine = null;
            www.Dispose();
            ShowConnectionFailureMessage();
        }

        private void ShowConnectionFailureMessage()
        {
            StoreLoadError?.Invoke();
        }
    }
}