using UnityEngine;

public class WallCollisionFX : MonoBehaviour
{
    [SerializeField] private bool addBoost;
    [SerializeField] AudioSource bounceSoundFX;
    private ManageCollParticlePool manageCollParticlePool;
    private float addVelocity = 10f;
    private bool stayCollisionActive = false;
    //private ParticleSystem instantiatedStayFX;

    private float remapMinVal = 0.4f;

    //private static Transform[] poolingList;

    //private static Queue<ParticleSystem> qPool;
    //private int poolAmount = 3;

    private void Start()
    {
        manageCollParticlePool = transform.parent.GetComponent<ManageCollParticlePool>();

        //poolingList = new Transform[poolAmount];
        // Pool x Amount of Wall Collision Particles
        //if (poolingList == null) 

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if(bounceSoundFX != null && GameManager.playSoundFX)
                bounceSoundFX.Play();
            ContactPoint contactPoint = collision.GetContact(0);
            PlayerController pc = collision.collider.GetComponent<PlayerController>();
            Rigidbody rb = pc.rb;
            //Debug.Log("Creating Wall Collision FX in Direction: " + contactPoint.normal);
            //var collisionFX = Instantiate(collisionFXPrefab, contactPoint.point, Quaternion.identity);
            //Pooling
            var collisionFX = manageCollParticlePool.qPool.Dequeue();
            //collisionFX.gameObject.SetActive(true);
            collisionFX.Play();
            manageCollParticlePool.qPool.Enqueue(collisionFX);
            //collisionFX.transform.rotation = Quaternion.identity;
            collisionFX.transform.position = contactPoint.point;
            float playerSpeed = rb.velocity.y; // 3min - 10max 0.2-1
            //Debug.Log("Collision Speed: " + playerSpeed);
            float remapVal = remapMinVal + (playerSpeed - 3) * (1 - remapMinVal) / (10 - 3);
            remapVal = Mathf.Clamp(remapVal, remapMinVal, 1f);
            collisionFX.startSize = remapVal;
            //Debug.Log("RemapVal: " + remapVal);
            collisionFX.transform.rotation = Quaternion.LookRotation(contactPoint.normal);
            if (addBoost)
            {
                if (contactPoint.normal.x != 0)
                    pc.SetPlayerXVelocity((addVelocity) * contactPoint.normal.x * -1 );
                else
                    pc.SetPlayerYVelocity(addVelocity * contactPoint.normal.y * -1);
            }
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && !addBoost)
        {
            ContactPoint contactPoint = collision.contacts[0];
            if (!stayCollisionActive)
            {
                //stayFxTransform = Instantiate(collisionStayFXPrefab, contactPoint.point, Quaternion.identity);
            manageCollParticlePool.stayFxTransform.Play();
            //manageCollParticlePool.stayFxTransform.transform.rotation = Quaternion.LookRotation(contactPoint.normal);
            }
            manageCollParticlePool.stayFxTransform.transform.position = new Vector3(collision.collider.transform.position.x, contactPoint.point.y, contactPoint.point.z);
            stayCollisionActive = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        stayCollisionActive = false;
        //if(manageCollParticlePool.stayFxTransform != null)
        //manageCollParticlePool.stayFxTransform.Stop();
    }
}
 