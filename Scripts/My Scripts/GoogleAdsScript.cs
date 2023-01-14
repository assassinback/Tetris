using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleAdsScript : MonoBehaviour
{
    // Start is called before the first frame update
    public BannerView bannerView;
    public InterstitialAd interstitial;
    public static GoogleAdsScript _instance;
    //string adUnitId = "";
    private void Awake()
    {
        _instance = this;

    }
    void Start()
    {
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

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);

        // Called when an ad request has successfully loaded.
        this.interstitial.OnAdLoaded += HandleOnAdLoadedNormalAd;
        // Called when an ad request failed to load.
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoadNormalAd;
        // Called when an ad is shown.
        this.interstitial.OnAdOpening += HandleOnAdOpeningNormalAd;
        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleOnAdClosedNormalAd;
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
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
        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);
        // Called when an ad request has successfully loaded.
        this.bannerView.OnAdLoaded += this.HandleOnAdLoadedBanner;
        // Called when an ad request failed to load.
        this.bannerView.OnAdFailedToLoad += this.HandleOnAdFailedToLoadBanner;
        // Called when an ad is clicked.
        this.bannerView.OnAdOpening += this.HandleOnAdOpenedBanner;
        // Called when the user returned from the app after an ad click.
        this.bannerView.OnAdClosed += this.HandleOnAdClosedBanner;
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);


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
