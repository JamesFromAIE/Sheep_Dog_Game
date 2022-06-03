using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/StayInRadius")]
public class StayInRadiusBehaviour : FlockBehaviour
{
    public Vector3 center; // CENTER OF FLOCK
    public float radius = 25f; // RADIUS FROM CENTER

    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, FlockManager flock)
    {
        Vector3 centerOffset = center - agent.transform.position; // GET TRUE WORLD CENTER
        float t = centerOffset.magnitude / radius; // GET MOVEMENT DIRECTION TO CENTER
        if (t < 0.9f) return Vector3.zero; // DO NOTHING IF INSIDE RADIUS

        return centerOffset * t * t;  // MOVE TOWARD ZERO IF OUTSIDE RADIUS
    }

}
