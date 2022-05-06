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

    TransformAccessArray _agentTransforms;
    public List<Dog> _allDogs;

    // Start is called before the first frame update
    void Start()
    {
        _agentTransforms = new TransformAccessArray(StartingCount);

        _squareMaxSpeed = MaxSpeed * MaxSpeed;
        _squareNeighbourRadius = NeighbourRadius * NeighbourRadius;
        _squareAvoidanceRadius = _squareNeighbourRadius * AvoidanceRadiusMultiplier * AvoidanceRadiusMultiplier;

        for (int i = 0; i < StartingCount; i++)
        {
            var randPosV3 = (UnityEngine.Random.insideUnitCircle * StartingCount * AgentDensity).ConvertV2ToV3() + transform.position;

            FlockAgent newAgent = Instantiate(
                AgentPrefab,
                randPosV3,
                Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0f, 360f)),
                transform);
            newAgent.name = "Agent " + i;
            newAgent.Initialise(this);
            _agents.Add(newAgent);
            _agentTransforms.Add(newAgent.transform);
        }
    }

    [SerializeField] private bool _useJobs;
    // Update is called once per frame
    void Update()
    {
        float startTime = Time.realtimeSinceStartup;

        

        if (_useJobs)
        {
            foreach (var agent in _agents)
            {

                List<Transform> context = GetNearbyObjects(agent);

                Vector3 move = Behaviour.CalculateMove(agent, context, this); // Move based on Applied Behaviour

                move *= DriveFactor; // Link Movement speed to DriveFactor
                if (move.sqrMagnitude > _squareMaxSpeed) move = move.normalized * MaxSpeed; // CAP Movement speed to MaxSpeed

                agent.Move(move); // Move Agent in "move" direction

            }


            SetAgentSpeedFromDogsJob();
        }
        else
        {
            foreach (var agent in _agents)
            {
                
                List<Transform> context = GetNearbyObjects(agent);

                Vector3 move = Behaviour.CalculateMove(agent, context, this); // Move based on Applied Behaviour

                move *= DriveFactor; // Link Movement speed to DriveFactor
                if (move.sqrMagnitude > _squareMaxSpeed) move = move.normalized * MaxSpeed; // CAP Movement speed to MaxSpeed

                agent.Move(move); // Move Agent in "move" direction
                
            }

            foreach (var agent in _agents)
            {
                agent.MoveSpeed = SetAgentSpeedFromDogs(agent);


            }
            

        }

        //Debug.Log(((Time.realtimeSinceStartup - startTime) * 1000f) + "ms");

    }

    void SetAgentSpeedFromDogsJob()
    {
        NativeArray<float> speedList = new NativeArray<float>(_agents.Count, Allocator.TempJob); // ARRAY FOR CURRENT DATA (EMPTY)
        for (int i = 0; i < _agents.Count; i++)
        {
            if (_agents[i].DogList.Count == 0) continue;
            speedList[i] = _agents[i].MoveSpeed; // FILL NEW ARRAYS WITH CURRENT DATA
        }

        NativeArray<float3> dogPosList = new NativeArray<float3>(_allDogs.Count, Allocator.TempJob); // ARRAY FOR CURRENT DATA (EMPTY)
        for (int i = 0; i < _allDogs.Count; i++)
        {
            dogPosList[i] = _allDogs[i].transform.position.Vector3ToFloat3(); // FILL NEW ARRAYS WITH CURRENT DATA
        }

        SetAgentSpeedFromDogsJob setAgentSpeedFromDogsJob = new SetAgentSpeedFromDogsJob // SET JOB VALUES
        {
            speedList = speedList, // PASS FLOATs INTO JOB
            dogPosList = dogPosList, // PASS FLOAT3s INTO JOB
        };

        JobHandle jobHandle = setAgentSpeedFromDogsJob.Schedule(_agentTransforms); // QUEUE UP ALL JOBS ONTO THREADS

        jobHandle.Complete(); // RUN THROUGH QUEUE UNTIL ALL THREADS HAVE COMPLETED THEIR JOBS

        for (int i = 0; i < _agents.Count; i++)
        {
            _agents[i].MoveSpeed = speedList[i]; // PASS IN NEW VALUES INTO SCRIPTS
        }

        speedList.Dispose(); // DISPOSE OF ARRAY TO AVOID MEMORY LEAK
        dogPosList.Dispose(); // DISPOSE OF ARRAY TO AVOID MEMORY LEAK
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

        var speed = 1f - (closestDistance / 7);
        if (speed < 0.1f) speed = 0.1f;

        return speed;
    }

    public void ToggleJobs()
    {
        _useJobs = !_useJobs;
    }

}

[BurstCompile]
public struct SetAgentSpeedFromDogsJob : IJobParallelForTransform
{
    //[NativeDisableContainerSafetyRestriction]
    [ReadOnly] public NativeArray<float3> dogPosList;

    public NativeArray<float> speedList;

    public void Execute(int index, TransformAccess transform)
    {
        float closestDistance = 10;
        float newDistance;
        int count = 0;
        foreach (var dog in dogPosList)
        {
            if (count == 0)
            {
                closestDistance = Helper.DistanceF3(transform.position.Vector3ToFloat3(), dogPosList[count]);
                continue;
            }

            newDistance = Helper.DistanceF3(transform.position.Vector3ToFloat3(), dogPosList[count]);
            if (newDistance < closestDistance) closestDistance = newDistance;

            count++;
        }

        var speed = 1f - (closestDistance / 7);
        if (speed < 0.1f) speed = 0.1f;

        speedList[index] = speed;
    }
}
