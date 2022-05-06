using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;


public class DogManager : MonoBehaviour
{
    public static DogManager Instance;

    public List<Dog> _allDogs;

    void Awake() => Instance = this;


    void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100.0f, (1 << 9)) && SelectedDictionary.Instance.SelectedTable.Count > 0)
            {
                var dogs = SelectedDictionary.Instance.SelectedTable.Values.ToArray();

                for (int i = 0; i < dogs.Length; i++)
                {
                    var dog = dogs[i];

                    if (dog.IsSitting) continue;

                    dog.TokenSource?.Cancel();

                    var pathList = DogPathfinding.Instance.GetVector3Path
                                                            (dog.transform.position - DogPathfinding.Instance.gridOffset, 
                                                            hit.point - DogPathfinding.Instance.gridOffset, 
                                                            new List<Vector3>());

                    if (pathList == null) Debug.LogError("There is NO path in this list");
                    else
                    {
                        dog.MoveDogToPositionList(pathList);
                    }
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
