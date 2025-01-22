/*using UnityEngine;
using GoogleMobileAds.Api; // Google AdMob
using System;

public class AdMobManager : MonoBehaviour
{
    public static AdMobManager Instance;

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

        RequestInterstitialAd();
        RequestRewardedAd();
    }

    #region Interstitial Ads
    private void RequestInterstitialAd()
    {
        interstitialAd = new InterstitialAd(interstitialAdUnitId);
        interstitialAd.OnAdClosed += HandleInterstitialClosed;

        // Request a new ad
        AdRequest adRequest = new AdRequest.Builder().Build();
        interstitialAd.LoadAd(adRequest);
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd.IsLoaded())
        {
            interstitialAd.Show();
        }
        else
        {
            Debug.LogWarning("Interstitial ad is not ready yet!");
            RequestInterstitialAd(); // Request a new ad
        }
    }

    private void HandleInterstitialClosed(object sender, EventArgs args)
    {
        Debug.Log("Interstitial ad closed.");
        RequestInterstitialAd(); // Reload ad after closing
    }
    #endregion

    #region Rewarded Ads
    private void RequestRewardedAd()
    {
        rewardedAd = new RewardedAd(rewardedAdUnitId);
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Request a new ad
        AdRequest adRequest = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(adRequest);
    }

    public void ShowRewardedAd()
    {
        if (rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
        }
        else
        {
            Debug.LogWarning("Rewarded ad is not ready yet!");
            RequestRewardedAd(); // Request a new ad
        }
    }

    private void HandleUserEarnedReward(object sender, Reward args)
    {
        Debug.Log($"Reward earned: {args.Amount} {args.Type}");
        GrantReward(); // Grant the reward to the user
    }

    private void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        Debug.Log("Rewarded ad closed.");
        RequestRewardedAd(); // Reload ad after closing
    }

    private void GrantReward()
    {
        // Logic to grant reward to the player (e.g., coins, lives, etc.)
        Debug.Log("Player rewarded successfully!");
    }
    #endregion

    #region Ad Status Handling
    private void OnDestroy()
    {
        // Clean up events
        if (interstitialAd != null)
        {
            interstitialAd.OnAdClosed -= HandleInterstitialClosed;
            interstitialAd.Destroy();
        }

        if (rewardedAd != null)
        {
            rewardedAd.OnUserEarnedReward -= HandleUserEarnedReward;
            rewardedAd.OnAdClosed -= HandleRewardedAdClosed;
        }
    }
    #endregion
}
*/