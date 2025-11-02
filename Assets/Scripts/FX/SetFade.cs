using UnityEngine;

public class SetFade : MonoBehaviour
{
    [SerializeField] Material[] fadeInMats;
    [SerializeField] Material[] fadeOutMats;

    [SerializeField] float fadeInDistance;
    [SerializeField] float fadeInLength;
    [SerializeField] float fadeOutDistance;
    [SerializeField] float fadeOutLength;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Material m in fadeInMats)
        {
            m.SetFloat(Shader.PropertyToID("_fadeInDistance"), fadeInDistance);
            m.SetFloat(Shader.PropertyToID("_fadeInLength"), fadeInLength);
        }
        foreach(Material m in fadeOutMats)
        {
            m.SetFloat(Shader.PropertyToID("_fadeOutDistance"), fadeOutLength);
            m.SetFloat(Shader.PropertyToID("_fadeOutLength"), fadeOutDistance);
        }
    }
}
