using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public static ObstacleManager Instance;

    public List<Transform> ObstaclePrefabs = new List<Transform>();
    public List<Transform> AllObstacles = new List<Transform>();

    void Awake() => Instance = this;

    void Start()
    {
        SpawnObstacles();
    }

    public void SpawnObstacles()
    {
        AllObstacles.Clear();

        transform.DeleteChildren();

        var width = DogPathfinding.Instance._gridWidth;
        var height = DogPathfinding.Instance._gridHeight;
        var offset = DogPathfinding.Instance.gridOffset;

        for (int i = 0; i < 2; i++)
        {
            var prefab = ObstaclePrefabs[0];

            var randPos = new Vector3(Random.Range(2, width - 2), 0, Random.Range(2, height - 2)) + offset;

            var newObstacle = Instantiate(prefab, randPos, Quaternion.identity, transform);

            newObstacle.name = "Obstacle " + i;
            AllObstacles.Add(newObstacle);
        }
    }
}
