using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Alignment")]
public class AlignmentBehaviour : FilteredFlockBehaviour
{
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, FlockManager flock)
    {
        List<Transform> filteredContext = (Filter == null) ? context : Filter.Filter(agent, context); // USE FILTERED LIST IF APPLIED TO THIS
        // IF No neighbours, RETURN no adjustment
        if (filteredContext.Count == 0) return agent.transform.forward;

        // ADD ALL points together and AVERAGE
        Vector3 alignmentMove = Vector3.zero;

        foreach (var item in filteredContext) // FOR EACH NEIGHBOUR...
        {
            alignmentMove += item.transform.forward; // TURN IN NEIGHBOUR'S DIRECTION
        }
        alignmentMove /= context.Count; // FIND AVERAGE OF ALL DIRECTIONS

        return alignmentMove.GroundV3(); // RETURN NEW GROUNDED DIRECTION
    }

}
