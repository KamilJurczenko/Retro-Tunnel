using UnityEngine;
using UnityEngine.Advertisements;

public class AdvertisementManager : MonoBehaviour, IUnityAdsListener
{
    [SerializeField] PlayerController playerController;
    private string gameId = "4584575";
    //private string gameId = "4584574"; iosKey

    void Start()
    {
        Advertisement.Initialize(gameId);
        Advertisement.AddListener(this);

        if (!PlayerPrefs.HasKey("restartCount"))
        {
            PlayerPrefs.SetInt("restartCount", 0);
        }
        else
        {
            int val = PlayerPrefs.GetInt("restartCount");
            if(val == 3)
            {
                ShowInterstitial();
                val = -1;
            }
            val++;
            PlayerPrefs.SetInt("restartCount", val);
        }
    }

    // Implement a function for showing a rewarded video ad:
    public void ShowRewardedVideo()
    {
        if (Advertisement.IsReady("Rewarded_Android"))
        {
            Advertisement.Show("Rewarded_Android");
        }
    }
    public void ShowInterstitial()
    {
        if (Advertisement.IsReady("Interstitial_Android"))
        {
            Advertisement.Show("Interstitial_Android");
        }
    }
    public void OnDestroy()
    {
        Advertisement.RemoveListener(this);
    }
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished && placementId == "Rewarded_Android")
        {
            Debug.Log("Ads successfully finished");
            playerController.Respawn();
        }
        else if (showResult == ShowResult.Skipped)
        {
            Debug.Log("Skipped Ad");
            // Do not reward the user for skipping the ad.
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }

    public void OnUnityAdsReady(string placementId)
    {
        // Ads Are Ready
    }
}