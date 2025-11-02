using UnityEngine;

public class BackgroundPlatformOffsetMove : MonoBehaviour
{
    private Renderer rend;
    private float t;
    [SerializeField] private float moveSpeed = 0.5f;
    private float startMoveSpeed;

    private float alteration = 1;
    //private bool moving;
    // Start is called before the first frame update
    void Awake()
    {
        rend = GetComponent<Renderer>();
        startMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerController.speedAlteration != alteration)
        {
            moveSpeed = startMoveSpeed * PlayerController.speedAlteration;
            alteration = PlayerController.speedAlteration;
        }
    }
    private void FixedUpdate()
    {
        if (!GameManager.gameOver && !GameManager.gamePaused && GameManager.gameStarted)
        {
            if (!PlayerController.negMoving)
            {
                t += Time.deltaTime * moveSpeed;
            }
            else
            {
                t -= Time.deltaTime * moveSpeed;
            }
            rend.material.SetFloat("_yOffset", t);
        }
    }
}
