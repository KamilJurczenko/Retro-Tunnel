using UnityEngine;

public class ForceDown : MonoBehaviour
{
    [SerializeField] PhysicMaterial floorPhysicsMaterial;
    float cacheBounciness;
    private void Start()
    {
        cacheBounciness = floorPhysicsMaterial.bounciness;
    }
    private void Update()
    {
        if (PlayerController.onFloorCollisionStay == true)
            floorPhysicsMaterial.bounciness = cacheBounciness;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (Mathf.Abs(rb.velocity.y) > 0.01f)
            {
                //PlayerController pc = other.GetComponent<PlayerController>();
                floorPhysicsMaterial.bounciness = 0;
                //Force player on ground with no bounce
                //float t = 0.5f * PlayerController.speedAlteration;
                //float zCalc = PlayerController.zCurrentPlayerVelocity * t + other.transform.position.z;
                //float maxForceFac = PlayerController.playerMaxYSpeed / 4;
                //float a = Mathf.Clamp(PlayerController.yCurrentPlayerVelocity, 1, maxForceFac);
                //rb.AddForce(Vector3.down * 500f * a, ForceMode.Force);
                if(rb.velocity.y > -4)
                    rb.velocity = new Vector3(rb.velocity.x,-4,rb.velocity.z);
            }
            //StartCoroutine(pc.MoveToPoint(new Vector3(other.transform.position.x, PlayerController.extents, zCalc), t));
        }
    }
}
