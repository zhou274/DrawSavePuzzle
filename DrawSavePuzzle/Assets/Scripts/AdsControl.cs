using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SocialPlatforms;
using GoogleMobileAds.Api;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using GoogleMobileAds.Common;
/*
 * 
 * Document for Unity Ads : https://docs.unity.com/ads/ImplementingBasicAdsUnity.html
 */
/*
 * 
 * Document for Google Admob : https://developers.google.com/admob/unity/quick-start
 */

public class AdsControl : MonoBehaviour
{
    
    private static AdsControl instance;

    //for Admob

    public string Android_AppID, IOS_AppID;

    public string Android_Banner_Key, IOS_Banner_Key;

    public string Android_Interestital_Key, IOS_Interestital_Key;

    public string Android_RW_Key, IOS_RW_Key;

    private AppOpenAd appOpenAd;
    private BannerView bannerView;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;
    private RewardedInterstitialAd rewardedInterstitialAd;
    private bool isShowingAppOpenAd;
    
    public UnityEvent OnAdLoadedEvent;
    public UnityEvent OnAdFailedToLoadEvent;
    public UnityEvent OnAdOpeningEvent;
    public UnityEvent OnAdFailedToShowEvent;
    public UnityEvent OnUserEarnedRewardEvent;
    public UnityEvent OnAdClosedEvent;
    

    public enum REWARD_TYPE
    {
       SKIP_LEVEL
       
    };

    public REWARD_TYPE currentType;

    public int adCurrent;

  

    public static AdsControl Instance { get { return instance; } }

    void Awake()
    {
        if (FindObjectsOfType(typeof(AdsControl)).Length > 1)
        {
            Destroy(gameObject);
            return;
        }
       

        instance = this;
        DontDestroyOnLoad(gameObject); 
    }

   




    private void Start()
    {
        //MobileAds.SetiOSAppPauseOnBackground(true);
        //// Initialize the Google Mobile Ads SDK.
        //MobileAds.Initialize(HandleInitCompleteAction);
       
       
    }

    private void HandleInitCompleteAction(InitializationStatus initstatus)
    {
        Debug.Log("Initialization complete.");

        // Callbacks from GoogleMobileAds are not guaranteed to be called on
        // the main thread.
        // In this example we use MobileAdsEventExecutor to schedule these calls on
        // the next Update() loop.
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
           
            RequestBannerAd();
            RequestAndLoadInterstitialAd();
            RequestAndLoadRewardedAd();
        });
    }

    #region HELPER METHODS

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()
            .AddKeyword("unity-admob-sample")
            .Build();
    }

    public void OnApplicationPause(bool paused)
    {
        // Display the app open ad when the app is foregrounded.
        if (!paused)
        {
           // ShowAppOpenAd();
        }
    }

    #endregion

    #region BANNER ADS

    public void RequestBannerAd()
    {
        //PrintStatus("Requesting Banner ad.");

        // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
        string adUnitId = "unused";

#elif UNITY_ANDROID
        string adUnitId =  Android_Banner_Key;
#elif UNITY_IPHONE
        string adUnitId = IOS_Banner_Key;
#else
        string adUnitId = "unexpected_platform";
#endif

        // Clean up banner before reusing
        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        // Create a 320x50 banner at top of the screen
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        // Add Event Handlers
        bannerView.OnAdLoaded += (sender, args) =>
        {
            //PrintStatus("Banner ad loaded.");
            OnAdLoadedEvent.Invoke();
        };
        bannerView.OnAdFailedToLoad += (sender, args) =>
        {
           // PrintStatus("Banner ad failed to load with error: " + args.LoadAdError.GetMessage());
            OnAdFailedToLoadEvent.Invoke();
        };
        bannerView.OnAdOpening += (sender, args) =>
        {
            //PrintStatus("Banner ad opening.");
            OnAdOpeningEvent.Invoke();
        };
        bannerView.OnAdClosed += (sender, args) =>
        {
            //PrintStatus("Banner ad closed.");
            OnAdClosedEvent.Invoke();
        };
        bannerView.OnPaidEvent += (sender, args) =>
        {
            string msg = string.Format("{0} (currency: {1}, value: {2}",
                                        "Banner ad received a paid event.",
                                        args.AdValue.CurrencyCode,
                                        args.AdValue.Value);
            //PrintStatus(msg);
        };

        // Load a banner ad
        bannerView.LoadAd(CreateAdRequest());
    }

    public void DestroyBannerAd()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }
    }

    public void ShowBannerAd()
    {
        //if (bannerView != null)
        //{
        //    bannerView.Show();
        //}
    }

    public void HideBannerAd()
    {
        //if (bannerView != null)
        //{
        //    bannerView.Hide();
        //}
    }

    #endregion

    #region INTERSTITIAL ADS

    public void RequestAndLoadInterstitialAd()
    {
       // PrintStatus("Requesting Interstitial ad.");

#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = Android_Interestital_Key;
#elif UNITY_IPHONE
        string adUnitId = IOS_Interestital_Key;
#else
        string adUnitId = "unexpected_platform";
#endif

        // Clean up interstitial before using it
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }

        interstitialAd = new InterstitialAd(adUnitId);

        // Add Event Handlers
        interstitialAd.OnAdLoaded += (sender, args) =>
        {
            //PrintStatus("Interstitial ad loaded.");
            OnAdLoadedEvent.Invoke();
        };
        interstitialAd.OnAdFailedToLoad += (sender, args) =>
        {
            //PrintStatus("Interstitial ad failed to load with error: " + args.LoadAdError.GetMessage());
            OnAdFailedToLoadEvent.Invoke();
        };
        interstitialAd.OnAdOpening += (sender, args) =>
        {
            //PrintStatus("Interstitial ad opening.");
            OnAdOpeningEvent.Invoke();
        };
        interstitialAd.OnAdClosed += (sender, args) =>
        {
            // PrintStatus("Interstitial ad closed.");
            RequestAndLoadInterstitialAd();
            OnAdClosedEvent.Invoke();
        };
        interstitialAd.OnAdDidRecordImpression += (sender, args) =>
        {
            //PrintStatus("Interstitial ad recorded an impression.");
        };
        interstitialAd.OnAdFailedToShow += (sender, args) =>
        {
            //PrintStatus("Interstitial ad failed to show.");
        };
        interstitialAd.OnPaidEvent += (sender, args) =>
        {
            string msg = string.Format("{0} (currency: {1}, value: {2}",
                                        "Interstitial ad received a paid event.",
                                        args.AdValue.CurrencyCode,
                                        args.AdValue.Value);
            //PrintStatus(msg);
        };

        // Load an interstitial ad
        interstitialAd.LoadAd(CreateAdRequest());
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.IsLoaded())
        {
            interstitialAd.Show();
        }
        else
        {
            //PrintStatus("Interstitial ad is not ready yet.");
        }
    }

    public void ShowInterstitalRandom()
    {
        StartCoroutine(ShowInterstitalRandomIE());
    }

    IEnumerator ShowInterstitalRandomIE()
    {
        yield return new WaitForSeconds(0.5f);

        if (adCurrent >= 2)
        {
            ShowInterstitialAd();
            adCurrent = 0;
        }
        else
            adCurrent++;
    }

    public void DestroyInterstitialAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }
    }

    #endregion

    #region REWARDED ADS

    public void RequestAndLoadRewardedAd()
    {
       // PrintStatus("Requesting Rewarded ad.");
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = Android_RW_Key;
#elif UNITY_IPHONE
        string adUnitId = IOS_RW_Key;
#else
        string adUnitId = "unexpected_platform";
#endif

        // create new rewarded ad instance
        rewardedAd = new RewardedAd(adUnitId);

        // Add Event Handlers
        rewardedAd.OnAdLoaded += (sender, args) =>
        {
           // PrintStatus("Reward ad loaded.");
            OnAdLoadedEvent.Invoke();
        };
        rewardedAd.OnAdFailedToLoad += (sender, args) =>
        {
           // PrintStatus("Reward ad failed to load.");
            OnAdFailedToLoadEvent.Invoke();
        };
        rewardedAd.OnAdOpening += (sender, args) =>
        {
           // PrintStatus("Reward ad opening.");
            OnAdOpeningEvent.Invoke();
        };
        rewardedAd.OnAdFailedToShow += (sender, args) =>
        {
           // PrintStatus("Reward ad failed to show with error: " + args.AdError.GetMessage());
            OnAdFailedToShowEvent.Invoke();
        };
        rewardedAd.OnAdClosed += (sender, args) =>
        {
            // PrintStatus("Reward ad closed.");
            RequestAndLoadRewardedAd();
            OnAdClosedEvent.Invoke();
        };
        rewardedAd.OnUserEarnedReward += (sender, args) =>
        {

            // PrintStatus("User earned Reward ad reward: " + args.Amount);
            if (currentType == REWARD_TYPE.SKIP_LEVEL)
            {
                GameManager.instance.SkipLevel();
            }
           

            OnUserEarnedRewardEvent.Invoke();
        };
        rewardedAd.OnAdDidRecordImpression += (sender, args) =>
        {
            //PrintStatus("Reward ad recorded an impression.");
        };
        rewardedAd.OnPaidEvent += (sender, args) =>
        {
            string msg = string.Format("{0} (currency: {1}, value: {2}",
                                        "Rewarded ad received a paid event.",
                                        args.AdValue.CurrencyCode,
                                        args.AdValue.Value);
            //PrintStatus(msg);
        };

        // Create empty ad request
        rewardedAd.LoadAd(CreateAdRequest());
    }

    public void ShowRewardedAd(REWARD_TYPE _type)
    {
        if (rewardedAd != null)
        {
            currentType = _type;
            rewardedAd.Show();
        }
        else
        {
            //PrintStatus("Rewarded ad is not ready yet.");
        }
    }

    public bool IsRWAvailable()
    {
        bool available = false;

        if (rewardedAd.IsLoaded())
            available = true;
        return available;
    }

    public void RequestAndLoadRewardedInterstitialAd()
    {
        //PrintStatus("Requesting Rewarded Interstitial ad.");

        // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
            string adUnitId = "ca-app-pub-3940256099942544/5354046379";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/6978759866";
#else
            string adUnitId = "unexpected_platform";
#endif

        // Create an interstitial.
        RewardedInterstitialAd.LoadAd(adUnitId, CreateAdRequest(), (rewardedInterstitialAd, error) =>
        {
            if (error != null)
            {
                //PrintStatus("Rewarded Interstitial ad load failed with error: " + error);
                return;
            }

            this.rewardedInterstitialAd = rewardedInterstitialAd;
            //PrintStatus("Rewarded Interstitial ad loaded.");

            // Register for ad events.
            this.rewardedInterstitialAd.OnAdDidPresentFullScreenContent += (sender, args) =>
            {
               // PrintStatus("Rewarded Interstitial ad presented.");
            };
            this.rewardedInterstitialAd.OnAdDidDismissFullScreenContent += (sender, args) =>
            {
               // PrintStatus("Rewarded Interstitial ad dismissed.");
                this.rewardedInterstitialAd = null;
            };
            this.rewardedInterstitialAd.OnAdFailedToPresentFullScreenContent += (sender, args) =>
            {
               // PrintStatus("Rewarded Interstitial ad failed to present with error: " + args.AdError.GetMessage());
                this.rewardedInterstitialAd = null;
            };
            this.rewardedInterstitialAd.OnPaidEvent += (sender, args) =>
            {
                string msg = string.Format("{0} (currency: {1}, value: {2}",
                                            "Rewarded Interstitial ad received a paid event.",
                                            args.AdValue.CurrencyCode,
                                            args.AdValue.Value);
                //PrintStatus(msg);
            };
            this.rewardedInterstitialAd.OnAdDidRecordImpression += (sender, args) =>
            {
                //PrintStatus("Rewarded Interstitial ad recorded an impression.");
            };
        });
    }

    public void ShowRewardedInterstitialAd()
    {
        if (rewardedInterstitialAd != null)
        {
            rewardedInterstitialAd.Show((reward) =>
            {
                //PrintStatus("Rewarded Interstitial ad Rewarded : " + reward.Amount);
            });
        }
        else
        {
            //PrintStatus("Rewarded Interstitial ad is not ready yet.");
        }
    }

    #endregion

    public void ShowInterstitalMediation()
    {
        int numberShow = PlayerPrefs.GetInt("ShowAds");

        if(numberShow < 1)
        {
            numberShow++;
            PlayerPrefs.SetInt("ShowAds", numberShow);
            return;
        }
        else
        {
            numberShow = 0;
            PlayerPrefs.SetInt("ShowAds", numberShow);
        }

        if (interstitialAd != null && interstitialAd.IsLoaded())
        {
            interstitialAd.Show();
        }
        
    }

    public void ShowRWMediation(REWARD_TYPE _type)
    {
        
        if (rewardedAd != null && rewardedAd.IsLoaded())
        {
            ShowRewardedAd(_type);
        }
        

    }
}

