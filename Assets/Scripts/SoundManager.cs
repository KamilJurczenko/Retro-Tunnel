using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;

    public IEnumerator ChangeVolumeCoroutine()
    {
        float tmp = musicSource.volume;
        float t = 0;
        while (t < tmp)
        {
            t += 0.001f;
            musicSource.volume = t;
            yield return null;
        }
    }
}
