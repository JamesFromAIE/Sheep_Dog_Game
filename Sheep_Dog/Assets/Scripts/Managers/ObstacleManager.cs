using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public static ObstacleManager Instance; // VARIABLE FOR SINGLETON
    public int ObstacleCount { get; private set; } = 2; // VARIABLE TO REFLECT NUMBER OF OBSTACLES IN SCENE
    int _startingObstacleCount; // NUMBER OF OBSTACLES TO SPAWN

    public List<Transform> ObstaclePrefabs = new List<Transform>(); // ALL OBSTACLE PREFABS
    public List<Transform> AllObstacles { get; private set; } = new List<Transform>(); // INITIALISE LIST TO HOLD OBSTACLES IN SCENE

    [SerializeField] ScriptableLevel[] _levelArray;
    [SerializeField] int[] _levelWeight;
    public ScriptableLevel Level { get; private set; }
    public GameObject Fence { get; private set; }
    public Transform MidPoint { get; private set; }
    public List<MeshCollider> WalkablePlanes { get; private set; } = new List<MeshCollider>();

    void Awake()
    {
        Instance = this; // SET SINGLETON TO THIS SCRIPT
        _startingObstacleCount = ObstacleCount; // SET STARTING COUNT TO OBSTACLE COUNT
    }

    public void GetAndSetFirstLevelLayout()
    {
        Level = _levelArray[0];
    }

    public void GetNewLevelLayout()
    {
        if (_levelArray.Length != _levelWeight.Length)
        {
            Debug.LogError("Mismatch between Levels and Weights", this);
            return;
        }

        List<ScriptableLevel> randList = new List<ScriptableLevel>();

        for (int i = 0; i < _levelArray.Length; i++)
        {
            int weight = _levelWeight[i];
            ScriptableLevel level = _levelArray[i];

            for (int j = 0; j < weight; j++)
            {
                randList.Add(level);
            }
        }

        if (randList.Count == 0)
        {
            Debug.LogError("There are no Levels to GET", this);
            return;
        }

        ScriptableLevel[] weightedArray = randList.ToArray();

        int randIndex = Random.Range(0, weightedArray.Length);

        Level = weightedArray[randIndex];

    }

    public void SpawnLevel()
    {
        ClearOldLevel();

        Fence = Instantiate(Level.Fences, Level.Fences.transform.position, Level.Fences.transform.rotation, transform);
        MidPoint = Instantiate(Level.MidPoint, Level.MidPoint.transform.position, Level.MidPoint.transform.rotation, transform);

        foreach (var plane in Level.WalkablePlanes)
        {
            WalkablePlanes.Add(Instantiate(plane, plane.transform.position, plane.transform.rotation, transform));
        }
    }

    private void ClearOldLevel()
    {
        AllObstacles?.Clear(); // CLEAR ALL OBSTACLES FROM LIST
        Fence = null;
        MidPoint = null;
        WalkablePlanes?.Clear();

        transform.DeleteChildren(); // DELETE OBSTACLES IN CHILDREN
    }

    private void ClearObstacles()
    {
        foreach(var ob in AllObstacles)
        {
            Destroy(ob.gameObject);
        }

        AllObstacles?.Clear(); // CLEAR ALL OBSTACLES FROM LIST
    }

    public void SpawnObstacles()
    {
        for (int i = 0; i < ObstacleCount; i++) // FOR EVERY COUNT OF OBSTACLE COUNT
        {
            var prefab = Helper.GetRandomValue(ObstaclePrefabs); // GET RANDOM ROCK PREFAB
            var plane = GetSpawnablePlanes(WalkablePlanes); // GET RANDOM PLANE
            var randRot = Quaternion.Euler(0, Random.Range(0, 8) * 45, 0);

            var randPos = RandomPositionInPlane(plane); // GET RANDOM POSITION

            int iterations = 0;

            while (!randPos.IsPointSpawnable(plane.bounds) || IsObstacleTooCloseToObstacles(randPos, AllObstacles, 3f)) // WHILE SPAWN POSIITON IS OUTSIDE OF SPAWNING BOUNDS...
            {
                plane = GetSpawnablePlanes(WalkablePlanes); // GET NEW RANDOM PLANE
                randPos = RandomPositionInPlane(plane); // GET NEW RANDOM POSITION
                iterations++;

                if (iterations >= 100)
                {
                    ClearObstacles();
                    iterations = 0;
                }
            }

            var newObstacle = Instantiate(prefab, randPos + prefab.transform.position, randRot, transform); // INSTANTIATE NEW OBSTACLE IN SCENE
            
            newObstacle.name = "Obstacle " + i; // NAME OBSTACLE
            AllObstacles.Add(newObstacle); // ADD OBSTACLE TO OBSTACLE LIST

        }
    }

    MeshCollider GetSpawnablePlanes(List<MeshCollider> planes)
    {
        List<MeshCollider> newList = new List<MeshCollider>();

        foreach (var plane in planes) if (plane.isTrigger == true) newList.Add(plane);

        int index = Random.Range(0, newList.Count);

        return newList[index];


    }

    bool IsObstacleTooCloseToObstacles(Vector3 point, List<Transform> list, float factor)
    {
        foreach (Transform t in list) // FOR EACH TRANSFORM IN LIST...
        {
            if (Vector3.Distance(point, t.position) < factor) return true; // IF THIS OBSTACLE IS TOO CLOSE TO POINT, RETURN TRUE
        }
        return false; // BECAUSE NO OBSTACLES WERE TOO CLOSE TO POINT, RETURN FALSE
    }

    Vector3 RandomPositionInPlane(MeshCollider coll)
    {
        var width = coll.transform.localScale.x * 10;
        var length = coll.transform.localScale.z * 10;
        var offset = coll.transform.position - new Vector3(width / 2, 0, length / 2);

        // GET RANDOM POSITION
        return new Vector3(Random.Range(width / 3, width - width / 3),
                           0,
                           Random.Range(length / 3, length - length / 3)) + offset;
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

    public MeshCollider GetClosestWalkablePlane(Vector3 origPos)
    {
        MeshCollider closestMesh = null;
        float closestDistance = 0;
        int iterations = 0;

        if (WalkablePlanes.Count == 1) return WalkablePlanes[0];

        foreach(var plane in WalkablePlanes)
        {
            if (iterations == 0)
            {
                closestMesh = plane;
                closestDistance = Vector3.Distance(origPos, closestMesh.ClosestPoint(origPos));
                continue;
            }

            float newDistance = Vector3.Distance(origPos, plane.ClosestPoint(origPos));
            if (newDistance < closestDistance)
            {
                closestMesh = plane;
                closestDistance = newDistance;
            }

            iterations++;
        }

        return closestMesh;
    }

    public MeshCollider GetClosestWalkablePlane(Vector3 origPos, out Vector3 destination)
    {
        float closestDistance = 0;

        MeshCollider closestMesh = null;
        int iterations = 0;

        if (WalkablePlanes.Count == 1)
        {
            destination = WalkablePlanes[0].ClosestPoint(origPos);
            return WalkablePlanes[0];
        }

        foreach (var plane in WalkablePlanes)
        {
            if (!plane.convex) continue;

            if (iterations == 0)
            {
                closestMesh = plane;
                closestDistance = Vector3.Distance(origPos, closestMesh.ClosestPoint(origPos));
                iterations++;
                continue;
            }

            float newDistance = Vector3.Distance(origPos, plane.ClosestPoint(origPos));
            if (newDistance < closestDistance)
            {
                closestMesh = plane;
                closestDistance = newDistance;
            }

            iterations++;
        }

        destination = closestMesh.ClosestPoint(origPos);
        return closestMesh;
    }



}
