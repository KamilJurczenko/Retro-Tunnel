using UnityEngine;

public class RestartPanel : MonoBehaviour
{
    [SerializeField] GameObject restartPanelObject;
    [SerializeField] GameObject restartButtonAd;
    [SerializeField] RectTransform restartButtonNoAd;

    public void SetActivePanel(bool active)
    {
        restartPanelObject.SetActive(active);
        if(PlayerController.deathCount > 1)
        {
            restartButtonAd.SetActive(false);
            restartButtonNoAd.anchoredPosition = new Vector3(0, restartButtonNoAd.anchoredPosition.y);
        }
    }

}
