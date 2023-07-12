using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SocialPlatforms;
using System;
using UnityEngine.Advertisements;
using UnityEngine.UI;
public class AdsControl : MonoBehaviour
{


    protected AdsControl()
    {
    }

    private static AdsControl _instance;
    public string AdmobID_Android, AdmobID_IOS, BannerID_Android, BannerID_IOS;
    public string UnityID_Android, UnityID_IOS, UnityZoneID;

    public static AdsControl Instance { get { return _instance; } }

    void Awake()
    {
        if (FindObjectsOfType(typeof(AdsControl)).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        MakeNewInterstial();
        RequestBanner();
        
        if (PlayerPrefs.GetInt("RemoveAds") == 0)
            ShowBanner();
        else
            HideBanner();

        DontDestroyOnLoad(gameObject); //Already done by CBManager

    }


    public void HandleInterstialAdClosed(object sender, EventArgs args)
    {
        MakeNewInterstial();

    }

    void MakeNewInterstial()
    {


    }


    public void showAds()
    {
        int adsCounter = PlayerPrefs.GetInt("AdsCounter");

        PlayerPrefs.SetInt("AdsCounter", adsCounter);
    }


    public bool GetRewardAvailable()
    {
        bool avaiable = false;
        return avaiable;
    }

    public void ShowRewardVideo()
    {

    }


    private void RequestBanner()
    {

    }

    public void ShowBanner()
    {

    }

    public void HideBanner()
    {

    }


    public void ShowFB()
    {
        Application.OpenURL("https://www.facebook.com/PonyStudio2507/?ref=settings");
    }

    public void RateMyGame()
    {
#if UNITY_EDITOR
        Application.OpenURL("https://itunes.apple.com/us/app/color-flow-puzzle/id1436566275?ls=1&mt=8");
#elif UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.ponygames.MagicBlockPuzzle");
#elif UNITY_IPHONE
        Application.OpenURL("https://itunes.apple.com/us/app/color-flow-puzzle/id1436566275?ls=1&mt=8");
#else
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.ponygames.MagicBlockPuzzle");
#endif


    }

    private void HandleShowResult()
    {

    }

    public void PlayCallbackRewardVideo()
    {
    }

    public void PlayDelegateRewardVideo(Action<bool> onVideoPlayed)
    {

    }
    public void TestDelegate()
    {
        AdsControl.Instance.PlayDelegateRewardVideo(delegate
        {
            //function
        });
    }

}

