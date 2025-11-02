using UnityEngine;

public class AccelerationParticlesTrail : MonoBehaviour
{
    [SerializeField] Transform objectToFollow;
    [SerializeField] Vector3 offsetPosition;
    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = objectToFollow.transform.position + offsetPosition;
        transform.localEulerAngles = new Vector3(0f, PlayerController.yAngle/2, 0f);// + offsetRotation; 
    }
}
