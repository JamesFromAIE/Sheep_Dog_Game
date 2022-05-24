using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public static ObstacleManager Instance; // VARIABLE FOR SINGLETON
    public int ObstacleCount { get; private set; } = 2; // VARIABLE TO REFLECT NUMBER OF OBSTACLES IN SCENE
    int _startingObstacleCount; // NUMBER OF OBSTACLES TO SPAWN

    public List<Transform> ObstaclePrefabs = new List<Transform>(); // ALL OBSTACLE PREFABS
    public List<Transform> AllObstacles = new List<Transform>(); // INITIALISE LIST TO HOLD OBSTACLES IN SCENE
    public List<Transform> AllFenceStates = new List<Transform>(); // INITIALISE LIST TO HOLD ALL FENCE STATES IN SCENE

    void Awake()
    {
        Instance = this; // SET SINGLETON TO THIS SCRIPT
        _startingObstacleCount = ObstacleCount; // SET STARTING COUNT TO OBSTACLE COUNT
    }

    public void SpawnFenceState()
    {
        foreach (var fences in AllFenceStates) // FOREACH FENCE STATE
        {
            fences.gameObject.SetActive(false); // DISABLE FENCE
        }

        int randNum = Random.Range(0, AllFenceStates.Count); // GET RANDOM FENCE NUMBER

        AllFenceStates[randNum].gameObject.SetActive(true); // ENABLE THIS FENCE
    }

    public void SpawnObstacles()
    {
        AllObstacles.Clear(); // CLEAR ALL OBSTACLES FROM LIST

        transform.DeleteChildren(); // DELETE OBSTACLES IN CHILDREN

        var width = DogManager.Instance._gridWidth; // SIMPLIFY WIDTH
        var height = DogManager.Instance._gridHeight; // SIMPLIFY HEIGHT
        var offset = DogManager.Instance.gridOffset; // SIMPLIFY OFFSET

        for (int i = 0; i < ObstacleCount; i++) // FOR EVERY COUNT OF OBSTACLE COUNT
        {
            var prefab = Helper.GetRandomValue(ObstaclePrefabs); // GET RANDOM ROCK PREFAB

            // GET RANDOM POSITION
            var randPos = new Vector3(Random.Range(6, width - 6), prefab.transform.position.y, Random.Range(6, height - 6)) + offset;

            var newObstacle = Instantiate(prefab, randPos, Quaternion.identity, transform); // INSTANTIATE NEW OBSTACLE IN SCENE

            newObstacle.name = "Obstacle " + i; // NAME OBSTACLE
            AllObstacles.Add(newObstacle); // ADD OBSTACLE TO OBSTACLE LIST
        }
    }

    public void IncreaseObstacleCount(int value)
    {
        if (ObstacleCount < 6) // IF CURRENT NUMBER OF OBSTACLES IS LESS THAN 6
            ObstacleCount += value; // INCREASE MAX NUMBER OF OBSTACLES
    }

    public void ResetObstacleCount()
    {
        ObstacleCount = _startingObstacleCount; // RESET OBSTACLE COUNT TO STARTING COUNT
    }

    

}
