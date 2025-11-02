using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] Vector3 rotationSpeedVec;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationSpeedVec * Time.deltaTime);
    }
}
