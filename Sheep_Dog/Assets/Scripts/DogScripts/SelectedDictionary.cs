using System.Collections.Generic;
using UnityEngine;

public class SelectedDictionary : MonoBehaviour
{
    public static SelectedDictionary Instance; // SINGLETON VARIABLE
    
    void Awake() => Instance = this; // SET SINGLETON TO THIS SCRIPT

    public Dictionary<int, Dog> SelectedTable = new Dictionary<int, Dog>(); // INITIALISE NEW DICTIONARY

    public void AddSelected(Dog dog) // ADD DOG TO DICTIONARY
    {
        int id = dog.GetInstanceID(); // GET DOG INSTANCE ID

        if (!(SelectedTable.ContainsKey(id))) // IF DICTIONARY DOESNT CONTAIN THIS KEY...
        {
            SelectedTable.Add(id, dog); // ADD INSTANCE ID AND DOG TO DICTIONARY
            dog.gameObject.AddComponent<SelectionComponent>(); // ADD SELECT COMPONENT ONTO DOG
        }
    }

    public void Deselect(int id) // REMOVE DOG FROM DICTIONARY
    {
        Destroy(SelectedTable[id].GetComponent<SelectionComponent>()); // REMOVE SELECT COMPONENT FROM DOG
        SelectedTable.Remove(id); // REMOVE DOG FRO DICTIONARY
    }

    public void DeselectAll() // REMOVE ALL DOGS FROM DICTIONARY
    {
        foreach(KeyValuePair<int,Dog> pair in SelectedTable) // FOR EVERY PAIR IN THE DICTIONARY
        {
            if(pair.Value != null) // IF DOG IS NOT NULL...
            {
                Destroy(SelectedTable[pair.Key].GetComponent<SelectionComponent>()); // REMOVE SELECT COMPONENT FROM DOG
            }
        }
        SelectedTable.Clear(); // CLEAR DICTIONARY OF ALL PAIRS
    }
}
