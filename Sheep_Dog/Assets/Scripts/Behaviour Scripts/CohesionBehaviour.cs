using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Cohesion")]
public class CohesionBehaviour : FilteredFlockBehaviour
{
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        // IF No neighbours, RETURN no adjustment
        if (context.Count == 0) return Vector3.zero;

        // ADD ALL points together and AVERAGE
        Vector3 cohesionMove = Vector3.zero;
        List<Transform> filteredContext = (Filter == null) ? context : Filter.Filter(agent, context); // USE FILTERED LIST IF APPLIED TO THIS
        foreach (var item in filteredContext)
        {
            cohesionMove += item.position;
        }
        cohesionMove /= context.Count;

        //CREATE OFFSET from agent position
        cohesionMove -= agent.transform.position;

        return cohesionMove.GroundV3();
    }

}
