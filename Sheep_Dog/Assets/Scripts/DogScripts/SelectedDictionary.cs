using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedDictionary : MonoBehaviour
{
    public static SelectedDictionary Instance;
    void Awake() => Instance = this;


    public Dictionary<int, Dog> SelectedTable = new Dictionary<int, Dog>();

    public void AddSelected(Dog dog)
    {
        int id = dog.GetInstanceID();

        if (!(SelectedTable.ContainsKey(id)))
        {
            SelectedTable.Add(id, dog);
            dog.gameObject.AddComponent<SelectionComponent>();
        }
    }

    public void Deselect(int id)
    {
        Destroy(SelectedTable[id].GetComponent<SelectionComponent>());
        SelectedTable.Remove(id);
    }

    public void DeselectAll()
    {
        foreach(KeyValuePair<int,Dog> pair in SelectedTable)
        {
            if(pair.Value != null)
            {
                Destroy(SelectedTable[pair.Key].GetComponent<SelectionComponent>());
            }
        }
        SelectedTable.Clear();
    }
}
