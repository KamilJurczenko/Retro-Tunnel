using System.Collections.Generic;
using UnityEngine;

public class SettingPooling : MonoBehaviour
{
    [SerializeField] Transform lengthReference;

    public List<Transform> poolingList = new List<Transform>();

    private float extent;

    private float zStart;

    [SerializeField]private bool active;

    private void Start()
    {
        //active = false;
        zStart = -50000;
        extent = lengthReference.GetComponent<Renderer>().bounds.extents.z;
    }

    //private bool stopSpawning;
    void FixedUpdate()
    {
        if ((CameraController.cameraPosition.z < (poolingList[0].position.z - extent)) && CameraController.cameraPosition.z > zStart
            || (CameraController.cameraPosition.z > (extent + poolingList[0].position.z)) 
            && active)
        {
            Transform tempT = null;
            float x = poolingList[0].position.x;
            float y = poolingList[0].position.y;
            float z = poolingList[0].position.z;

            if (CameraController.cameraPosition.z >= (extent + poolingList[0].position.z))
            {
                //Debug.Log("Platform step forward");
                z = poolingList[poolingList.Count - 1].position.z + (extent * 2);
                tempT = poolingList[0];
                poolingList.RemoveAt(0);
                poolingList.Add(tempT);
            }
            else if (CameraController.cameraPosition.z < (poolingList[0].position.z - extent))
            {
                Debug.Log("Platform step back");
                tempT = poolingList[2];
                poolingList.RemoveAt(2);
                z = poolingList[0].position.z - (extent * 2);
                poolingList.Insert(0, tempT);
            }
            //Debug.Log(z);
            tempT.position = new Vector3(x, y, z);
        }
    }
    public void SetPooling(float start, bool a)
    {
        zStart = start;
        poolingList[0].position = new Vector3(poolingList[0].position.x, poolingList[0].position.y, start + extent);
        poolingList[1].position = new Vector3(poolingList[1].position.x, poolingList[1].position.y, start + extent * 3);
        poolingList[2].position = new Vector3(poolingList[2].position.x, poolingList[2].position.y, start + extent * 5);
        active = a;
    }
}
