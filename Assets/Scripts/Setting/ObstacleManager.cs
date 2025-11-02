using UnityEngine;

// Spawns Obstacles
public class ObstacleManager : MonoBehaviour
{
    [SerializeField] GameObjectSpawner gameObjectSpawnerStage1;
    [SerializeField] GameObjectSpawner gameObjectSpawnerStage2;
    [SerializeField] GameObjectSpawner gameObjectSpawnerStage3;
    [SerializeField] GameObjectSpawner gameObjectSpawnerStage4;

    public int preSpawnedObstacles = 3;

    public void InstantiateObstacleStage(float start, float zAdd, float end, int obstacleStage)
    {
        GameObjectSpawner tmp;
        if (obstacleStage == 1)
            tmp = gameObjectSpawnerStage1;
        else if (obstacleStage == 2)
            tmp = gameObjectSpawnerStage2;
        else if (obstacleStage == 3)
            tmp = gameObjectSpawnerStage3;
        else if (obstacleStage == 4)
            tmp = gameObjectSpawnerStage4;
        else
        {
            Debug.LogError("obstacleStage Value invalid");
            return;
        }
        Debug.Log("Obstacle Stage Set");
        tmp.SetSpawnProperties(start, zAdd, end, preSpawnedObstacles);
    }
}
