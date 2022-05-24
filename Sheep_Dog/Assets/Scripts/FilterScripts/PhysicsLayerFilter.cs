using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Filter/PhysicsLayer")]
public class PhysicsLayerFilter : ContextFilter
{
    public LayerMask _mask; // LAYER TO DETECT FOR

    public override List<Transform> Filter(FlockAgent agent, List<Transform> original)
    {
        List<Transform> filtered = new List<Transform>(); // INITIALISE NEW LIST
        foreach (var item in original) // FOREACH NEIGHBOURING OBJECT 
        {
            if (_mask == (_mask | (1 << item.gameObject.layer))) // IF THIS OBJECT HAS THIS LAYER...
            {
                filtered.Add(item); // ADD ITEM TO NEW LIST
            }
        }
        return filtered; // RETURN NEW LIST
    }
}
