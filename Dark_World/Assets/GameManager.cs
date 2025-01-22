using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using GoogleMobileAds.Api;

public class GameManager : MonoBehaviour, IDetailedStoreListener
{
    public static GameManager Instance;

    // IAP Product IDs
    private const string PRODUCT_REMOVE_ADS = "remove_ads";
    private const string PRODUCT_PREMIUM_SKINS = "premium_skins";
    private const string PRODUCT_PREMIUM_WEAPONS = "premium_weapons";
    private const string PRODUCT_POWERUPS = "power_ups";

    private bool adsEnabled = true; // Flag for ads
    private IStoreController storeController; // The Unity Purchasing system.
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;

    private string interstitialAdUnitId = "ca-app-pub-xxxxxxxxxxxxxxxx/interstitialAdId"; // Replace with your AdMob Interstitial Ad Unit ID
    private string rewardedAdUnitId = "ca-app-pub-xxxxxxxxxxxxxxxx/rewardedAdId";       // Replace with your AdMob Rewarded Ad Unit ID


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initialize the Google Mobile Ads SDK
        MobileAds.Initialize(initStatus => { Debug.Log("AdMob Initialized!"); });

        InitializeIAP();
    }

    #region IAP
    private void InitializeIAP()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(PRODUCT_REMOVE_ADS, ProductType.NonConsumable);
        builder.AddProduct(PRODUCT_PREMIUM_SKINS, ProductType.NonConsumable);
        builder.AddProduct(PRODUCT_PREMIUM_WEAPONS, ProductType.NonConsumable);
        builder.AddProduct(PRODUCT_POWERUPS, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public void PurchaseProduct(string productId)
    {
        if (storeController != null && storeController.products.WithID(productId) != null)
        {
            storeController.InitiatePurchase(productId);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("In-App Purchasing successfully initialized");
        storeController = controller;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        OnInitializeFailed(error, null);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        var errorMessage = $"Purchasing failed to initialize. Reason: {error}.";

        if (message != null)
        {
            errorMessage += $" More details: {message}";
        }

        Debug.Log(errorMessage);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (args.purchasedProduct.definition.id == PRODUCT_REMOVE_ADS)
        {
            adsEnabled = false;
            Debug.Log("Ads removed!");
        }
        else if (args.purchasedProduct.definition.id == PRODUCT_PREMIUM_SKINS)
        {
            UnlockPremiumSkins();
        }
        else if (args.purchasedProduct.definition.id == PRODUCT_PREMIUM_WEAPONS)
        {
            UnlockPremiumWeapons();
        }
        else if (args.purchasedProduct.definition.id == PRODUCT_POWERUPS)
        {
            GrantPowerUps();
        }

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.Log($"Purchase failed - Product: '{product.definition.id}'," +
            $" Purchase failure reason: {failureDescription.reason}," +
            $" Purchase failure details: {failureDescription.message}");
    }

    private void UnlockPremiumSkins()
    {
        // Unlock skins logic here
        Debug.Log("Premium skins unlocked!");
    }

    private void UnlockPremiumWeapons()
    {
        // Unlock weapons logic here
        Debug.Log("Premium weapons unlocked!");
    }

    private void GrantPowerUps()
    {
        // Grant power-ups logic here
        Debug.Log("Power-ups granted!");
    }
    #endregion

    #region Leaderboards
    public void SubmitScoreToLeaderboard(string leaderboardId, long score)
    {
        Social.ReportScore(score, leaderboardId, success =>
        {
            Debug.Log(success ? "Score submitted successfully!" : "Failed to submit score.");
        });
    }

    public void ShowLeaderboard()
    {
        if (Social.localUser.authenticated)
        {
            Social.ShowLeaderboardUI();
        }
        else
        {
            Debug.LogError("Not authenticated for leaderboards.");
        }
    }
    #endregion

    #region Social Sharing
    public void ShareAchievement(string message)
    {
        string shareText = $"I just achieved {message} in Stick Fighter vs Zombies! Can you beat my score?";
        string shareUrl = "https://yourgamewebsite.com"; // Replace with your game link

        // Share logic for Android/iOS (native sharing intent)
        new NativeShare().SetText(shareText).SetUrl(shareUrl).Share();
    }
    #endregion


}
