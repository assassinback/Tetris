using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleAdsScript : MonoBehaviour
{
    // Start is called before the first frame update
    public BannerView _bannerView;
    public InterstitialAd interstitial;
    public static GoogleAdsScript _instance;
    //string adUnitId = "";
    private void Awake()
    {
        _instance = this;

    }
    void Start()
    {
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize(initStatus => { });

        RequestInterstitial();
    }

    public void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-5667375169098416/1718625404";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-5667375169098416/6539459825";
#else
        string adUnitId = "unexpected_platform";
#endif
        var adRequest = new AdRequest.Builder()
              .AddKeyword("unity-admob-sample")
              .Build();
        // Initialize an InterstitialAd.
        InterstitialAd.Load(adUnitId, adRequest,
          (InterstitialAd ad, LoadAdError error) =>
          {
              // if error is not null, the load request failed.
              if (error != null || ad == null)
              {
                  Debug.LogError("interstitial ad failed to load an ad " +
                                 "with error : " + error);
                  return;
              }

              Debug.Log("Interstitial ad loaded with response : "
                        + ad.GetResponseInfo());

              interstitial = ad;
          });

        interstitial.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        interstitial.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        interstitial.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        interstitial.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        interstitial.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        interstitial.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
        
    }
    public void HandleOnAdLoadedNormalAd(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }
    public void HandleOnAdFailedToLoadNormalAd(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.LoadAdError.GetCause());
    }
    public void HandleOnAdOpeningNormalAd(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpening event received");
        GameObject.Find("768x1024(Clone)").GetComponent<Canvas>().sortingOrder = 101;
    }
    public void HandleOnAdClosedNormalAd(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
        interstitial.Destroy();
    }
    public void RequestBanner()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-5667375169098416/4104868173";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-5667375169098416/3870146020";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Create a 320x50 banner at the top of the screen.
        this._bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Raised when an ad is loaded into the banner view.
        _bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner view loaded an ad with response : "
                + _bannerView.GetResponseInfo());
        };
        // Raised when an ad fails to load into the banner view.
        _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad with error : "
                + error);
        };
        // Raised when the ad is estimated to have earned money.
        _bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Banner view paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        _bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        _bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };
        // Raised when an ad opened full screen content.
        _bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        _bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };
        // Load the banner with the request.
        this._bannerView.LoadAd(request);


    }
     
    public void HandleOnAdLoadedBanner(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
        GameObject.Find("BANNER(Clone)").GetComponent<Canvas>().sortingOrder = 100;
        GameObject.Find("BANNER(Clone)").GetComponent<Canvas>().transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(GameObject.Find("BANNER(Clone)").GetComponent<Canvas>().transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition.x, 0);
    }
    public void HandleOnAdFailedToLoadBanner(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.LoadAdError.GetMessage());
    }
    public void HandleOnAdOpenedBanner(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }
    public void HandleOnAdClosedBanner(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
    }
}