using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Avoidance")]
public class AvoidanceBehaviour : FilteredFlockBehaviour
{
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, FlockManager flock)
    {
        List<Transform> filteredContext = (Filter == null) ? context : Filter.Filter(agent, context); // USE FILTERED LIST IF APPLIED TO THIS
        
        if (filteredContext.Count == 0) return Vector3.zero; // IF No neighbours, RETURN no adjustment

        Vector3 avoidanceMove = Vector3.zero; // ADD ALL points together and AVERAGE
        int nAvoid = 0; // NUMBER of things to AVOID

        
        foreach (var item in filteredContext) // FOR EACH OBJECT TO BE AVOIDED
        {
            float objDist;
            var agentScript = item.GetComponent<FlockAgent>(); // GET AGENT SCRIPT
            var dogScript = item.GetComponent<Dog>(); // GET DOG SCRIPT
            var agentPos = agent.transform.position; // GET AGENT POSITION
            var collPoint = item.GetComponent<Collider>().ClosestPoint(agentPos); // GET COLL POINT FROM OBJECT

            if (agentScript == null && dogScript == null)  // IF TRANSFORM is WALL OR OBSTACLE...
            {
                objDist = Vector3.SqrMagnitude(collPoint - agentPos); // GET DISTANCE BETWEEN AGENT AND OBJECT

                if (objDist < flock.SquareAvoidanceRadius) // IF OBJECT IS TOO CLOSE...
                {
                    nAvoid++;
                    avoidanceMove += (agent.transform.position - collPoint).normalized / 2; // TURN AWAY FROM OBJECT
                }
            }
            else
            {

                if (agentScript != null) objDist = Vector3.SqrMagnitude(item.position - agentPos); // IF TRANSFORM is AGENT
                else objDist = Vector3.SqrMagnitude(collPoint - agentPos); // IF TRANSFORM is DOG


                if (objDist < flock.SquareAvoidanceRadius) // IF OBJECT IS TOO CLOSE...
                {
                    nAvoid++;
                    avoidanceMove += agent.transform.position - item.position; // TURN AWAY FROM OBJECT
                }

            }
                
        }

        if (nAvoid > 0) avoidanceMove /= nAvoid; // FIND AVERAGE OF ALL DIRECTIONS

        return avoidanceMove.GroundV3(); // RETURN NEW GROUNDED DIRECTION
    }

}
