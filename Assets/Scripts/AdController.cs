﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdController : MonoBehaviour, IUnityAdsListener
{
    private const string ANDROID_PLAY_STORE_ID = "3381670";
    private const string APPLE_APP_STORE_ID = "3381671";

    private const string VIDEO_AD = "video";
    private const string BANNER_AD = "banner";

    // Start is called before the first frame update
    private void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(ANDROID_PLAY_STORE_ID, true);
    }

    public void OnUnityAdsDidError(string message)
    {
        // ...
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // ...
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // ..
    }

    public void OnUnityAdsReady(string placementId)
    {
        if (placementId == BANNER_AD)
        {
            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
            Advertisement.Banner.Show(BANNER_AD);
        }
    }
}
