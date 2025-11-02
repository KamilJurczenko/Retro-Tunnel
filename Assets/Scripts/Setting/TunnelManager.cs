using UnityEngine;

public class TunnelManager : MonoBehaviour
{
    [SerializeField] Renderer rend;
    [SerializeField] Transform main;

    private float zExtent;

    private static Vector3 currentStartPos;
    private static Vector3 currentEndPos;
    private static Vector3 nextStartPos;
    private static Vector3 nextEndPos;

    private static Vector3[] currentPositions;
    private static Vector3[] nextPositions;

    public static bool tunnelEnterTrigger;
    public static bool tunnelExitTrigger;

    public static float startPos;

    private float spawnDist;
    private void Awake()
    {
        startPos = 0;
        currentPositions = new Vector3[2];
        nextPositions = new Vector3[2];
    }
    // Start is called before the first frame update
    void Start()
    {
        spawnDist = StageManager.currentNextStagePoint / PlayerController.playerMetersFactor;
        zExtent = rend.bounds.extents.z;
        if(PlayerVariablesSave.tutorialPlayed)
            SetTunnelPosition(spawnDist);
    }
    public void SetTunnelPosition(float pos)
    {
        transform.position = Vector3.forward * (pos + startPos);

        tunnelEnterTrigger = false;
        tunnelExitTrigger = false;

        currentStartPos = main.position - Vector3.forward * zExtent;
        currentEndPos = main.position + Vector3.forward * zExtent;
        nextStartPos = currentStartPos + Vector3.forward * spawnDist;
        nextEndPos = currentEndPos + Vector3.forward * spawnDist;

        currentPositions[0] = currentStartPos;
        currentPositions[1] = currentEndPos;

        nextPositions[0] = nextStartPos;
        nextPositions[1] = nextEndPos;
        Debug.Log("Setting Tunnel Position at: " + pos);
    }
    public static Vector3[] GetCurrentPositions()
    {
        return currentPositions;
    }
    public static Vector3[] GetNextPositions()
    {
        return nextPositions;
    }
}
