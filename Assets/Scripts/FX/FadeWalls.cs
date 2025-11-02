using UnityEngine;

public class FadeWalls : MonoBehaviour
{
    [SerializeField] Material[] mats;

    [SerializeField] private float fadeDistance;
    [SerializeField] private float fadeLength;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Material m in mats)
        {
            m.SetFloat(Shader.PropertyToID("_FadeDistance"), fadeDistance);
            m.SetFloat(Shader.PropertyToID("_FadeLength"), fadeLength);
        }
    }
}
