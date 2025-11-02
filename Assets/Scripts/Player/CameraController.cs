using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Transform followObjectTransform;
    [SerializeField] GameObject[] GameUI;
    [SerializeField] PlayerController playerController;

    Vector2 followObjectOffset;

    public static Vector3 cameraPosition;

    private Vector3 initialPosition;
    private Vector3 playerStartPosition;

    public static float cameraZOffset = 7.5f;
    public static float cameraYOffset = 0.85f;

    private float lerpDuration = 1f;
    private float lerpTimeElapsed;
    private float camWidth;
    private float camHeight;

    public static float zInterpolationVal;
    public static float accelerationInterpolationVal = 0.4f;

    float lerpValue = 0.2f;
    public static bool lerpedToGameStart;

    bool canLerp;
    void Awake()
    {
        lerpedToGameStart = false;
        initialPosition = transform.position;
        //Debug.Log(target.position.y);
        playerStartPosition = new Vector3(target.position.x,
            target.position.y + cameraYOffset, target.transform.position.z - cameraZOffset);
        followObjectOffset = new Vector2(Mathf.Abs(followObjectTransform.position.y - transform.position.y),Mathf.Abs(followObjectTransform.position.z - transform.position.z));
        if (ButtonHandler.startGameButtonCalled)
            FixCameraToPlayer();
        else
            transform.position = initialPosition;
        cameraPosition = transform.position;
    }
    private void Start()
    {
        Camera camera = GetComponent<Camera>();
        camHeight = camera.orthographicSize;
        camWidth = camera.aspect * camHeight;
        zInterpolationVal = 1;
    }

    private void Update()
    {
       
        if (Input.GetMouseButtonUp(0))
        {
            lerpValue = 0;
            canLerp = true;
            lerpTimeElapsed = 0;

        }
        else if (Input.GetMouseButtonDown(0))
        {
            canLerp = false;
            lerpValue = 0.2f;
        }
        else if(!Input.GetMouseButton(0))
        {
            lerpValue = lerpTimeElapsed / lerpDuration;
            lerpValue = lerpValue * lerpValue * (3f - 2f * lerpValue);
        }
        if (canLerp) lerpTimeElapsed += Time.deltaTime;
        if (lerpValue < 0) lerpValue = 0.2f;
    } 
    // Update is called once per frame
    void FixedUpdate()
    {
        cameraPosition = transform.position;

        followObjectTransform.position = new Vector3(followObjectTransform.position.x, followObjectTransform.position.y, transform.position.z + followObjectOffset.y);

        if ((lerpedToGameStart || ButtonHandler.startGameButtonCalled) && GameManager.gameStarted)
        {
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, Mathf.Clamp(target.position.x, RoomConstants.leftWallPosition.x + camWidth / 2, RoomConstants.rightWallPosition.x - camWidth / 2), lerpValue),
                Mathf.Lerp(transform.position.y, Mathf.Clamp(target.position.y + cameraYOffset, RoomConstants.floorPosition.y, RoomConstants.ceilingPosition.y - camHeight / 3), 0.3f),
                Mathf.Lerp(transform.position.z, target.position.z - cameraZOffset, zInterpolationVal));
        }
    }
    public void FixCameraToPlayer()
    {
        transform.position = new Vector3(target.position.x,
            target.position.y + cameraYOffset, target.transform.position.z - cameraZOffset);
    }
    public IEnumerator LerpToPlayer()
    {
        float t = 0;
        while(t < 1)
        {
            t += 0.025f;
            transform.position = Vector3.Lerp(initialPosition, playerStartPosition, t);
            yield return new WaitForFixedUpdate();
        }
        lerpedToGameStart = true;
        playerController.PrepareGameStart();
        foreach (GameObject go in GameUI)
            go.SetActive(true);
    }
}
