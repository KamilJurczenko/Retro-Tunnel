using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public PlayerDead playerDead;
    [SerializeField] GameObject accelerationParticleObject;
    [SerializeField] RestartPanel restartPanel;
    [SerializeField] GameObject pauseButton;
    [SerializeField] ObjectDissolveFX playerDissolveFX;
    [SerializeField] PlayerVariablesSave playerVariablesSave;
    [SerializeField] AudioSource bounceSoundFX;
    [SerializeField] AudioSource backgroundmusic;
    [SerializeField] AudioSource gameOverSoundFX;
    [SerializeField] AudioSource spawnSoundFX;
    [SerializeField] GameManager gameManager;
    [SerializeField] CameraController cameraController;

    Renderer rend;
    Collider coll;

    [SerializeField] Transform meshObject;

    public static bool canMove;

    public Rigidbody rb;

    public static float zCurrentPlayerVelocity;
    public static float yCurrentPlayerVelocity;

    [SerializeField] private float startPlayerSpeed;
    private float staticStartSpeed = 15f;
    //[SerializeField] private float xMaxSpeed;
    [SerializeField] private float xMaxInputSpeed;
    private float rotationSpeed = 3f;
    // Input Properties
    public static float xSpeed;
    public static float playerMaxYSpeed = 12.5f;
    float ySpeedFactor = 0.08f;
    float ySlowFactor = 0.96f;

    private float xDragVal = 0.05f;
    public static float extents;
    public static Vector3 playerPosition;
    public static Vector3 playerStartPosition;

    public static float currentScore; // Player Score = Players z Position / playerMetersFactor
    public static float playerMetersFactor = 2;

    //private float zTargetSpeed;
    public static int maxSpeedIncreaseCount = 6; // Maximal counter to increase Players speed
    public static int speedIncreaseCounter;
    public static float speedIncrement = 2.0f;
    public static float speedAlteration = 1; // newSpeed / oldSpeed
    public static float nextStageSpeedAlteration;
    public static bool onFloorCollisionStay;

    private bool scoreCountStart = false;
    private float zStart;
    public bool playerForcedMove = false;

    private float xRot;
    public bool noCeilMode = false; // Player can only reach explicit amount of speed to reach 
    private float maxVelNoCeil;
    public static bool negMoving; //player moving in negative direction
    public static float lastCollisionPoint;
    private int framesCollisionStay;
    public static float startScoreCountPos;
    public static float yAngle;

    public static int deathCount;

    private float remapedValToCenter; // Value to move to tunnel center point

    Shader opaqueShader;
    Shader transparentShader;

    private void Awake()
    {
        deathCount = 0;
        playerStartPosition = new Vector3(0, 5f, 0);
        transform.position = playerStartPosition;
    }
    // Start is called before the first frame update
    void Start()
    {
        transparentShader = Shader.Find("Shader Graphs/Retro_Grid_Transparent");
        opaqueShader = Shader.Find("Shader Graphs/Retro_Grid");
        rend = meshObject.GetComponent<Renderer>();
        coll = transform.GetComponent<Collider>();

        speedIncreaseCounter = 0;
        yAngle = 0;
        extents = rend.bounds.extents.y;
        startScoreCountPos = playerStartPosition.z;
        canMove = false;
        negMoving = false;
        onFloorCollisionStay = false;
        xSpeed = 0;
        currentScore = 0;
        playerPosition = playerStartPosition;
        rb = transform.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        //InvokeRepeating("IncrementSpeedDebug",2.0f,3f); //Debug purpose for speed incrementing
        zCurrentPlayerVelocity = startPlayerSpeed;
        speedAlteration = zCurrentPlayerVelocity / staticStartSpeed;
        nextStageSpeedAlteration = (zCurrentPlayerVelocity + speedIncrement) / zCurrentPlayerVelocity;
        maxVelNoCeil = Mathf.Sqrt((Mathf.Abs(RoomConstants.floorPosition.y - RoomConstants.ceilingPosition.y) - coll.bounds.size.y * 2) * -Physics.gravity.y * 2);
        /*if(GameManager.gameIsRestarting)
        {
            //backgroundmusic.Play();
            StartCoroutine(StartDissolve());
        }*/
        //Debug.Log("Max noCeilVel: " + maxVelNoCeil);
    }
    public IEnumerator StartDissolve()
    {
        rend.material.shader = transparentShader;
        rend.material.renderQueue = 3200;
        if(GameManager.playSoundFX)
            spawnSoundFX.Play();
        yield return StartCoroutine(playerDissolveFX.StartDissolve(0));
        spawnSoundFX.Stop();
        rend.material.shader = opaqueShader;
        gameManager.OnGameStart();
    }
    private void IncrementSpeedDebug()
    {
        //SetPlayerVelocity(zCurrentPlayerVelocity + 2f,'z');
    }

    // Update is called once per frame
    void Update()
    {
        xSpeed = Joystick.joystickInputAxis.x * xMaxInputSpeed;
        if (PlayerVariablesSave.tutorialPlayed)
        {
            if (!scoreCountStart)
            {
                //Debug.Log("SCORE START SET");
                scoreCountStart = true;
                zStart = transform.position.z;
            }
            currentScore = transform.position.z - zStart;
            currentScore *= 2;
        }
        if (!GameManager.gameStarted && ButtonHandler.restartButtonCalled)
        {
            ButtonHandler.restartButtonCalled = false;
            PrepareGameStart();
        }
        if (Joystick.dragging && !GameManager.gameStarted)
        {
            OnGameStart();
        }

        playerPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z); // z,y      
        rb.angularVelocity = Vector3.zero;
        yCurrentPlayerVelocity = rb.velocity.y;
        //Debug.Log(lookAt);
    }
    private void FixedUpdate()
    {
        if (GameManager.gameStarted && !GameManager.gamePaused)
        {
            float xRotationSpeed = 1f;
            if (playerForcedMove)
            {
                float xDistance = -playerPosition.x;
                remapedValToCenter = ExtensionMethods.Remap(xDistance, 0, RoomConstants.singleFloorWidth / 2, 0, 1);

                Quaternion qr = Quaternion.Euler(0, yRotation(remapedValToCenter), 0);
                transform.localRotation = Quaternion.Lerp(transform.localRotation, qr, Time.deltaTime * 5);

                xRotationSpeed = 0.5f;
            }
            else if (!playerForcedMove && Joystick.dragging)
            {
                transform.localRotation = Quaternion.Euler(0, yRotation(Joystick.joystickInputAxis.x), 0);
            }
            // No Input
            else
            {
                Quaternion qr = Quaternion.Euler(0, 0, 0);
                transform.localRotation = Quaternion.Lerp(transform.localRotation, qr, Time.deltaTime * 5);
            }
            //Spinning
            meshObject.localRotation = Quaternion.Euler(xRotation(xRotationSpeed), 0, 0);
        }


        if (canMove) // Player Input -> Move to desired position
        {
            // Y Movement
            float t = 1;
            if ((rb.velocity.y > 0 && Joystick.joystickInputAxis.y < 0) || (rb.velocity.y < 0 && Joystick.joystickInputAxis.y > 0))
            {
                t = 1 - (Mathf.Abs(Joystick.joystickInputAxis.y) * (1 - ySlowFactor)); // slowing player down
            }
            else
            {
                if (Joystick.joystickInputAxis.y < 0)
                    t = Mathf.Abs(ySpeedFactor * Joystick.joystickInputAxis.y) + 1; // increase player speed
            }
            //Debug.Log(t);
            if(Mathf.Abs(rb.velocity.y * t) < playerMaxYSpeed)
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * t, zCurrentPlayerVelocity);

            // X Movement
            //SetPlayerXVelocity(xSpeed);
                //accept only oposite direction input
                /*float overFlow = Mathf.Abs(xMaxSpeed - Mathf.Abs(xSpeed + rb.velocity.x));
                xSpeed += overFlow * Mathf.Sign(xSpeed) * -1;*/
                //Debug.Log(overFlow);
            if (Mathf.Abs(rb.velocity.x) < xMaxInputSpeed) 
            {
                float sign = Mathf.Sign(xSpeed);
                xSpeed = sign * Mathf.Clamp(Mathf.Abs(xSpeed),0 , xMaxInputSpeed - Mathf.Abs(rb.velocity.x));
                rb.MovePosition(rb.position + Vector3.right * xSpeed * Time.fixedDeltaTime);
            } 
        }

        //Add Drag when Player.x Velocity greater 0
        if (Mathf.Abs(rb.velocity.x) >= 0)
        {
            float xVel = rb.velocity.x;
            xVel /= (xDragVal + 1);
            SetPlayerXVelocity(xVel);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Obstacle"))
        {
            // Play Death Animation via Particle System
            ContactPoint contactPoint = collision.GetContact(0);
            OnGameOver();
        }
        else if (collision.collider.CompareTag("Floor"))
        {
            if (noCeilMode && rb.velocity.y > maxVelNoCeil)
            {
                rb.velocity = new Vector2(rb.velocity.x, maxVelNoCeil);
            }
            if(GameManager.playSoundFX)
                bounceSoundFX.Play();
            PlayerVariablesSave.floorBouncesCounter++;
        }
        else if (collision.collider.CompareTag("Obstacle_Tut"))
        {
            TutorialManager.tutObstHit = true;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            framesCollisionStay++;
            if (framesCollisionStay > 10)
            {
                //Debug.Log("Floor Collision Stay!");
                onFloorCollisionStay = true;
                framesCollisionStay = 0;
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Floor"))
            onFloorCollisionStay = false;
    }
    private float yRotation(float align)
    {
        yAngle = Vector3.Angle(new Vector2(0, 1f), new Vector2(align,1f));
        // Default Rotation (0,1)
        Vector3 cross = Vector3.Cross(new Vector2(0, 1f), new Vector2(align, 1f));
        if (cross.z > 0) yAngle = -yAngle;
        //Debug.Log(yAngle);
        return yAngle / 2;
    }
    private float xRotation(float xRotSpeed = 1)
    {
        xRot += rotationSpeed * speedAlteration * xRotSpeed;
        if (xRot >= 360) xRot = 0;
        return xRot;
    }
    public void PrepareGameStart()
    {
        if (!backgroundmusic.isPlaying && GameManager.playMusic)
            backgroundmusic.Play();

        StartCoroutine(StartDissolve());
    }
    private void OnGameStart()
    {
        GameManager.gameStarted = true;
        GameManager.gamePaused = false;
        rb.isKinematic = false;
        canMove = true;
    }
    public void OnGameOver()
    {
        if (!GameManager.dD)
        {
            deathCount++;
            canMove = false;
            //deathParticlesRend.enabled = true;
            if(GameManager.playSoundFX)
                gameOverSoundFX.Play();
            backgroundmusic.Stop();
            playerDead.AnimatePlayerDeath();
            GameManager.gameOver = true;
            rb.isKinematic = true;
            Joystick.dragActive = false;
            rotationSpeed = 0;
            rend.enabled = false;
            pauseButton.SetActive(false);
            restartPanel.SetActivePanel(true);

            playerVariablesSave.SavePlayerVariables();
        }
        // TODO Save Player Variables
    }
    public void Respawn()
    {
        pauseButton.SetActive(true);
        restartPanel.SetActivePanel(false);
        playerDead.StopParticles();
        GameManager.gamePaused = true;
        GameManager.gameStarted = false;
        rend.enabled = true;
        Joystick.dragActive = true;
        GameManager.gameOver = false;
        rotationSpeed = 3f;
        meshObject.localRotation = Quaternion.Euler(0, 0, 0);
        int a = StageManager.stageIndex;
        Vector3 respawnPosition = Vector3.zero;

        if(a != 2 || (a % 2 == 0 && a > 3)) // Obstacle Stage
        {
            respawnPosition = new Vector3(0,playerStartPosition.y, GameObjectSpawner.respawnCheckpoint);
        }
        else if(a < 4 || (a % 2 != 0 && a > 3)) // Lava Stage
        {
            float h_0 = 3f;
            float w = (zCurrentPlayerVelocity * Mathf.Sqrt(2*-Physics.gravity.y* h_0))/ -Physics.gravity.y;
            Debug.Log("W: " + w);
            respawnPosition = new Vector3(GameObjectSpawner.nextObstacleVec2.x,
                GameObjectSpawner.nextObstacleVec2.y + h_0,
                GameObjectSpawner.respawnCheckpoint + LavaStageManager.lavaPlatformSpawnDist - w);
        }
        transform.position = respawnPosition;
        cameraController.FixCameraToPlayer();
        PrepareGameStart();
    }
    public IEnumerator MoveToPoint(Vector3 p, float speed = 0, bool particles = false)
    {
        if (!GameManager.gameOver)
        {
            if (speed == 0)
                speed = zCurrentPlayerVelocity;
            if(particles) accelerationParticleObject.SetActive(true);
            playerForcedMove = true;
            Debug.Log("Moving to: " + p + "with " + speed + " meter/seconds");
            if (p.z < playerPosition.z)
                negMoving = true;
            rb.isKinematic = true;
            canMove = false;
            while (Vector3.Distance(playerPosition, p) > 0.2f)
            {
                if (speed != 0 && CameraController.zInterpolationVal >= CameraController.accelerationInterpolationVal)
                {
                    CameraController.zInterpolationVal -= Time.deltaTime;
                }
                transform.position = Vector3.MoveTowards(transform.position, p, speed * Time.deltaTime);
                yield return new WaitForFixedUpdate();
            }
            CameraController.zInterpolationVal = 1;
            rb.isKinematic = false;
            accelerationParticleObject.SetActive(false);
            canMove = true;
            negMoving = false;
            playerForcedMove = false;
        }
    }
    public IEnumerator SetPlayerZVelocity(float velocity, float time = 0)
    {
        zCurrentPlayerVelocity = velocity;
        speedAlteration = zCurrentPlayerVelocity / staticStartSpeed;
        Debug.Log("New SpeedAlteration: " + speedAlteration);
        if (time == 0)
            rb.velocity = new Vector3(rb.velocity.x,rb.velocity.y,velocity);
        Debug.Log("Increasing Players Speed by: " + velocity + "over: " + time + " seconds");
        yield return null;
    }
    public void SetPlayerYVelocity(float velocity)
    {
        //Debug.Log("Setting Y Vel");
        rb.velocity = new Vector3(rb.velocity.x, velocity, rb.velocity.z);
    }
    public void SetPlayerXVelocity(float velocity)
    {
        //float sign = Mathf.Sign(velocity);
        //velocity = Mathf.Clamp(Mathf.Abs(velocity),0, xMaxSpeed) * sign;
        //Debug.Log("Setting X Vel");
        rb.velocity = new Vector3(velocity, rb.velocity.y, rb.velocity.z);
    }
}
