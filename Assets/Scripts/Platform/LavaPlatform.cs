using UnityEngine;

public class LavaPlatform : MonoBehaviour
{
    private float moveSpeed = 0.1f;
    public bool isMoving;
    private int xMoveDir;
    public float xExtent;
    public float yExtent;
    public float zExtent;
    // Start is called before the first frame update
    void Awake()
    {
        //rb = pc.GetComponent<Rigidbody>();
        Bounds b = GetComponent<Renderer>().bounds;
        xExtent = b.extents.x;
        yExtent = b.extents.y;
        zExtent = b.extents.z;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //TODO make some Platforms Move in x Direction
        if (isMoving)
        {
            //Debug.Log("Lava Platform moving");
            transform.Translate(xMoveDir * moveSpeed,0,0);
            if ((transform.position.x + xExtent) >= (RoomConstants.singleFloorWidth / 2))
            {
                //Debug.Log("Minus Direction");
                xMoveDir = -1;
            }
            if ((transform.position.x - xExtent) <= (-RoomConstants.singleFloorWidth / 2))
            {
                //Debug.Log("Plus Direction");
                xMoveDir = 1;
            }
        }
    }
    public void InstantiateLavaPlatform(bool move)
    {
        isMoving = move;
        int rand = Random.Range(0, 2);
        if (rand == 0)
            xMoveDir = -1;
        else
            xMoveDir = 1;
    }
}
