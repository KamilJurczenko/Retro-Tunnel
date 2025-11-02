using System.Collections.Generic;
using UnityEngine;

public class SlalomObstaclesManager : MonoBehaviour
{
    [SerializeField] List<GameObject> obstacles;
    [SerializeField] GameObjectSpawner gameObjectSpawner;

    private List<GameObject> slalomOrder;

    public void InstantiateSlalomObstacles() 
    {
        Debug.Log("Slalom Stage");
        slalomOrder = new List<GameObject>(obstacles);
        float randVal = Random.value;

        /*int rand = Random.Range(2,obstacles.Count + 1);
        if(rand != obstacles.Count)
        {
            // Cut List
            slalomOrder.RemoveRange(rand, obstacles.Count-rand);
        }
        */

        if(randVal < 0.5f)
        {
            
        }
        else
        {
            GameObject tmp = slalomOrder[0];
            slalomOrder.RemoveAt(0);
            slalomOrder.Add(tmp);
        }
        gameObjectSpawner.staticObjectList = new List<GameObject>(slalomOrder);
    }
}
