using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    float offset;
    float z;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position.z;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if(!PlayerController.negMoving)
        transform.position = new Vector3(transform.position.x, transform.position.y, offset + CameraController.cameraPosition.z);
        //z += 0.4f;
        //transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }
}
