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
    bool testMode = true;
    // Start is called before the first frame update
    void Start()
    {
        Advertisement.Initialize(gameID, testMode);
        ShowBanner();
    }

    public void PlayAd()
    {
        if (Advertisement.IsReady("Custom_Interstitial"))
        {
            Advertisement.Show("Custom_Interstitial");
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
