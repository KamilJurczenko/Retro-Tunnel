using System.Collections.Generic;
using UnityEngine;

// Used when last Obstacle not from Stage 3 in Stage 4
public class BoostSpawner : MonoBehaviour
{
    [SerializeField] int poolAmount;

    List<GameObject> spawnBoostPool = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        string path = "Prefabs/Interactable_Objects/PlayerBoostObject";
        GameObject boostGameObject = Resources.Load(path) as GameObject;
        try
        {
            for (int i = 0; i < poolAmount; i++)
            {
                GameObject go = Instantiate(boostGameObject);
                //Debug.Log("Instance ID: " + go.GetInstanceID());
                go.SetActive(false);
                spawnBoostPool.Add(go);
            }
        }
        catch
        {
            Debug.LogError("Error with Instantiating Player Boost Object!");
        }
    }
}
