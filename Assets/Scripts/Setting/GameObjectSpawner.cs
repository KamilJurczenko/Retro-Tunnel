using System.Collections.Generic;
using UnityEngine;

public class GameObjectSpawner : MonoBehaviour
{
    [SerializeField] public List<GameObject> staticObjectList;
    [SerializeField] private List<GameObject> appendToStart;
    [SerializeField] private List<GameObject> appendToEnd;
    List<GameObject> currentSpawnedObjects;
    List<GameObject> notSpawnedObjects;

    private float[] objectDistancesArr;
    private float[] initialObjectDistancesArr;

    public List<Vector3> objectPositions; // used to spawn next objects when crossed
    public int startPoolAtIndex = 0;

    private float objWidth;
    public float objHeight;
    public float objLength;

    [SerializeField] bool xStaticPosition = true;
    [SerializeField] bool yStaticPosition = true;
    [SerializeField] bool staticObject = true;
    [SerializeField] bool randomSpawn = false;
    [SerializeField] bool deactivateOnPass = true;

    public float zLastPosition;
    private float zAddition;

    private float yMinSpawn;
    private float yMaxSpawn;
    private float lastYSpawn;
    private float ySpawnSteps;
    private float yMaxJumpHeightStep;
    private float spawnEnd;

    private float currentRandComp;
    private float startRandComp = 0.3f;

    private int spwnCount = 0;

    [SerializeField] bool overrideLastReachedObstacle = false;
    public static float respawnCheckpoint;
    public static Vector2 nextObstacleVec2;
    public static bool nextMovingPlatform;

    private bool firstObjectSpawned;
    public bool dynSpawnBool = false;
    [SerializeField] private int minAmount = 0;

    private void Awake()
    {
        nextMovingPlatform = false;
        respawnCheckpoint = 0;
        currentRandComp = startRandComp;
        currentSpawnedObjects = new List<GameObject>();
        objectPositions = new List<Vector3>();
        if (staticObjectList.Count > 0)
        {
            Renderer rend = staticObjectList[0].GetComponent<Renderer>();
            if (rend != null)
            {
                objWidth = rend.bounds.size.x;
                objHeight = rend.bounds.size.y;
                objLength = rend.bounds.size.z;
                //xDivider = (int)(RoomConstants.singleFloorWidth / objWidth);
            }
        }
        if (staticObject)
        {
            initialObjectDistancesArr = new float[staticObjectList.Count + appendToEnd.Count + appendToStart.Count];
            for (int i = 0; i < appendToStart.Count; i++)
            {
                initialObjectDistancesArr[i] = Mathf.Abs(appendToStart[i].transform.position.z - staticObjectList[0].transform.position.z);
            }
            for (int i = 0; i < staticObjectList.Count - 1; i++)
            {
                initialObjectDistancesArr[appendToStart.Count + i] = Mathf.Abs(staticObjectList[i].transform.position.z - staticObjectList[i + 1].transform.position.z);
            }
            for (int i = 0; i < appendToEnd.Count; i++)
            {
                initialObjectDistancesArr[appendToStart.Count + staticObjectList.Count + i] = Mathf.Abs(appendToEnd[i].transform.position.z - staticObjectList[staticObjectList.Count - 1].transform.position.z);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //SetObjectDistancesInArr();
        if (!GameManager.sO)
            transform.gameObject.SetActive(false);
    }
    void FixedUpdate()
    {
        if (dynSpawnBool && (CameraController.cameraPosition.z > (objectPositions[startPoolAtIndex].z + objLength/2))
            && ((zAddition + zLastPosition) < spawnEnd))
        {
            if (overrideLastReachedObstacle)
            {
                respawnCheckpoint = objectPositions[0].z;
                if (currentSpawnedObjects[1].TryGetComponent<LavaPlatform>(out var lP) && lP.isMoving)
                    nextObstacleVec2 = new Vector2(0, objectPositions[1].y);
                else
                    nextObstacleVec2 = new Vector2(objectPositions[1].x, objectPositions[1].y);
                Debug.Log("Next obstacle: " + nextObstacleVec2);
                Debug.Log("Last reached obstacle: " + respawnCheckpoint);
            }
            if (currentSpawnedObjects[0].TryGetComponent<SpawnProperties>(out var spawnProperties) && spawnProperties.spawnOnce)
            {

            }
            else notSpawnedObjects.Insert(0, currentSpawnedObjects[0]);
            //Debug.Log(currentSpawnedObjects[0].name);
            if(deactivateOnPass)
                currentSpawnedObjects[0].SetActive(false);
            currentSpawnedObjects.RemoveAt(0);
            objectPositions.RemoveAt(0);
            SpawnObject(1);
        }
    }
    public void SetSpawnProperties(float start, float zAdd, float end, int startSpawnAmount,
        float yMin = 0, float yMax = 0, float yStart = 0, float ySteps = 0, float maxHeight = 0)
    {
        currentSpawnedObjects.Clear();
        objectPositions = new List<Vector3>();
        firstObjectSpawned = false;
        yMaxJumpHeightStep = maxHeight;
        ySpawnSteps = ySteps;
        yMinSpawn = yMin;
        yMaxSpawn = yMax;
        lastYSpawn = yStart;
        zAddition = zAdd;
        notSpawnedObjects = new List<GameObject>(staticObjectList);
        zLastPosition = start;
        spawnEnd = end;

        foreach (GameObject go in appendToStart)
        {
            SpawnObject(1, go);
        }
        //SpawnObject(1, null, objectDistancesArr[0]);
        SpawnObject(startSpawnAmount);
        foreach (GameObject go in appendToEnd)
        {
            //SpawnObject(1, go, objectDistancesArr[objectDistancesArr.Length - 1]);
            SpawnObject(1, go);
        }
        if (spawnEnd != 0) dynSpawnBool = true;
    }
    public void SetStartPoolPos(int val)
    {
        startPoolAtIndex = val;
    }
    public float[] GetObjectDistances()
    {
        objectDistancesArr = new float[initialObjectDistancesArr.Length];
        for (int i = 0; i < objectDistancesArr.Length; i++)
        {
            objectDistancesArr[i] = initialObjectDistancesArr[i] * PlayerController.speedAlteration;
        }
        return objectDistancesArr;
    }
    public bool TrySpawn(GameObjectSpawner gos)
    {
        float start = zLastPosition;
        if (firstObjectSpawned)
            start += StageManager.obstacleSpawnDist;
        else firstObjectSpawned = true;
        int amount;
        float end = spawnEnd;
        if (gos.minAmount != 0) //Spawn Amount randomly
        {
            // Check if maxLength > allowed (overflows)
            amount = 0; // max allowed till next stage + under maxAmount
            float spwnDist = gos.GetObjectDistances()[1];
            Debug.Log("Ministage spawn Dist: " + spwnDist);
            float tmp = start;
            tmp += gos.appendToStart.Count * spwnDist;
            while (tmp < end && amount < gos.staticObjectList.Count)
            {
                tmp += spwnDist;
                amount++;
            }
            if (amount >= gos.minAmount)
            {

            }
            else
            {
                return false;
            }
            amount = Random.Range(gos.minAmount, amount);
        }
        else
        {
            gos.GetObjectDistances();
            float startObjPos = gos.staticObjectList[0].transform.position.z;
            float endObjPos = gos.staticObjectList[gos.staticObjectList.Count - 1].transform.position.z;
            float abs = Mathf.Abs(startObjPos - endObjPos);
            amount = gos.staticObjectList.Count;
            if (start + abs < end)
            {
            }
            else return false;

        }
        SpawnMiniStage(start, 0, amount, gos);
        return true;
    }
    private void SpawnMiniStage(float start, float dist, int amount, GameObjectSpawner gameobjspawner)
    {
        //Debug.Log("Spawning Ministage...");
        gameobjspawner.spwnCount = 0;
        gameobjspawner.SetSpawnProperties(start, dist, 0, amount);
        zLastPosition = gameobjspawner.zLastPosition;
        objectPositions.Add(new Vector3(0,0, zLastPosition));
    }

    public void SpawnObject(int iterations, GameObject forceSpawnGO = null, float zAdd = 0)
    {
        for(int i = 0; i < iterations; i++)
        {
            GameObject currentObject;
            if (forceSpawnGO == null)
            {
                int o = 0;
                if(randomSpawn) o = Random.Range(0, notSpawnedObjects.Count);
                currentObject = notSpawnedObjects[o];
                notSpawnedObjects.RemoveAt(o);
            }
            else currentObject = forceSpawnGO;
            //Debug.Log(currentObject.name);
            currentObject.SetActive(true);
            float xPos = currentObject.transform.position.x;
            float zOffset = 0;
            float zPos;

            if (currentObject.TryGetComponent<SpawnProperties>(out var sp))
            {
                if (xPos != 0)
                    xPos = Random.Range(-sp.xRandomness, sp.xRandomness);
                zOffset = sp.zOffset;
            }

            if (currentObject.TryGetComponent<SlalomObstaclesManager>(out var som))
            {
                som.InstantiateSlalomObstacles();
            }

            if (currentObject.TryGetComponent<GameObjectSpawner>(out var gos))
            {
                if (!TrySpawn(gos))
                {
                    i -= 1;
                    Debug.Log("Skipping OnGroundMiniStage...");
                }
                else currentSpawnedObjects.Add(currentObject);
                continue;
            }

            if (zAdd == 0)
            {
                if (!firstObjectSpawned)
                {
                    zPos = zLastPosition;
                    firstObjectSpawned = true;
                }
                else
                {
                    if (!staticObject)
                    {
                        zPos = zLastPosition + zAddition + zOffset;
                    }
                    else
                    {
                        zPos = zLastPosition + objectDistancesArr[spwnCount - 1];
                    }
                }
            }
            else
            {
                zPos = zAdd + zLastPosition;
            }

            //Debug.Log("CURRENT OBJ: " + currentObject.name);
            currentObject.SetActive(true);

            if (currentObject.TryGetComponent<LavaPlatform>(out var lavaPlatform))
            {
                //int rand = Random.Range(0, 2);
                bool b = false;
                /*if (rand == 1)
                    b = true;
                else b = false;*/
                float rand = Random.value;
                //Debug.Log(rand);
                if (rand < currentRandComp)
                {
                    b = true;
                    currentRandComp = startRandComp;
                }
                else currentRandComp += 0.10f;

                //Debug.Log(b);
                lavaPlatform.InstantiateLavaPlatform(b);
            }


            float yPos = currentObject.transform.position.y;
            if (!yStaticPosition)
            {
                yPos = 0;
                float yOffset = 0;
                if (ySpawnSteps != 0)
                {
                    float maxVal;
                    if (lastYSpawn + yMaxJumpHeightStep < yMaxSpawn)
                        maxVal = lastYSpawn + yMaxJumpHeightStep;
                    else maxVal = yMaxSpawn;
                    float rand = Random.Range(yMinSpawn, maxVal);
                    //float stepSize = (lastYSpawn + yMaxJumpHeightStep) / ySpawnSteps;
                    float numSteps = Mathf.Floor(rand / ySpawnSteps);
                    yOffset = numSteps * ySpawnSteps;

                    /*
                    Debug.Log("Last y spawn: " + lastYSpawn);
                    Debug.Log("Current y spawn: " + (yOffset + yPos));
                    Debug.Log("MaxVal: " + maxVal);
                    Debug.Log("Stepsize: " + ySpawnSteps);
                    Debug.Log("NumSteps: " + numSteps);
                    Debug.Log("Random y value: " + yOffset);
                    */
                }
                yPos += yOffset + objHeight / 2;
                lastYSpawn = yPos;
            }
            if (!GameManager.oC)
            {
                if (currentObject.TryGetComponent<Collider>(out var coll))
                {
                    foreach (Transform t in currentObject.transform)
                    {
                        if (t.TryGetComponent<Collider>(out var coll2))
                            coll2.enabled = false;
                    }
                    coll.enabled = false;
                }
            }

            currentObject.transform.position = new Vector3(xPos, yPos, zPos);
            zLastPosition = zPos;

            objectPositions.Add(currentObject.transform.position);
            currentSpawnedObjects.Add(currentObject);
            spwnCount++;
            //Debug.Log(currentObject.name + " at: " + zPos);

            //Debug.Log(transform.position.z);
            //Debug.Log(StageManager.zPositionLastObject);
        }
    }
}
