using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Avoidance")]
public class AvoidanceBehaviour : FilteredFlockBehaviour
{
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        // IF No neighbours, RETURN no adjustment
        if (context.Count == 0) return Vector3.zero;

        // ADD ALL points together and AVERAGE
        Vector3 avoidanceMove = Vector3.zero;
        int nAvoid = 0; // NUMBER of things to AVOID

        List<Transform> filteredContext = (Filter == null) ? context : Filter.Filter(agent, context); // USE FILTERED LIST IF APPLIED TO THIS
        foreach (var item in filteredContext)
        {
            float objDist;
            var agentScript = item.GetComponent<FlockAgent>();
            var agentPos = agent.transform.position;
            var collPoint = item.GetComponent<Collider>().ClosestPoint(agentPos);

            if (agentScript != null) objDist = Vector3.SqrMagnitude(item.position - agentPos); // IF TRANSFORM is AGENT...
            else objDist = Vector3.SqrMagnitude(collPoint - agentPos); // IF TRANSFORM is NOT AGENT...

            if (objDist < flock.SquareAvoidanceRadius)
            {
                nAvoid++;
                avoidanceMove += agent.transform.position - item.position;
            }
        }

        if (nAvoid > 0) avoidanceMove /= nAvoid;

        return avoidanceMove.GroundV3();
    }

}
