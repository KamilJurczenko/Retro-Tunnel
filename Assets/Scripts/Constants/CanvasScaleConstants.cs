using UnityEngine;

public class CanvasScaleConstants : MonoBehaviour
{
    public static Vector3 uiCanvasScale;

    // Start is called before the first frame update
    void Awake()
    {
        uiCanvasScale = GetComponent<RectTransform>().localScale;
    }
}
