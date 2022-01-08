using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
public class AdsManager : MonoBehaviour
{
#if UNITY_ANDROID
    string gameID = "4549061";
#elif UNITY_IOS
    string gameID = "4549060";
#endif
    [SerializeField]
    int numberOfTimesAdHasRun = 0;
    [SerializeField]
    bool testMode = true;
    // Start is called before the first frame update
    void Start()
    {
        Advertisement.Initialize(gameID, testMode);
        ShowBanner();
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
            if (Random.Range(1, 100 + 1) <= 40)
            {
                numberOfTimesAdHasRun++;
                Debug.Log("Ad has run for less than 3 times. current run is: " + numberOfTimesAdHasRun);
                Debug.Log("Ad is running");
                
                if (Advertisement.IsReady("Custom_Interstitial"))
                {
                    Advertisement.Show("Custom_Interstitial");
                }
            }
            else
            {
                Debug.Log("Ad is not running");
            }
        }
    }

    public void ShowBanner()
    {
        if (Advertisement.IsReady("Custom_Banner"))
        {
            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
            Advertisement.Banner.Show("Custom_Banner");
        }
        else
        {
            StartCoroutine(RepeatShowBanner());
        }
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
}
