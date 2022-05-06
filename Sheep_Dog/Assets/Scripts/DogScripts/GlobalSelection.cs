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
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(_p1);

            if (Physics.Raycast(ray, out _hit, 100.0f, (1 << 10)))
            {
                if (Input.GetKey(KeyCode.LeftShift)) //inclusive select
                {
                    _selectedTable.AddSelected(_hit.transform.GetComponentInParent<Dog>());
                }
                else //exclusive selected
                {
                    _selectedTable.DeselectAll();
                    _selectedTable.AddSelected(_hit.transform.GetComponentInParent<Dog>());
                }
            }
            else //if we didnt _hit something
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    //do nothing
                }
                else
                {
                    _selectedTable.DeselectAll();
                }
            }

        }
       
    }

}
