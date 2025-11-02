using System.Collections;
using UnityEngine;

public class PlayerTrail : MonoBehaviour
{
    TrailRenderer trail;
    float startTime;
    //float t;
    // Start is called before the first frame update
    void Start()
    {
        trail = transform.GetComponent<TrailRenderer>();
        startTime = trail.time;
        trail.time = 0;
    }

    // Update is called once per frame
    /*void Update()
    {
        //transform.position = PlayerController.playerPosition;
        if (PlayerController.zIncreasingSpeed)
        {
            trail.enabled = true;
            if(t < 1)
                t += Time.deltaTime * 2;
            trail.time = Mathf.Lerp(0,startTime,t);
        }
        else
        {
            if (t > 0)
                t -= Time.deltaTime * 2;
            trail.time = Mathf.Lerp(0, startTime, t);
            if (trail.time == 0) trail.enabled = false;
        }           
    }*/
    public void EnableAccelerationTrail()
    {
        StartCoroutine(ManageTrail(0));
    }
    public void DisableAccelerationTrail()
    {
        StartCoroutine(ManageTrail(1));
    }
    private IEnumerator ManageTrail(float t)
    {
        trail.enabled = true;
        if (t == 1)
        {
            while (t > 0)
            {
                t -= Time.deltaTime * 2;
                trail.time = Mathf.Lerp(0, startTime, t);
                yield return null;
            }
        }
        else
        {
            while (t < 1)
            {
                t += Time.deltaTime * 2;
                trail.time = Mathf.Lerp(0, startTime, t);
                yield return null;
            }
        }          
    }
}
