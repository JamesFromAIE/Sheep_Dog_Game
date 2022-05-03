using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Filter/PhysicsLayer")]
public class PhysicsLayerFilter : ContextFilter
{
    public LayerMask _mask;

    public override List<Transform> Filter(FlockAgent agent, List<Transform> original)
    {
        Collider coll = new Collider();
        
        List<Transform> filtered = new List<Transform>();
        foreach (var item in original)
        {
            if (_mask == (_mask | (1 << item.gameObject.layer)))
            {
                filtered.Add(item);
            }

            //if (Physics.Raycast(item.position, item.forward, 3f, _mask))
            //{
            //    filtered.Add(item);
            //}
        }
        return filtered;
    }
}
