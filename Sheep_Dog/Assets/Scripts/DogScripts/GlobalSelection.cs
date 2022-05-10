using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSelection : MonoBehaviour
{
    SelectedDictionary _selectedTable;
    RaycastHit _hit;

    Vector3 _p1;

    // Start is called before the first frame update
    void Start()
    {
        _selectedTable = GetComponent<SelectedDictionary>();
    }

    // Update is called once per frame
    void Update()
    {
        //1. when left mouse button clicked (but not released)
        if (Input.GetMouseButtonDown(0))
        {
            _p1 = Input.mousePosition;
        }

        //2. when mouse button comes up
        if (Input.GetMouseButtonUp(0) && !Helper.isOverUI())
        {
            Ray ray = Camera.main.ScreenPointToRay(_p1);

            if (Physics.Raycast(ray, out _hit, 100.0f, (1 << 10)))
            {
                if (_selectedTable.SelectedTable.ContainsKey(_hit.transform.GetComponent<Dog>().GetInstanceID())) return;

                _selectedTable.DeselectAll();

                _selectedTable.AddSelected(_hit.transform.GetComponentInParent<Dog>());
            }
            else //if we didnt _hit something
            {

                _selectedTable.DeselectAll();
                
            }

        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectDogIndex(0);

        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectDogIndex(1);

    }

    public void SelectDogIndex(int index)
    {
        var dog = DogManager.Instance.AllDogs[index];

        if (_selectedTable.SelectedTable.ContainsKey(dog.GetInstanceID())) return;

        _selectedTable.DeselectAll();

        _selectedTable.AddSelected(dog);

    }

}
