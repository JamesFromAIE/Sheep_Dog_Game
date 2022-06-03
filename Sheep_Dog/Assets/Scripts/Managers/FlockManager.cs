using System.Collections.Generic;
using UnityEngine;


public class FlockManager : MonoBehaviour
{
    public static FlockManager Instance; // VARIABLE FOR SINGLETON

    private void Awake() => Instance = this; // SET SINGLETON TO THIS SCRIPT

    public FlockAgent BaseAgentPrefab; // BASE PREFAB OF AGENT TO SPAWN
    public FlockAgent AlternateAgentPrefab; // ALTERNATE PREFAB OF AGENT TO SPAWN
    List<FlockAgent> _agents = new List<FlockAgent>(); // INITIALISE LIST FOR ALL AGENTS
    public FlockBehaviour Behaviour; // VARIABLE T HOLD BEHAVIOUR WHIHC WILL APPLY TO AGENTS

    const float AgentDensity = .8f; // DENSITY OF WHICH AGENTS SPAWN

    [Range(1f, 100f)]
    public float DriveFactor = 10f; // MOVE SPEED MULTIPLIERS FOR AGENTS
    [Range(1f, 100f)]
    public float MaxSpeed = 5f; // MAX SPEED OF AGENTS
    [Range(.1f, 2f)]
    public float NeighbourRadius = 1.5f; // DISTANCE TO CONSIDER AN AGENT A NEIGHBOUR
    [Range(0f, 1f)]
    public float AvoidanceRadiusMultiplier = 0.5f; // NEIGHBOUR RADIUS MULTIPLIER FOR AGENTS

    public Collider _spawnBounds; // BOUNDS IN WHICH AGENTS CAN SPAWN
    public ParticleSystem _confetti;

    // Utility Floats to save calculations
    float _squareMaxSpeed; 
    float _squareNeighbourRadius; 
    float _squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return _squareAvoidanceRadius;} }

    public void SpawnFlock()
    {
        _agents?.Clear(); // CLEAR AGENTS FOR AGENT LIST 

        transform.DeleteChildren(); // DELETE ALL CHILDREN IN THIS OBJECT

        int spawnCount; // INITIALISE SPAWN COUNT
        Vector3 center = ObstacleManager.Instance.MidPoint.position;
        List<MeshCollider> spawnPlanes = ObstacleManager.Instance.WalkablePlanes;

        // GET EITHER MENU OR GAME MANAGER STARTING COUNT BASED ON CURRENT SCENE
        if (GameManager.Instance != null) spawnCount = GameManager.Instance.AgentCount;
        else spawnCount = MenuManager.Instance.AgentCount;

        //--------------- SQUARED VALUES FOR CALCULATIONS BELOW INVOLVING SQR MAGNITUDE -----------------
        _squareMaxSpeed = MaxSpeed * MaxSpeed;
        _squareNeighbourRadius = NeighbourRadius * NeighbourRadius;
        _squareAvoidanceRadius = _squareNeighbourRadius * AvoidanceRadiusMultiplier * AvoidanceRadiusMultiplier;
        //                         ---------------------------------------------

        for (int i = 0; i < spawnCount; i++) // ITERATE FOR THE NUMBER OF AGENTS THERE ARE TO SPAWN
        {
            // GET NEW RANDOM POSITION TO SPAWN AGENT FROM 
            var randPosV3 = (Random.insideUnitCircle * spawnCount * AgentDensity).ConvertV2ToV3() + center;

            while (!randPosV3.IsPointSpawnableList(spawnPlanes)) // WHILE SPAWN POSIITON IS OUTSIDE OF SPAWNING BOUNDS...
            {
                // GET NEW RANDOM POSITION TO SPAWN AGENT FROM 
                randPosV3 = (Random.insideUnitCircle * spawnCount * AgentDensity).ConvertV2ToV3() + center;
            }

            var prefab = Random.value > 0.8f ? AlternateAgentPrefab : BaseAgentPrefab; // 20% CHANCE OF ALT PREFAB, 80% CHANCE OF BASE PREFAB

            FlockAgent newAgent = Instantiate( // SPAWN NEW AGENT AT POSITION
                prefab,
                randPosV3,
                Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0f, 360f)),
                transform);
            newAgent.name = "Agent " + i; // NAME AGENT
            newAgent.Initialise(this); // SET AGENT'S ORIGIN TO THIS FLOCK
            _agents.Add(newAgent); // ADD AGENT TO AGENT LIST
        }
    }

    void Update()
    {
        if (GameManager.Instance != null) if (GameManager.Instance.State != GameState.Playing) return;

        foreach (var agent in _agents)
        {

            List<Transform> context = GetNearbyObjects(agent);

            Vector3 move = Behaviour.CalculateMove(agent, context, this); // Move based on Applied Behaviour

            move *= DriveFactor; // Link Movement speed to DriveFactor
            if (move.sqrMagnitude > _squareMaxSpeed) move = move.normalized * MaxSpeed; // CAP Movement speed to MaxSpeed

            agent.Move(move); // Move Agent in "move" direction

            agent.MoveSpeed = SetAgentSpeedFromDogs(agent);

        }

    }

    public void RemoveAgentFromList(FlockAgent agent)
    {
        if (_agents.Contains(agent)) _agents.Remove(agent); // IF AGENT IS IN LIST, REMOVE IT
    }


    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>(); // INITIALISE TRANSFORM LIST

        Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, NeighbourRadius); // GET COLLIDERS FROM NEARBY OBJECTS

        foreach (Collider c in contextColliders) // FOREACH OBJECT CAUGHT IN OVERLAP SPHERE
        {
            if (c != agent.AgentCollider) // IF THIS COLLIDER ISN'T THIS AGENT'S COLLIDER
            {
                context.Add(c.transform); // ADD COLLIDER'S TRANSFORM TO LIST
            }
        }
        return context; // RETURN NEARBY OBJECTS
    }

    float SetAgentSpeedFromDogs(FlockAgent agent)
    {
        float closestDistance = 10; // INITIALISE CDISTANCE
        float newDistance; // INITIALISE NEW DISTANCE
        int count = 0; // INITIALISE DOG COUNT

        foreach (var dog in agent.DogList) // FOREACH DOG IN AGENT'S LIST OF NEARBY DOGS...
        {
            if (count == 0) // IF THIS IS THE FIRST ITERATION
            {
                closestDistance = Vector3.Distance(agent.transform.position, dog.transform.position); // GET DISTANCE FROM DOG AND AGENT
                count++; // INCREMENT ITERATION COUNT BY ONE
                continue;
            }

            newDistance = Vector3.Distance(agent.transform.position, dog.transform.position); // GET DISTANCE FROM DOG AND AGENT
            if (newDistance < closestDistance) closestDistance = newDistance; // IF N DISTANCE IS LESS THAN C DISTANCE, SET C DISTANCE TO N DISTANCE

            count++; // INCREMENT ITERATION COUNT BY ONE
        }

        var speed = 1f - (closestDistance / 10); // SPEED IS AFFECTED BY THE CLOSEST DOG
        if (speed < 0.1f) speed = 0.1f; // IF SPEED IS TOO SMALL, FLOOR DISTANCE AT 0.1F

        return speed; // RETURN NEW SPEED
    }

    public void EnableAgents()
    {
        foreach (var agent in _agents) if (!agent.enabled) agent.enabled = true; // ENABLE ALL AGENTS IN LIST
    }

    public void DisableAgents()
    {
        foreach (var agent in _agents) if (agent.enabled) agent.enabled = false; // DISABLE ALL AGENTS IN LIST
    }

    public void PlayConfetti(Vector3 agentPos)
    {
        var confetti = Instantiate(_confetti, agentPos, _confetti.transform.rotation);


    }

    

}
