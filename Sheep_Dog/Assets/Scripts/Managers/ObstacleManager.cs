using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public static ObstacleManager Instance;
    public int ObstacleCount { get; private set; } = 2;
    int _startingObstacleCount;

    public List<Transform> ObstaclePrefabs = new List<Transform>();
    public List<Transform> AllObstacles = new List<Transform>();
    public List<Transform> AllFenceStates = new List<Transform>();

    void Awake()
    {
        Instance = this;
        _startingObstacleCount = ObstacleCount;
    }

    public void SpawnFenceState()
    {
        foreach (var fences in AllFenceStates)
        {
            fences.gameObject.SetActive(false);
        }

        int randNum = Random.Range(0, AllFenceStates.Count);

        AllFenceStates[randNum].gameObject.SetActive(true);
    }

    public void SpawnObstacles()
    {
        AllObstacles.Clear();

        transform.DeleteChildren();

        var width = DogPathfinding.Instance._gridWidth;
        var height = DogPathfinding.Instance._gridHeight;
        var offset = DogPathfinding.Instance.gridOffset;

        for (int i = 0; i < ObstacleCount; i++)
        {
            var prefab = Helper.GetRandomValue(ObstaclePrefabs);

            var randPos = new Vector3(Random.Range(2, width - 2), 0, Random.Range(2, height - 2)) + offset;

            var newObstacle = Instantiate(prefab, randPos, Quaternion.identity, transform);

            newObstacle.name = "Obstacle " + i;
            AllObstacles.Add(newObstacle);
        }
    }

    public void IncreaseObstacleCount(int value)
    {
        if (ObstacleCount < 6)
            ObstacleCount += value;
    }

    public void ResetObstacleCount()
    {
        ObstacleCount = _startingObstacleCount;
    }

    

}
