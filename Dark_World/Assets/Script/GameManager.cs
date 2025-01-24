using System;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using GoogleMobileAds.Api;
using UnityEngine.Networking;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class GameManager : MonoBehaviour, IDetailedStoreListener
{
    private FirebaseAuth auth;
    private FirebaseUser user;
    private DatabaseReference databaseRef;
    private string apiUrl = "https://yourapi.com/leaderboard"; // Replace with your actual API URL

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
        auth = FirebaseAuth.DefaultInstance;
        databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        SignInAnonymously();
        InitializeIAP();
    }

    private void SignInAnonymously()
    {
        auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                user = auth.CurrentUser;
                Debug.Log("Signed in anonymously as: " + user.UserId);
            }
            else
            {
                Debug.LogError("Failed to sign in anonymously: " + task.Exception);
            }
        });
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
        else
        {
            Debug.LogError("Purchase failed: StoreController not initialized or product not found.");
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
        if (user != null)
        {
            StartCoroutine(FetchAndStoreLeaderboard(user.UserId, leaderboardId, score));
        }
        else
        {
            Debug.LogError("User not authenticated!");
        }
    }

    private IEnumerator FetchAndStoreLeaderboard(string playerId, string leaderboardId, long score)
    {
        WWWForm form = new WWWForm();
        form.AddField("playerId", playerId);
        form.AddField("leaderboardId", leaderboardId);
        form.AddField("score", score.ToString());

        using (UnityWebRequest request = UnityWebRequest.Post(apiUrl + "/submit", form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Score submitted successfully to API!");
                StoreScoreInFirebase(playerId, leaderboardId, score);
            }
            else
            {
                Debug.LogError("Failed to submit score to API: " + request.error);
            }
        }
    }

    private void StoreScoreInFirebase(string playerId, string leaderboardId, long score)
    {
        databaseRef.Child("leaderboards").Child(leaderboardId).Child(playerId).SetValueAsync(score)
            .ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("Score stored successfully in Firebase!");
                }
                else
                {
                    Debug.LogError("Failed to store score in Firebase: " + task.Exception);
                }
            });
    }

    public void FetchLeaderboard(string leaderboardId, Action<string> callback)
    {
        databaseRef.Child("leaderboards").Child(leaderboardId).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                callback?.Invoke(snapshot.GetRawJsonValue());
            }
            else
            {
                Debug.LogError("Failed to fetch leaderboard: " + task.Exception);
                callback?.Invoke(null);
            }
        });
    }
    #endregion

    #region Social Sharing
    public void ShareAchievement(string leaderboardId)
    {
        if (user != null)
        {
            databaseRef.Child("leaderboards").Child(leaderboardId).Child(user.UserId).GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    long score = snapshot.Exists ? long.Parse(snapshot.Value.ToString()) : 0;
                    string shareText = $"I just achieved a score of {score} in DarkWorld! Can you beat my score?";
                    string shareUrl = "https://yourgamewebsite.com"; // Replace with your game link

                    //   new NativeShare().SetText(shareText).SetUrl(shareUrl).Share();
                }
                else
                {
                    Debug.LogError("Failed to fetch score for sharing: " + task.Exception);
                }
            });
        }
        else
        {
            Debug.LogError("User not authenticated!");
        }
    }
    #endregion
}
