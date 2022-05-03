using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockAgent AgentPrefab;
    List<FlockAgent> _agents = new List<FlockAgent>();
    public FlockBehaviour Behaviour;

    [Range(1, 100)]
    public int StartingCount = 50;
    const float AgentDensity = 0.1f;

    [Range(1f, 100f)]
    public float DriveFactor = 10f; // Move Speed Multiplier for Agents
    [Range(1f, 100f)]
    public float MaxSpeed = 5f; // Max Speed of Agents
    [Range(.1f, 2f)]
    public float NeighbourRadius = 1.5f; // Distance to consider an Agent as a Neighbour
    [Range(0f, 1f)]
    public float AvoidanceRadiusMultiplier = 0.5f; // NeighbourRadius Multiplier for Agents

    // Utility Floats to save calculations
    float _squareMaxSpeed; 
    float _squareNeighbourRadius; 
    float _squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return _squareAvoidanceRadius;} }


    // Start is called before the first frame update
    void Start()
    {
        _squareMaxSpeed = MaxSpeed * MaxSpeed;
        _squareNeighbourRadius = NeighbourRadius * NeighbourRadius;
        _squareAvoidanceRadius = _squareNeighbourRadius * AvoidanceRadiusMultiplier * AvoidanceRadiusMultiplier;

        for (int i = 0; i < StartingCount; i++)
        {
            var randPosV3 = (Random.insideUnitCircle * StartingCount * AgentDensity).ConvertV2ToV3();

            FlockAgent newAgent = Instantiate(

                AgentPrefab,
                randPosV3,
                Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)),
                transform);
            newAgent.name = "Agent " + i;
            newAgent.Initialise(this);
            _agents.Add(newAgent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(FlockAgent agent in _agents)
        {
            List<Transform> context = GetNearbyObjects(agent); 


            Vector3 move = Behaviour.CalculateMove(agent, context, this); // Move based on Applied Behaviour

            move *= DriveFactor; // Link Movement speed to DriveFactor
            if (move.sqrMagnitude > _squareMaxSpeed) move = move.normalized * MaxSpeed; // CAP Movement speed to MaxSpeed

            agent.Move(move); // Move Agent in "move" direction
        }
    }

    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, NeighbourRadius);

        foreach (Collider c in contextColliders)
        {
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }
        return context;
    }
}
