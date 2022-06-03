using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/SteeredCohesion")]
public class SteeredCohesionBehaviour : FilteredFlockBehaviour
{
    Vector3 _currentVelocity; // REFERENCED VARIABLE OF AGENT VELOCITY
    float3 _currentSpeed;
    public float AgentSmoothTime = 0.5f;

    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, FlockManager flock)
    {
        List<Transform> filteredContext = (Filter == null) ? context : Filter.Filter(agent, context); // USE FILTERED LIST IF APPLIED TO THIS
        // IF No neighbours, RETURN no adjustment
        if (filteredContext.Count == 0) return Vector3.zero;

        // ADD ALL points together and AVERAGE
        Vector3 cohesionMove = Vector3.zero;
        
        foreach (var item in filteredContext) // FOR EACH NEIGHBOUR...
        {
            cohesionMove += item.position; // TURN IN NEIGHBOUR'S DIRECTION
        }
        cohesionMove /= context.Count; // FIND AVERAGE OF ALL DIRECTIONS

        cohesionMove -= agent.transform.position; // CREATE OFFSET from agent position
        // GET SMOOTHED MOVEMENT DIRECTION
        cohesionMove = Vector3.SmoothDamp(agent.transform.forward, cohesionMove, ref _currentVelocity, AgentSmoothTime);

        return cohesionMove.GroundV3(); // RETURN NEW GROUNDED DIRECTION
    }

   
}