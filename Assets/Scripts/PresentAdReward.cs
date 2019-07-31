using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PresentAdReward : MonoBehaviour
{
    public Text cooldownText;

    public float coolDown = 30f;

    public float rewardAmount = 20f;

    public string zoneId = "rewardedVideo";

    private Button button;

    private float remainingCooldown;

    // Use this for initialization
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OpenReward);

    }

    private bool IsReady()
    {
        if (Time.time > remainingCooldown)
        {
            return AdManager.instance.IsAdWithZoneIdReady(zoneId);
        }

        return false;
    }

    private void OpenReward()
    {
        AdManager.instance.ShowVideoAd(PresentRewardCallback, zoneId);
    }

    private void PresentRewardCallback(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("Player finished watching the video ad and is being rewarded with extra fuel.");
                GameManager.instance.extraFuel += rewardAmount;

                if (coolDown > 0f)
                {
                    remainingCooldown = Time.time + coolDown;
                }
                break;
            case ShowResult.Skipped:
                Debug.Log("Player skipped watching the video ad, no reward.");
                break;
            case ShowResult.Failed:
                Debug.Log("video ad failed, no reward.");
                break;
        }

    }

    void Update()
    {
        if (button)
        {

            button.interactable = IsReady();

            if (button.interactable)
            {
                cooldownText.text = "Reward ready! Touch to watch video and open";
            }
            else
            {
                cooldownText.text = "Next reward in: " + (int)(remainingCooldown - Time.time) + " seconds";
            }
        }
    }
}