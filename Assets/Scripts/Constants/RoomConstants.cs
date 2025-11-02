using UnityEngine;

public class RoomConstants : MonoBehaviour
{
    [SerializeField] Transform floorTransform;
    [SerializeField] Transform wallRightTransform;
    [SerializeField] Transform wallLeftTransform;
    [SerializeField] Transform ceilingTransform;

    public static float singleFloorLength;
    public static float singleFloorWidth;
    public static float singleWallHeight;
    public static Vector2 rightWallPosition;
    public static Vector2 leftWallPosition;
    public static Vector2 ceilingPosition;
    public static Vector2 floorPosition;

    void Awake()
    {
        Vector3 floorBounds = floorTransform.GetComponent<MeshRenderer>().bounds.extents;
        singleFloorWidth = floorBounds.x * 2;
        singleFloorLength = floorBounds.z * 2;
        Vector3 wallBounds = wallRightTransform.GetComponent<MeshRenderer>().bounds.extents;
        singleWallHeight = wallBounds.y * 2;
        rightWallPosition = wallRightTransform.position;
        leftWallPosition = wallLeftTransform.position;
        ceilingPosition = ceilingTransform.position;
        floorPosition = floorTransform.position;
    }
}
