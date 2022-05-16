using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class GlobalSelection : MonoBehaviour
{
    SelectedDictionary _selectedTable;
    RaycastHit _hit;

    Vector3 _p1;

    InputManager _inputManager;

    void Awake() => _inputManager = InputManager.Instance;

    // Start is called before the first frame update
    void Start()
    {
        _selectedTable = GetComponent<SelectedDictionary>();
    }

    public void SelectDogIndex(int index)
    {
        var dog = DogManager.Instance.AllDogs[index];

        if (_selectedTable.SelectedTable.ContainsKey(dog.GetInstanceID())) return;

        _selectedTable.DeselectAll();

        _selectedTable.AddSelected(dog);

    }

    public void SelectDogMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(_inputManager.GetDogMoveRayOrigin());

        if (Physics.Raycast(ray, out _hit, 100.0f, (1 << 10)))
        {
            if (_selectedTable.SelectedTable.ContainsKey(_hit.transform.GetComponent<Dog>().GetInstanceID())) return;

            var dogs = _selectedTable.SelectedTable.Values.ToArray();

            if (dogs.Length > 0) dogs[0].MoveNVAgent(dogs[0].transform.position);

            _selectedTable.DeselectAll();

            _selectedTable.AddSelected(_hit.transform.GetComponentInParent<Dog>());
        }
        else //if we didnt _hit something
        {

            _selectedTable.DeselectAll();

        }
    }

    public void OnEnable()
    {
        _inputManager.OnSelectDogKey += SelectDogIndex;
        _inputManager.OnSelectDogMouse += SelectDogMouse;
    }

    public void OnDisable()
    {
        _inputManager.OnSelectDogKey -= SelectDogIndex;
        _inputManager.OnSelectDogMouse -= SelectDogMouse;
    }


}
