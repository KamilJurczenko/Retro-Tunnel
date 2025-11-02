using UnityEngine;

public class RoomScaler : MonoBehaviour
{
    bool scaleUp = true;
    bool startScaling = false;

    [SerializeField] Transform[] wallsTransform;

    [SerializeField] float moveSpeed;

    private float xMin;
    private float xCurrent;

    void Start()
    {
        xMin = wallsTransform[0].position.x;
        
    }
    void Update()
    {
        if (startScaling)
        {
            float scaleSign;
            if (scaleUp) scaleSign = 1;
            else scaleSign = -1;

            foreach (Transform t in wallsTransform)
            {
                float wallSign = Mathf.Sign(t.position.x);
                float xMax = (RoomConstants.floorPosition.x + RoomConstants.singleFloorWidth / 2);
                xCurrent += Time.deltaTime * moveSpeed;
                t.position = new Vector3(scaleSign * wallSign * Mathf.Clamp(xCurrent, xMin, xMax), t.position.y, t.position.z);
            }
        }
    }
    //Scales Room down or up
    public void ScaleRoom(bool scaleBool)
    {
        startScaling = true;
        scaleUp = scaleBool;
    }
}
