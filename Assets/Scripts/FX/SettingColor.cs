using UnityEngine;
using UnityEngine.UI;

public class SettingColor : MonoBehaviour
{
    [SerializeField] public Color[] clrs;
    [SerializeField] Renderer backgroundRenderer;
    [SerializeField] Renderer tunnelRenderer;
    [SerializeField] Renderer tunnelPassRenderer;
    [SerializeField] ParticleSystemRenderer tunnelStageTransRenderer;
    [SerializeField] Image backgroundShineMat;
    [SerializeField] GameObject[] backgroundObjects;
    private int counter = 0;
    private void Start()
    {
        SetColor();
    }
    public void SetColor()
    {
        if (counter > 0)
            backgroundObjects[counter - 1].SetActive(false);
        if (counter > clrs.Length - 1)
        {
            //backgroundObjects[counter - 1].SetActive(false);
            counter = 0;
        }
        backgroundObjects[counter].SetActive(true);
        Color clr = clrs[counter];
        tunnelRenderer.material.SetColor("Grid_Color", clr);
        tunnelPassRenderer.material.SetColor("_SecondaryColor", NextColor());
        tunnelStageTransRenderer.material.SetColor("Grid_Color", clr);
        tunnelStageTransRenderer.material.SetColor("Color_749f26e18955442b9716f80567bb9708", clr * 0.5f);
        backgroundRenderer.material.SetColor("Grid_Color", clr);
        backgroundShineMat.material.SetColor("_GradientColor", clr);
        counter++;
    }
    private Color NextColor()
    {
        if (counter + 1 < clrs.Length)
        {
            return clrs[counter + 1];
        }
        else return clrs[0];
    }
}
