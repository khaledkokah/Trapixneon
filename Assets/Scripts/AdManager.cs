using System;
using System.Security.Policy;
using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class AdManager : MonoBehaviour
{
    public static AdManager instance;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    /// <summary>
    /// Used to 'pause' the game when running in the Unity editor (Unity Ads will pause the game on actual Android or iOS devices by default, this is just for the editor)
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForAdEditor()
    {
        float currentTimescale = Time.timeScale;
        Time.timeScale = 0f;
        AudioListener.pause = true;

        yield return null;

        while (Advertisement.isShowing)
        {
            yield return null;
        }

        AudioListener.pause = false;
        if (currentTimescale > 0f)
        {
            Time.timeScale = currentTimescale;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    private void Start()
    {
        Debug.Log("Unity Ads initialized: " + Advertisement.isInitialized);
        Debug.Log("Unity Ads is supported: " + Advertisement.isSupported);
        Debug.Log("Unity Ads test mode enabled: " + Advertisement.testMode);
    }

    public void ShowStandardVideoAd()
    {
        ShowVideoAd();
    }

    public void ShowVideoAd(Action<ShowResult> adCallBackAction = null, string zone = "")
    {

        StartCoroutine(WaitForAdEditor());

        if (string.IsNullOrEmpty(zone))
        {
            zone = null;
        }

        var options = new ShowOptions();

        if (adCallBackAction == null)
        {
            options.resultCallback = DefaultAdCallBackHandler;
        }
        else
        {
            options.resultCallback = adCallBackAction;
        }

        if (Advertisement.IsReady(zone))
        {
            Debug.Log("Showing ad for zone: " + zone);
            Advertisement.Show(zone, options);
        }
        else
        {
            Debug.LogWarning("Ad was not ready. Zone: " + zone);
        }
    }

    private void DefaultAdCallBackHandler(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Time.timeScale = 1f;
                break;

            case ShowResult.Failed:
                Time.timeScale = 1f;
                break;

            case ShowResult.Skipped:
                Time.timeScale = 1f;
                break;
        }
    }

    public void ShowRewardedFuelVideoAd()
    {
        ShowVideoAd(FuelRewardCallBack, "rewardedVideo");
    }

    private void FuelRewardCallBack(ShowResult showResult)
    {

        var fuelBtn = GameObject.Find("BtnVideoRewardAd").GetComponent<Button>();

        switch (showResult)
        {
            case ShowResult.Finished:
                Debug.Log("Player finished watching the video ad and is being rewarded with extra fuel.");
                GameManager.instance.extraFuel = 10f;
                fuelBtn.transform.GetChild(0).GetComponent<Text>().text = "Fuel added (10 extra)";
                fuelBtn.enabled = false;
                break;

            case ShowResult.Skipped:
                Debug.Log("Player skipped watching the video ad, no reward.");
                fuelBtn.GetComponent<Button>();
                fuelBtn.transform.GetChild(0).GetComponent<Text>().text = "No fuel added - you skipped the ad";
                fuelBtn.enabled = false;
                break;

            case ShowResult.Failed:
                Debug.Log("video ad failed, no reward.");
                break;
        }
    }

    public bool IsAdWithZoneIdReady(string zoneId)
    {
        return Advertisement.IsReady(zoneId);
    }

    //  // Update is called once per frame
    //  void Update()
    //  {
    //      if(Input.GetKeyDown(KeyCode.E))
    //{
    //	if(Advertisement.IsReady("video")){
    //		Advertisement.Show("video");
    //	}
    //}
    //  }
}
