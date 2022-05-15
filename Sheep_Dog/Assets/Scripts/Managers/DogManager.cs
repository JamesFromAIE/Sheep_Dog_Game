using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;


public class DogManager : MonoBehaviour
{
    public static DogManager Instance;

    [SerializeField] Dog dogPrefab;

    public LayerMask invalidMasks;

    public List<Dog> AllDogs { get; private set; } = new List<Dog>();
    [SerializeField] AudioClip[] _selectDogClips;
    public bool DogIsMoving;

    void Awake() => Instance = this;

    public void SpawnDogs()
    {
        AllDogs.Clear();

        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out Dog dog)) Destroy(dog.gameObject);
        }

        var obstacles = ObstacleManager.Instance.AllObstacles;

        var width = DogPathfinding.Instance._gridWidth;
        var height = DogPathfinding.Instance._gridHeight;
        var offset = DogPathfinding.Instance.gridOffset;

        for (int i = 0; i < 2; i++)
        {
            var randPos = new Vector3(Random.Range(2, width - 2), 0, Random.Range(2, height - 2)) + offset;

            while (IsDogTooCloseToObstacles(randPos, obstacles, 2.5f))
            {
                randPos = new Vector3(Random.Range(2, width - 2), 0, Random.Range(2, height - 2)) + offset;
            }

            var newDog = Instantiate(dogPrefab, randPos, Quaternion.identity, transform);

            newDog.name = "Dog " + i;
            newDog._selectSound = _selectDogClips[i];
            AllDogs.Add(newDog);
        }
    }

    bool IsDogTooCloseToObstacles(Vector3 point, List<Transform> list, float factor)
    {
        foreach (Transform t in list)
        {
            if (Vector3.Distance(point, t.position) < factor) return true;
        }
        return false;
    }


    void Update()
    {
        if (Input.GetMouseButton(0) && !Helper.isOverUI()) // RECENTLY CHANGED FROM RMB TO LMB FOR MOBILE USE
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100.0f, invalidMasks))
            {
                var dogs = SelectedDictionary.Instance.SelectedTable.Values.ToArray();

                if (dogs.Length > 0) dogs[0].MoveNVAgent(dogs[0].transform.position);

                return;
            }


            if (Physics.Raycast(ray, out hit, 100.0f, (1 << 9)) && 
                SelectedDictionary.Instance.SelectedTable.Count > 0)
            {
                var dogs = SelectedDictionary.Instance.SelectedTable.Values.ToArray();

                for (int i = 0; i < dogs.Length; i++)
                {
                    var dog = dogs[i];

                    if (dog.IsSitting || !dog.IsSelected) continue;

                    dog.TokenSource?.Cancel();

                    //------------------------------------------------------------
                    //NEW PATHFINDING WITH NAVMESH

                    var destination = hit.point;

                    dog.MoveNVAgent(destination);

                        

                    //------------------------------------------------------------



                    /*
                    ------------------------------------------------------------
                    // OLD PATHFINDING WITH A*
                    var pathList = DogPathfinding.Instance.GetVector3Path
                                                            (dog.transform.position - DogPathfinding.Instance.gridOffset, 
                                                             hit.point - DogPathfinding.Instance.gridOffset);

                    if (pathList == null) Debug.LogError("There is NO path in this list");
                    else 
                    {
                        dog.MoveDogToPositionList(pathList);
                    }
                    ------------------------------------------------------------
                    */
                }
            }
        }
        else
        {
            var dogs = SelectedDictionary.Instance.SelectedTable.Values.ToArray();

            if (dogs.Length > 0) dogs[0].MoveNVAgent(dogs[0].transform.position);
        }
    }

    

}

public enum DogStates
{
    Idle = 0,
    Sitting = 1,
    Moving = 2,
}
