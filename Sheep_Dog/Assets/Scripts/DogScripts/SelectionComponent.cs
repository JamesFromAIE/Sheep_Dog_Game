using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionComponent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!TryGetComponent(out Dog dog)) return;
        else dog.DogSelected(true);

    }

    private void OnDestroy()
    {
        if (!TryGetComponent(out Dog dog)) return;
        else dog.DogSelected(false);
    }
}
