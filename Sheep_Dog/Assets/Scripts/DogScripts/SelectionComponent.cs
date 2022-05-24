using UnityEngine;

public class SelectionComponent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!TryGetComponent(out Dog dog)) return; // IF THIS IS NOT DOG, DO NOTHING
        else dog.DogSelected(true); // SET DOG SELECTED BOOL TO TRUE

    }

    private void OnDestroy()
    {
        if (!TryGetComponent(out Dog dog)) return;  // IF THIS IS NOT DOG, DO NOTHING
        else dog.DogSelected(false); // SET DOG SELECTED BOOL TO TRUE
    }
}
