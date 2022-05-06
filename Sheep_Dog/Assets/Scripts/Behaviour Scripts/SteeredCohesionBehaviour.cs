using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/SteeredCohesion")]
public class SteeredCohesionBehaviour : FilteredFlockBehaviour
{
    Vector3 _currentVelocity;
    float3 _currentSpeed;
    public float AgentSmoothTime = 0.5f;

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
        cohesionMove = Vector3.SmoothDamp(agent.transform.forward, cohesionMove, ref _currentVelocity, AgentSmoothTime);

        return cohesionMove.GroundV3();
    }

   
}