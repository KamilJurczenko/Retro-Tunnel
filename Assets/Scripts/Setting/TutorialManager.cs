using System.Collections;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Transform tutorialObstacleTransform;
    [SerializeField] private Transform tutorial_UI_HAND_Transform;
    [SerializeField] private Animator inputTutorialAnimator;
    [SerializeField] private Transform inputTransform;
    [SerializeField] private GameObject darkenObject;
    [SerializeField] private GameObject playerScoreObject;
    [SerializeField] private StageManager stageManager;
    [SerializeField] private TunnelManager tunnelManager;
    [SerializeField] private GameManager gameManager;

    [SerializeField] private PlayerController player;
    [SerializeField] private Joystick playerInputInstance;

    private int[] inputDirOrder = new int[] {0, 3, 3, 3, 1, 2, 3, 4, 3};

    private float[] obstPositions;

    private float currentCheckpointPosition;
    private float nextObstaclePosition;
    private int nextObstacleIndex = 0;
    private int removeObstacleIndex = 0;

    private float obstDist;

    public static bool tutObstHit;

    private float lastObstPosition;

    Vector2 desiredInput;

    private bool endCon;

    private bool firstTutorialPhaseFinished = false;
    private bool firstTouch = false;
    private bool askingForInput = false;

    private bool showInputHelp = false;
    /* Spawn Obstacles with helped Input
     * Spawn same Obstacles with helped Input but no time pause
    */

    // Start is called before the first frame update
    void Start()
    {
        ResumeTutorialUI();
        tutorialObstacleTransform.gameObject.SetActive(true);
        playerScoreObject.SetActive(false);
        PlayInputAnimation(inputDirOrder[nextObstacleIndex]);
        obstPositions = new float[tutorialObstacleTransform.childCount];
        for (int i = 0; i < obstPositions.Length; i++)
        {
            obstPositions[i] = tutorialObstacleTransform.GetChild(i).transform.position.z;
            if(i == 3)
            {
                obstPositions[i] -= 5f;
            }
            else if(i == 5 || i == 7)
            {
                obstPositions[i] += (obstDist / 2) + 5f;
            }
        }
        SetNextObstaclePosition();
        Joystick.dragActive = false;
        obstDist = Mathf.Abs(tutorialObstacleTransform.GetChild(0).transform.position.z - tutorialObstacleTransform.GetChild(1).transform.position.z);
        player.noCeilMode = true;
        lastObstPosition = obstPositions[obstPositions.Length - 1];
    }

    // Update is called once per frame
    void Update()
    {
        // First Input
        if (Joystick.dragging && !firstTouch)
        {
            StopTutorialUI();
            inputTransform.parent.gameObject.SetActive(false);
            firstTouch = true;
        }
        // First Tutorial Phase
        if(PlayerController.playerPosition.z > (nextObstaclePosition - (obstDist / 2) - 5f) && !firstTutorialPhaseFinished
            && (PlayerController.yCurrentPlayerVelocity < 0 && inputDirOrder[nextObstacleIndex] == 3 || inputDirOrder[nextObstacleIndex] != 3))
        {
            if (!askingForInput)
                StartCoroutine(AskForInput(inputDirOrder[nextObstacleIndex]));
        }
        if (firstTutorialPhaseFinished)
        {
            // Second Tutorial Phase
            if (PlayerController.playerPosition.z > (nextObstaclePosition + 5f))
            {
                currentCheckpointPosition = nextObstaclePosition;
                if (nextObstacleIndex < obstPositions.Length)
                    SetNextObstaclePosition();
            }
            if(removeObstacleIndex < tutorialObstacleTransform.childCount &&
                CameraController.cameraPosition.z > tutorialObstacleTransform.GetChild(removeObstacleIndex).transform.position.z)
            {
                tutorialObstacleTransform.GetChild(removeObstacleIndex).gameObject.SetActive(false);
                removeObstacleIndex++;
            }
            // On Second Phase Obstacle Collision
            if (tutObstHit)
            {
                Debug.Log("Obstacle Hit in Tutorial");
                showInputHelp = true;
                StartCoroutine(player.MoveToPoint(new Vector3(PlayerController.playerStartPosition.x, PlayerController.playerStartPosition.y, currentCheckpointPosition), PlayerController.zCurrentPlayerVelocity * 1.5f));
                // Show Input once
                tutObstHit = false;
            }
            if(showInputHelp && !player.playerForcedMove)
            {
                showInputHelp = false;
                ResumeTutorialUI();
                PlayInputAnimation(inputDirOrder[nextObstacleIndex]);
            }
            if (GameManager.gamePaused && Joystick.dragging)
            {
                StopTutorialUI();
            }
        }

        // On End Phase
        if(PlayerController.playerPosition.z > lastObstPosition)
        {
            if (firstTutorialPhaseFinished)
            {
                TutorialFinished();
                player.noCeilMode = false;
            }
            else
            {
                Joystick.dragAxis = Vector2.zero;
                Joystick.dragActive = true;
                nextObstacleIndex = 1;
                firstTutorialPhaseFinished = true;
                tutorialObstacleTransform.transform.position = new Vector3(tutorialObstacleTransform.transform.position.x, tutorialObstacleTransform.transform.position.y, 290f);
                for (int i = 0; i < obstPositions.Length; i++)
                {
                    obstPositions[i] = tutorialObstacleTransform.GetChild(i).transform.position.z;
                }
                lastObstPosition = obstPositions[obstPositions.Length - 1];
                nextObstaclePosition = obstPositions[0];
                currentCheckpointPosition = nextObstaclePosition - obstDist;
            }
        }

    }
    private IEnumerator AskForInput(int dir)
    {
        askingForInput = true;
        Joystick.dragActive = true;
        ResumeTutorialUI(!firstTutorialPhaseFinished);
        PlayInputAnimation(dir);
        endCon = false;
        Joystick.dragAxis = desiredInput;
        int cachedFloorBounce = PlayerVariablesSave.floorBouncesCounter;
        while (!endCon)
        {
            switch (dir)
            {
                // Floor Collision Condition
                case 3:
                    if (cachedFloorBounce != PlayerVariablesSave.floorBouncesCounter)
                        endCon = true;
                    break;
                case 1:
                    if (PlayerController.playerPosition.y < 4f)
                        endCon = true;
                    break;
                case 2:
                    if (PlayerController.playerPosition.x > 2.5f)
                        endCon = true;
                    break;
                case 4:
                    if (PlayerController.playerPosition.x < -2.5f)
                        endCon = true;
                    break;
            }
            if (Joystick.joystickInputAxis.magnitude < 0.6f)
            {
                ResumeTutorialUI(!firstTutorialPhaseFinished);
                PlayInputAnimation(dir);
            }
            else StopTutorialUI();
            yield return null;
        }
        StopTutorialUI();
        Joystick.dragActive = false;
        if (nextObstacleIndex < obstPositions.Length)
            SetNextObstaclePosition();
        askingForInput = false;
    }
    private void SetNextObstaclePosition()
    {
        nextObstaclePosition = obstPositions[nextObstacleIndex];
        nextObstacleIndex++;
    }
    private void ResumeTutorialUI(bool gamePause = true)
    {
        //Debug.Log("Resume Tutorial UI");
        inputTransform.parent.gameObject.SetActive(true);
        tutorial_UI_HAND_Transform.gameObject.SetActive(true);
        if (gamePause)
        {
            Time.timeScale = 0;
            GameManager.gamePaused = true;
            darkenObject.SetActive(true);
        }
    }
    private void StopTutorialUI()
    {
        //Debug.Log("Stop Tutorial UI");
        tutorial_UI_HAND_Transform.gameObject.SetActive(false);
        Time.timeScale = 1;
        GameManager.gamePaused = false;
        darkenObject.SetActive(false);
    }
    public void TutorialFinished()
    {
        playerScoreObject.SetActive(true);
        playerScoreObject.GetComponent<PlayerScore>().OnTutorialEnd();
        PlayerPrefs.SetInt("tutorialPlayed", 1);
        Debug.Log("Tutorial finished!");
        // Enter First Stage
        TunnelManager.startPos = PlayerController.playerPosition.z;
        tunnelManager.SetTunnelPosition(StageManager.currentNextStagePoint / PlayerController.playerMetersFactor);
        stageManager.SetNextStage(0);
        PlayerVariablesSave.tutorialPlayed = true;
        PlayerController.startScoreCountPos = PlayerController.playerPosition.z;
        Destroy(this);
    }
    private void PlayInputAnimation(int dir)
    {
        /*
         * 0 - TAP
         * 1 - UP
         * 2 - RIGHT
         * 3 - DOWN
         * 4 - LEFT
        */
        switch (dir)
        {
            case 0:
                inputTutorialAnimator.Play("hand_tutorial_TAP");
                desiredInput = Vector2.zero;
                break;
            case 1:
                inputTutorialAnimator.Play("hand_tutorial_moveUP");
                desiredInput = new Vector2(0, 1);
                break;
            case 2:
                inputTutorialAnimator.Play("hand_tutorial_moveRIGHT");
                desiredInput = new Vector2(1, 0);
                break;
            case 3:
                inputTutorialAnimator.Play("hand_tutorial_moveDOWN");
                desiredInput = new Vector2(0, -1);
                break;
            case 4:
                inputTutorialAnimator.Play("hand_tutorial_moveLEFT");
                desiredInput = new Vector2(-1, 0);
                break;
        }
    }
}
