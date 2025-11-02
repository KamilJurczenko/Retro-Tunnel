using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] PlayerScore playerScorecs;
    [SerializeField] ObstacleManager obstacleManager;
    [SerializeField] LavaStageManager lavaStageManager;
    [SerializeField] int newStageAtScore;
    [SerializeField] GroundSetup groundSetup;

    //public static Stage currentStageScore;
    public static int currentNextStagePoint;
    private int showScoreAt;
    //public static int newStageDividerIncrement = 1000; // increases (1000,2500,5000,10000,...)
    public static int stageIndex;
    public static float obstacleSpawnDist;
    private float startSpawnDist;
    public static float startOffsetPos;
    bool b;
    void Awake()
    {
        obstacleSpawnDist = 30f;
        startSpawnDist = obstacleSpawnDist;
        currentNextStagePoint = newStageAtScore;
        showScoreAt = currentNextStagePoint;
    }
    // Start is called before the first frame update
    void Start()
    {
        stageIndex = GameManager.sS - 1;
        if(PlayerVariablesSave.tutorialPlayed)
            SetNextStage(0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float divB = PlayerController.currentScore / showScoreAt;
        //Debug.Log(divB);
        if ((int)divB == 1 && !b)
        {
            AtNewStage();
            b = true;
        }
        else if((int)divB != 1)
        {
            b = false;
        }
    }
    public void SetNextStage(float startPos = -1)
    {
        float start;
        float end;
        //Start Stage
        if (startPos != -1)
        {
            start = PlayerController.playerPosition.z;
            end = TunnelManager.GetCurrentPositions()[0].z;
        }
        else
        {
            PrepareNextStage();
            start = TunnelManager.GetCurrentPositions()[1].z;
            end = TunnelManager.GetNextPositions()[0].z;
        }
        Debug.Log("Setting Next Stage. Index: " + stageIndex);
        end -= 10f; //Offset
        startOffsetPos = obstacleSpawnDist * 2;
        if (stageIndex == 0)
        {
            obstacleManager.InstantiateObstacleStage(start + startOffsetPos, obstacleSpawnDist, end, 1);
            groundSetup.SetGround(0);
        }
        else if(stageIndex == 1)
        {
            obstacleManager.InstantiateObstacleStage(start + startOffsetPos, obstacleSpawnDist, end, 2);
            groundSetup.SetGround(0);
        }
        else if(stageIndex == 2)
        {
            lavaStageManager.InstantiateLavaStage(start, end);
            groundSetup.SetGround(1);
        }
        else if(stageIndex == 3)
        {
            obstacleManager.InstantiateObstacleStage(start + startOffsetPos, obstacleSpawnDist, end, 3);
            groundSetup.SetGround(0);
        }
        else
        {
            //Spawn Universal Obstacle Stage
            if (stageIndex % 2 == 0)
            {
                obstacleManager.InstantiateObstacleStage(start + startOffsetPos, obstacleSpawnDist, end, 4);
                groundSetup.SetGround(0);
            }
            //Spawn Lava Stage
            else
            {
                lavaStageManager.InstantiateLavaStage(start, end);
                groundSetup.SetGround(1);
            }
        }
        //Debug.Log("Starting Stage at: " + start);
        //Debug.Log("Ending Stage at: " + );
    }
    public void PrepareNextStage()
    {
        stageIndex++;
        obstacleSpawnDist = startSpawnDist * PlayerController.nextStageSpeedAlteration;
        // Stage Loop
        // Spawn Random Obstacles from all Obstacle Stages and transit to Lava Stage sometime
        /*if (stageIndex > (stageOrder.Length - 1))
        {
            int tmp = newStagePoints[newStagePoints.Length - 1];
            for (int i = 0; i < newStagePoints.Length; i++)
            {
                newStagePoints[i] += tmp;
            }
            //noSpawnDist = spawnDistance * 3;
            stageIndex = 0;
            Debug.Log("Stage loop!");
        } */
        currentNextStagePoint += 1000;
    }
    // Called when player reaches new Stage Score
    public void AtNewStage()
    {
        playerScorecs.OnNewStage();
        //Debug.Log("At Stage: " + currentStageSpawner);
        //currentStageScore = currentStageSpawner;
        // Change Room Color
        //colorSettingcs.ChangeRoomColor();
        showScoreAt = currentNextStagePoint;
    }
}
