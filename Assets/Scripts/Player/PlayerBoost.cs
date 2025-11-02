using UnityEngine;

public class PlayerBoost : MonoBehaviour
{
    [SerializeField] AudioSource onTriggerSound;
    [SerializeField] float boostVal;
    [SerializeField] ParticleSystem particleEffects;
    public bool boostUsed;
    // Boost force dependent of player z and y speed

    private Vector2 windowPosition;

    private Animator baseAnimator;
    private void Start()
    {
        baseAnimator = GetComponent<Animator>();
    }
    public void SetWindowPosition(Vector2 pos)
    {
        windowPosition = pos;
        //Debug.Log("Setting Window Position on gameobject: " + gameObject.GetInstanceID());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //if (boostUsed == false)
            //{
            onTriggerSound.Play();
            float y_0 = PlayerController.playerPosition.y;
            float vy_0 = PlayerController.yCurrentPlayerVelocity;
            float boostVel;
            if (boostVal != 0)
            {
                boostVel = boostVal;
            }
            else
            {
                float x_0 = PlayerController.playerPosition.z;
                float deltaX = Mathf.Abs(windowPosition.x - x_0);
                //float t = deltaX / PlayerController.zCurrentPlayerVelocity;
                float deltaY = Mathf.Abs(windowPosition.y - y_0);
                float g = Physics.gravity.magnitude;
                boostVel = g * (deltaX / PlayerController.zCurrentPlayerVelocity) / 2 + deltaY / (deltaX / PlayerController.zCurrentPlayerVelocity);
                Debug.Log("Delta.y: " + deltaY);
                Debug.Log("Delta.x: " + deltaX);
                Debug.Log("Window Postion: " + windowPosition);
            }
            float boostVelComp = boostVel + vy_0 * -1;// Compensated Velocity  
            Debug.Log("Boost Velocity: " + boostVel);               
            Debug.Log("Start Player Speed: " + vy_0);

        //Debug.Log("Needed time to reach next Window: " + t + " seconds");// needed time to reach the target peak
            particleEffects.Play();
            Debug.Log("Boost Velocity: " + boostVel);
                
            other.GetComponent<PlayerController>().SetPlayerYVelocity(boostVelComp);
            boostUsed = true;
            baseAnimator.Play("BoostObjUpScale");
            //}
        }
    }

}
