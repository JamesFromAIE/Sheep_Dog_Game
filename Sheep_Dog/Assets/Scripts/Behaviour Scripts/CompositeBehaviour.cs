using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Composite")]
public class CompositeBehaviour : FlockBehaviour
{
    public FlockBehaviour[] behaviours; // ALL BEHAVIOURS TO EB APPLIED
    public float[] weights; // CORRESPONDING WEIGHTS RELATED TO BEHAVIOURS
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, FlockManager flock)
    {
        
        if (weights.Length != behaviours.Length) // HANDLE DATA MISMATCH
        {
            Debug.LogError("Data mismatch in " + name, this);
            return Vector3.zero;
        }

        // SET UP MOVE
        Vector3 move = Vector3.zero;

        //ITERATE through behaviours
        for (int i = 0; i < behaviours.Length; i++)
        {
            Vector3 partialMove = behaviours[i].CalculateMove(agent, context, flock) * weights[i]; // GET BEHAVIOUR MOVE DIRECTION

            if (partialMove != Vector3.zero) // IF NOT (0,0,0)
            {
                if (partialMove.sqrMagnitude > weights[i] * weights[i]) // IF NEW DIRECTION OVERCOMES WIEGHT
                {
                    partialMove.Normalize(); // SET VECTOR3 TO MAGNITUDE OF 1
                    partialMove *= weights[i]; // MULTIPLY DIRECTION INFLUENCE
                }

                move += partialMove; // MOVE IN DIRECTION
            }
        }

        return move; // RETURN FINAL MOVE DIRECTION 
    }
}
