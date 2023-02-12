using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener
{

#if UNITY_ANDROID
    string gameID = "4607713";
#elif UNITY_IOS
     gameID = "4607712";
#endif
    [SerializeField] private BannerPosition bannerPosition = BannerPosition.BOTTOM_CENTER;
    [SerializeField]
    int numberOfTimesAdHasRun = 0;
    [SerializeField]
    bool testMode = false;
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        testMode = true;


#else
        if (Debug.isDebugBuild)
        {
            testMode = true;
        }
        else
        {
            testMode = false;
        }
#endif

        Advertisement.Initialize(gameID, testMode, this);
        ShowBanner();
        InitialiseBanner("Banner_Android");
    }

    void InitialiseBanner(string bannerAdName)
    {
        //string bannerAdUnitId = bannerAdName;
        //BannerAdSize bannerSize = new BannerAdSize(BannerAdPredefinedSize.Banner);
        //BannerAdAnchor bannerAnchor = BannerAdAnchor.TopCenter;
        //Vector2 bannerOffset = Vector2.zero;
        //IBannerAd bannerAd = MediationService.Instance.CreateBannerAd(bannerAdUnitId, bannerSize, bannerAnchor, bannerOffset);

        Advertisement.Banner.SetPosition(bannerPosition);
        Advertisement.Banner.Show(bannerAdName);


        //BannerLoadOptions options = new BannerLoadOptions
        //{
        //    loadCallback = OnBannerLoaded,
        //    errorCallback = OnBannerError
        //};
        //
        //
        //Advertisement.Banner.Load(bannerAdName, options);
    }

    void OnBannerLoaded()
    {
        BannerOptions options = new BannerOptions
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };
        Advertisement.Banner.SetPosition(bannerPosition);
        // Show the loaded Banner Ad Unit:
        Advertisement.Banner.Show("Banner_Android", options);
    }
    void OnBannerClicked() { }
    void OnBannerShown() { }
    void OnBannerHidden() { }

    void OnBannerError(string message)
    {
        Debug.Log($"Banner Error: {message}");
    }

        public void PlayAd()
    {
        if (numberOfTimesAdHasRun >= 2)
        {
            Debug.Log("Ad has run for more than 2 times. current run is: " + numberOfTimesAdHasRun );
            numberOfTimesAdHasRun = 0;
        }
        else
        {
            
            //40% chance of playing the ad
            if (Random.Range(1, 100 + 1) <= 20)
            {
                numberOfTimesAdHasRun++;
                Debug.Log("Ad has run for less than 3 times. current run is: " + numberOfTimesAdHasRun);
                Debug.Log("Ad is running");
                
               // if (Advertisement.IsReady("Interstitial_Android"))
               // {
               //     Advertisement.Show("Interstitial_Android");
               // }
            }
            else
            {
                Debug.Log("Ad is not running");
            }
        }
    }

    public void ShowBanner()
    {
        //if (Advertisement.IsReady("Banner_Android"))
        //{
        //    Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        //    Advertisement.Banner.Show("Banner_Android");
        //}
        //else
        //{
        //    StartCoroutine(RepeatShowBanner());
        //}
    }
    public void HideBanner()
    {
        Advertisement.Banner.Hide();
    }
    IEnumerator RepeatShowBanner()
    {
        yield return new WaitForSeconds(1f);
        ShowBanner();
    }

    public void OnInitializationComplete()
    {
        //throw new NotImplementedException();
    }

    public void OnInitializationFailed(UnityEngine.Advertisements.UnityAdsInitializationError error, string message)
    {
        //throw new NotImplementedException();
    }
}
