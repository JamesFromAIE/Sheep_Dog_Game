using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Burst;

public class Flock : MonoBehaviour
{
    public static Flock Instance;

    private void Awake() => Instance = this;

    public FlockAgent BaseAgentPrefab;
    public FlockAgent AlternateAgentPrefab;
    List<FlockAgent> _agents = new List<FlockAgent>();
    public FlockBehaviour Behaviour;

    const float AgentDensity = .4f;

    [Range(1f, 100f)]
    public float DriveFactor = 10f; // Move Speed Multiplier for Agents
    [Range(1f, 100f)]
    public float MaxSpeed = 5f; // Max Speed of Agents
    [Range(.1f, 2f)]
    public float NeighbourRadius = 1.5f; // Distance to consider an Agent as a Neighbour
    [Range(0f, 1f)]
    public float AvoidanceRadiusMultiplier = 0.5f; // NeighbourRadius Multiplier for Agents

    public Collider _spawnBounds;

    // Utility Floats to save calculations
    float _squareMaxSpeed; 
    float _squareNeighbourRadius; 
    float _squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return _squareAvoidanceRadius;} }

    // Start is called before the first frame update
    void Start()
    {
        //SpawnNewFlock();
    }

    public void SpawnNewFlock()
    {
        _agents?.Clear();

        transform.DeleteChildren();

        int spawnCount;
        DogPathfinding pathRef = DogPathfinding.Instance;

        if (GameManager.Instance != null) spawnCount = GameManager.Instance.AgentCount;
        else spawnCount = MenuManager.Instance.AgentCount;


        _squareMaxSpeed = MaxSpeed * MaxSpeed;
        _squareNeighbourRadius = NeighbourRadius * NeighbourRadius;
        _squareAvoidanceRadius = _squareNeighbourRadius * AvoidanceRadiusMultiplier * AvoidanceRadiusMultiplier;

        for (int i = 0; i < spawnCount; i++)
        {
            var randPosV3 = (UnityEngine.Random.insideUnitCircle * spawnCount * AgentDensity).ConvertV2ToV3() + transform.position;

            while (!randPosV3.IsPointSpawnable(_spawnBounds.bounds))
            {
                randPosV3 = (UnityEngine.Random.insideUnitCircle * spawnCount * AgentDensity).ConvertV2ToV3() + transform.position;
            }

            var prefab = UnityEngine.Random.value > 0.8f ? AlternateAgentPrefab : BaseAgentPrefab;

            FlockAgent newAgent = Instantiate(
                prefab,
                randPosV3,
                Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0f, 360f)),
                transform);
            newAgent.name = "Agent " + i;
            newAgent.Initialise(this);
            _agents.Add(newAgent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //float startTime = Time.realtimeSinceStartup;

        foreach (var agent in _agents)
        {

            List<Transform> context = GetNearbyObjects(agent);

            Vector3 move = Behaviour.CalculateMove(agent, context, this); // Move based on Applied Behaviour

            move *= DriveFactor; // Link Movement speed to DriveFactor
            if (move.sqrMagnitude > _squareMaxSpeed) move = move.normalized * MaxSpeed; // CAP Movement speed to MaxSpeed

            agent.Move(move); // Move Agent in "move" direction

            agent.MoveSpeed = SetAgentSpeedFromDogs(agent);

        }
        //Debug.Log(((Time.realtimeSinceStartup - startTime) * 1000f) + "ms");

    }

    public void RemoveAgentFromList(FlockAgent agent)
    {
        if (_agents.Contains(agent)) _agents.Remove(agent);
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


    private float SetAgentSpeedFromDogs(FlockAgent agent)
    {
        float closestDistance = 10;
        float newDistance;
        int count = 0;
        foreach (var dog in agent.DogList)
        {
            if (count == 0)
            {
                closestDistance = Vector3.Distance(agent.transform.position, dog.transform.position);
                continue;
            }

            newDistance = Vector3.Distance(agent.transform.position, dog.transform.position);
            if (newDistance < closestDistance) closestDistance = newDistance;

            count++;
        }

        var speed = 1f - (closestDistance / 10);
        if (speed < 0.1f) speed = 0.1f;

        return speed;
    }

}
