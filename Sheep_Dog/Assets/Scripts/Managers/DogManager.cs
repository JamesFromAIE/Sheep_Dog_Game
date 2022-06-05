using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DogManager : MonoBehaviour
{
    public static DogManager Instance; // VARIABLE FOR SINGLETON
    InputManager _inputManager; // VARIABLE FOR INPUT MANAGER

    public int _gridWidth; // APPROX FIELD WIDTH 
    public int _gridHeight; // APPROX FIELD HEIGHT
    public Vector3 gridOffset; // APPROX FIELD OFFSET FROM VECTOR3.ZERO

    [SerializeField] Dog _dogPrefab; // PREFAB OF DOG TO SPAWN

    public LayerMask _invalidMasks; // MASKS WHICH CANNOT BE HIT WHEN GETTING NEW DOG DESTINATION

    public List<Dog> AllDogs { get; private set; } = new List<Dog>(); // INITIALISE NEW LIST OF ALL DOGS
    [SerializeField] AudioClip[] _selectDogClips; // BARK SOUNDS FOR DOG SELECTION
    public bool DogIsMoving = false; // BOOL TO CONTROL WHEN SELECTED DOG SHOULD AND SHOULDN'T MOVE DEPENDING ON INPUT FROM INPUT MANAGER

    void Awake()
    {
        Instance = this; // SET SINGLETON TO THIS SCRIPT
        _inputManager = InputManager.Instance; // SET REFERENCE TO INPUT MANAGER

    }

    private void OnEnable()
    {
        _inputManager.OnDogIsMoving += SetIsDogMoving; // SUBSCRIBE FUNCTION TO EVENT FROM INPUT MANAGER
    }

    private void OnDisable()
    {
        _inputManager.OnDogIsMoving -= SetIsDogMoving; // UNSUBSCRIBE FUNCTION FROM EVENT FROM INPUT MANAGER
    }

    void SetIsDogMoving(bool condition)
    {
        DogIsMoving = condition; // SET CONDITION TO WHETHER DOG SHOULD BE MOVING OR NOT
    }

    public void SpawnDogs()
    {
        AllDogs.Clear(); // CLEAR ALL DOGS FROM LIST

        transform.DeleteChildren();

        var obstacles = ObstacleManager.Instance.AllObstacles; // GET ALL OBSTACLES FROM OBSTACLE MANAGER

        for (int i = 0; i < 2; i++) // ITERATE OVER THIS 'FOR' LOOP TWO TIMES
        {
            var plane = Helper.GetRandomValue(ObstacleManager.Instance.WalkablePlanes);

            var width = plane.transform.localScale.x * 10;
            var length = plane.transform.localScale.z * 10;
            var offset = plane.transform.position - new Vector3(width / 2, 0, length / 2);

            var randPos = new Vector3(Random.Range(width / 4, width - width / 4), 0, Random.Range(length / 4, length - length / 4)) + offset; // GET RANDOM POSITION TO SPAWN

            while (IsDogTooCloseToObstacles(randPos, obstacles, 2.5f)) // WHILE POSITION IS TOO CLOSE TO OBSTACLES (INSIDE OF THEM)
            {
                plane = Helper.GetRandomValue(ObstacleManager.Instance.WalkablePlanes);

                width = plane.transform.localScale.x * 10;
                length = plane.transform.localScale.z * 10;
                offset = plane.transform.position - new Vector3(width / 2, 0, length / 2);

                randPos = new Vector3(Random.Range(width / 4, width - width / 4), 0, Random.Range(length / 4, length - length / 4)) + offset; // GET RANDOM POSITION TO SPAWN
            }

            var newDog = Instantiate(_dogPrefab, randPos, Quaternion.identity, transform); // INSTANTIATE NEW DOG INTO SCENE

            newDog.name = "Dog " + i; // NAME DOG
            newDog._selectSound = _selectDogClips[i]; // SET ITS SELECT SOUND TO BARK FROM ARRAY
            AllDogs.Add(newDog); // ADD DOG TO DOG LIST
        }
    }

    public void DisableDogs()
    {
        var dogs = SelectedDictionary.Instance.SelectedTable.Values.ToArray(); // GET ALL SELECTED DOGS INTO AN ARRAY

        for (int i = 0; i < dogs.Length;i++) dogs[i].enabled = false; // DISABLE ALL DOGS 
    }

    public void EnableDogs()
    {
        var dogs = SelectedDictionary.Instance.SelectedTable.Values.ToArray(); // GET ALL SELECTED DOGS INTO AN ARRAY

        for (int i = 0; i < dogs.Length; i++) dogs[i].enabled = true; // ENABLE ALL DOGS 
    }


    bool IsDogTooCloseToObstacles(Vector3 point, List<Transform> list, float factor)
    {
        foreach (Transform t in list) // FOR EACH TRANSFORM IN LIST...
        {
            if (Vector3.Distance(point, t.position) < factor) return true; // IF THIS OBSTACLE IS TOO CLOSE TO POINT, RETURN TRUE
        }
        return false; // BECAUSE NO OBSTACLES WERE TOO CLOSE TO POINT, RETURN FALSE
    }


    void Update()
    {
        GetAndSetDogDestinationNew(); // GET AND SET DOGS DESTINATION ON UPDATE 

    }

    void GetAndSetDogDestinationNew()
    {
        Ray ray = Camera.main.ScreenPointToRay(_inputManager.GetDogMoveRayOrigin()); // CAST RAY FROM SCREEN ONTO MAP
        RaycastHit hit; // VARIABLE TO CONTAIN HIT OBJECT

        if (Physics.Raycast(ray, out hit, 100.0f, _invalidMasks) || !DogIsMoving) // IF HIT INVALID OBJECT OR DOG SHOULND'T BE MOVING
        {
            var dogs = SelectedDictionary.Instance.SelectedTable.Values.ToArray(); // PUT ALL SELECTED DOGS INTO AN ARRAY

            if (dogs.Length > 0) dogs[0].MoveNVAgent(dogs[0].transform.position); // IF THERE IS MORE THAN ONE DOG, STOP IT MOVING

            return; // DO NOTHING ELSE
        }
        
       if (SelectedDictionary.Instance.SelectedTable.Count > 0) // IF AT LEAST ONE DOG IS SELECTED
       {
            if (Physics.Raycast(ray, out hit, 100.0f, (1 << 9))) // IF YOU HIT PATHFINDING PLANE LAYER...
            {
                var dogs = SelectedDictionary.Instance.SelectedTable.Values.ToArray(); // PUT ALL SELECTED DOGS INTO AN ARRAY

                for (int i = 0; i < dogs.Length; i++) // FOR EVERY SELECTED DOG
                {
                    var dog = dogs[i]; // SIMPLIFY DOG

                    if (dog.IsSitting || !dog.IsSelected) continue; // IF DOG IS SITTING OR ISN'T SELECTED, DO NOTHING

                    var destination = hit.point; // GET HIT POINT FROM RAYCAST

                    dog.MoveNVAgent(destination); // SET DOG TO MOVE TOWARD THIS DESTINATION

                }
            }
            else if (Physics.Raycast(ray, out hit, 100.0f, (1 << 12)) || Physics.Raycast(ray, out hit, 100.0f, (1 << 6))) // IF YOU HIT BOUNDARY LAYER...
            {
                var dogs = SelectedDictionary.Instance.SelectedTable.Values.ToArray(); // PUT ALL SELECTED DOGS INTO AN ARRAY

                for (int i = 0; i < dogs.Length; i++) // FOR EVERY SELECTED DOG
                {
                    var dog = dogs[i]; // SIMPLIFY DOG

                    if (dog.IsSitting || !dog.IsSelected) continue; // IF DOG IS SITTING OR ISN'T SELECTED, DO NOTHING

                    var hitPos = hit.point; // GET HIT POINT FROM RAYCAST

                    Vector3 destination;
                    ObstacleManager.Instance.GetClosestWalkablePlane(hitPos, out destination); // GET POINT ON PATHFINDING PLANE CLOSEST TO HIT POINT

                    dog.MoveNVAgent(destination); // SET DOG TO MOVE TOWARD THIS DESTINATION

                }
            }

       }
    }
}

public enum DogStates
{
    Idle = 0,
    Sitting = 1,
    Moving = 2,
}
