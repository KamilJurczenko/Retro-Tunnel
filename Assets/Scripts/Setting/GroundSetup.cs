using UnityEngine;

public class GroundSetup : MonoBehaviour
{
    [SerializeField] Transform[] lavaGround;
    [SerializeField] Transform[] defaultGround;
    
    // Possible TODO smooth Transition from one ground to another
    public void SetGround(int groundIndex = 0)
    {
        if(groundIndex == 0)
        {
            SetActive(true, defaultGround);
            SetActive(false, lavaGround);
        }
        else
        {
            SetActive(false, defaultGround);
            SetActive(true, lavaGround);
        }
    }
    private void SetActive(bool b, Transform[] t)
    {
        foreach (Transform tt in t)
            tt.gameObject.SetActive(b);
    }
}
