using UnityEngine;

public class TunnelConstants : MonoBehaviour
{
    public static float tunnelLength;
    // Start is called before the first frame update
    void Start()
    {
        tunnelLength = GetComponent<Renderer>().bounds.size.z;
    }
}
