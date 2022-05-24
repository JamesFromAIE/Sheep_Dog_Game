using UnityEngine;
using System.Linq;

public class GlobalSelection : MonoBehaviour
{
    SelectedDictionary _selectedTable; // REFERENCE FOR SELECT SCRIPT
    RaycastHit _hit; // VARIABLE FOR HIT OBJECT

    InputManager _inputManager; // SINGLETON VARIABLES

    void Awake() => _inputManager = InputManager.Instance; // SET SINGLETON TO THIS SCRIPT

    void Start()
    {
        _selectedTable = GetComponent<SelectedDictionary>(); // GET SELECT SCRIPT
    }

    public void SelectDogMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(_inputManager.GetDogMoveRayOrigin()); // CAST RAY FROM SCREEN ONTO MAP

        if (Physics.Raycast(ray, out _hit, 100.0f, (1 << 10))) // IF DOG SELECT COLLIDER IS HIT...
        {
            // DO NOTHING IF YOU ALREADY HAVE DOG SELECTED
            if (_selectedTable.SelectedTable.ContainsKey(_hit.transform.GetComponent<Dog>().GetInstanceID())) return; 

            // GET ARRAY OF SELECTED DOGS
            var dogs = _selectedTable.SelectedTable.Values.ToArray();

            // IF THERE IS MORE THAN ONE DOG, STOP MOVING PREVIOUS DOG
            if (dogs.Length > 0) dogs[0].MoveNVAgent(dogs[0].transform.position);

            _selectedTable.DeselectAll(); // DESELECT ALL DOGS

            _selectedTable.AddSelected(_hit.transform.GetComponentInParent<Dog>()); // ADD DOG TO SELECTED DICTIONARY
        }
    }

    public void OnEnable()
    {
        _inputManager.OnSelectDogMouse += SelectDogMouse; // SUBSCRIBE FUNCTION TO EVENT FROM INPUT MANAGER
    }

    public void OnDisable()
    {
        _inputManager.OnSelectDogMouse -= SelectDogMouse; // UNSUBSCRIBE FUNCTION FROM EVENT FROM INPUT MANAGER
    }


}
