using UnityEngine;

public class SettingColliderManager : MonoBehaviour
{
    [SerializeField] SettingPooling settingPoolingcs;

    [SerializeField] BoxCollider floorCollider;
    [SerializeField] BoxCollider ceilingCollider;
    [SerializeField] BoxCollider wallRightCollider;
    [SerializeField] BoxCollider wallLeftCollider;

    // Start is called before the first frame update
    void Start()
    { 
        floorCollider.size = new Vector3(RoomConstants.singleFloorWidth,1f, RoomConstants.singleFloorLength * 3);
        ceilingCollider.size = floorCollider.size;
        wallRightCollider.size = new Vector3(1f, RoomConstants.singleWallHeight, RoomConstants.singleFloorLength * 3);
        wallLeftCollider.size = wallRightCollider.size;
    }

    private void FixedUpdate()
    {
        floorCollider.transform.position = new Vector3(0,RoomConstants.floorPosition.y - 0.5f, settingPoolingcs.poolingList[1].transform.position.z);
        ceilingCollider.transform.position = new Vector3(0, RoomConstants.ceilingPosition.y + 0.5f, settingPoolingcs.poolingList[1].transform.position.z);
        wallRightCollider.transform.position = new Vector3(RoomConstants.rightWallPosition.x + 0.5f, RoomConstants.rightWallPosition.y, settingPoolingcs.poolingList[1].transform.position.z);
        wallLeftCollider.transform.position = new Vector3(RoomConstants.leftWallPosition.x - 0.5f, RoomConstants.leftWallPosition.y, settingPoolingcs.poolingList[1].transform.position.z);
    }
}
