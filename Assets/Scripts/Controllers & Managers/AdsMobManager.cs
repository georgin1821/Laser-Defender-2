using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdsMobManager : Singleton<AdsMobManager>
{
    private string appID = "ca-app-pub-2066779871986916~3498101778";

#if UNITY_ANDROID
    string rewardID = "ca-app-pub-2066779871986916/2725782443";
    string testRewardID = "ca-app-pub-3940256099942544/5224354917";
    string realRewardedID = "ca-app-pub-2066779871986916/2725782443";
#endif
    private bool isRewardedAdReady;

    public bool IsRewardedAdReady
    {
        get { return rewardedAd != null && rewardedAd.CanShowAd(); }
    }

    public RewardedAd rewardedAd;
    override protected void Awake()
    {
        base.Awake();
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize(initstatus =>
        {
            print("Ads are ready!");
        });
        // its a active for an hour
        LoadRewardedAd();
    }

    public void LoadRewardedAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }
        var adRequest = new AdRequest();
        // adRequest.Keywords.Add("unity-admob-sample");

        RewardedAd.Load(rewardID, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                print("Rewarded ad failed to load with error: " + error);
                return;
            }
            print("Rewarded ad loaded!!");
            rewardedAd = ad;
            RewardedAdEvents(rewardedAd);
        });
    }
    public void ShowRewardedAd()
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                print("Give rewards to Player");
            });
        }

        else
        {
            print("Rewarded Ad not ready");
        }
    }
    public void RewardedAdEvents(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
            LoadRewardedAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
            LoadRewardedAd();
        };
    }
}
