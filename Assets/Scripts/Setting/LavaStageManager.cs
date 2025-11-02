using UnityEngine;

public class LavaStageManager : MonoBehaviour
{
    [SerializeField] Transform startGroundTransform;

    [SerializeField] GameObjectSpawner lavaPlatformSpawner;
    [SerializeField] GameObjectSpawner obstacleSpawner;
    [SerializeField] SettingPooling lavaPooling;

    [SerializeField] BouncyPlatform lavaPlatform;

    private float zGroundExtent;

    // For Default Lava Platforms (TODO add Platform with higher yVelocity)
    private float yMinSpawn;
    private float yMaxSpawn;
    private float yMaxHeightJumpSteps;
    private float yPlatExtent;
    private float zPlatExtent;
    private float zMaxJumpLength;
    public static float lavaPlatformSpawnDist;

    private float startPos;
    private float minGroundStartPos = 50f;
    private float maxGroundStartPos = 80f; //is prioritized

    private float t;
    // Start is called before the first frame update
    void Awake()
    {
        lavaPlatformSpawnDist = 0;
        zGroundExtent = startGroundTransform.GetComponent<Renderer>().bounds.extents.z;
        yPlatExtent = lavaPlatformSpawner.objHeight / 2;
        zPlatExtent = lavaPlatformSpawner.objLength / 2;
        yMinSpawn = RoomConstants.floorPosition.y + yPlatExtent;
        float maxReachableHeight = Mathf.Pow(lavaPlatform.velocity, 2) / (2 * Physics.gravity.magnitude);
        yMaxHeightJumpSteps = maxReachableHeight - yPlatExtent - 1f;
        yMaxHeightJumpSteps = (Mathf.Floor(yMaxHeightJumpSteps / (yPlatExtent))) * yPlatExtent;
        Debug.Log("Player reach max: " + maxReachableHeight + " Height on Platform Collision");
        yMaxSpawn = RoomConstants.ceilingPosition.y - yPlatExtent - maxReachableHeight;
        t = (lavaPlatform.velocity / Physics.gravity.magnitude) * 2;
    }
    public void InstantiateLavaStage(float start, float end)
    {
        zMaxJumpLength = PlayerController.zCurrentPlayerVelocity * t;
        float zSpawnPosOffset = zMaxJumpLength / 5;
        lavaPlatformSpawnDist = zMaxJumpLength - zSpawnPosOffset;

        startPos = start;
        float endSpawnPos = end;
        float startEndSpawnDistance = Mathf.Abs((startPos + maxGroundStartPos) - endSpawnPos);
        //Debug.Log(endSpawnPos);
        //Debug.Log(startEndSpawnDistance);
        //Debug.Log(lavaSpawnDistance);
        float maxSpawnableInstances = startEndSpawnDistance / lavaPlatformSpawnDist;

        float missFac = 1 - (maxSpawnableInstances - (int)maxSpawnableInstances);
        float missDist = missFac * lavaPlatformSpawnDist;
        //Debug.Log("missdist: " + missDist);
        if (startEndSpawnDistance + missDist > minGroundStartPos)
            startEndSpawnDistance += missDist;
        float groundEndPos = Mathf.Abs(Mathf.Abs(startPos - endSpawnPos) - startEndSpawnDistance);
        //Debug.Log(startEndSpawnDistance);
        float groundPos = startPos + groundEndPos - zGroundExtent;
        startGroundTransform.transform.position = new Vector3(0, 0, groundPos);
        startGroundTransform.gameObject.SetActive(true);

        float startOffsetPos = 10f; //* PlayerController.speedAlteration;
        float startLavaPlatformSpawnPos = startGroundTransform.position.z + zGroundExtent + zPlatExtent + startOffsetPos;
        float ySpawnStep = (Mathf.Floor(yMaxSpawn / (yPlatExtent))) * yPlatExtent;

        lavaPlatformSpawner.SetSpawnProperties(startLavaPlatformSpawnPos,
        lavaPlatformSpawnDist, endSpawnPos,6 , yMinSpawn, ySpawnStep,
          startGroundTransform.position.y, yPlatExtent,
          yMaxHeightJumpSteps);
        lavaPlatformSpawner.SetStartPoolPos(1);
        lavaPlatformSpawner.dynSpawnBool = true;

        float startObstacleSpawn = startLavaPlatformSpawnPos + lavaPlatformSpawnDist / 2 + lavaPlatformSpawnDist * 2;
        obstacleSpawner.SetSpawnProperties(startObstacleSpawn, lavaPlatformSpawnDist, endSpawnPos - 10f, 3);

        lavaPooling.SetPooling(startGroundTransform.transform.position.z + zGroundExtent, true);

    }
}
