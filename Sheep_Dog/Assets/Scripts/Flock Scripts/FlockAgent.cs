using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FlockAgent : MonoBehaviour
{
    public AgentState _agentState; // VARIABLE FOR CURRENT AGENT STATE
    
    public float MoveSpeed = 1; // REFERNCE OF MOVE SPEED FOR OTHER SCRIPTS TO ALTER

    [SerializeField] float MoveModifier; // PERSONAL VARIABLE TO ALTER MOVE SPEED
    float _bleetFactor; // VARIABLE FOR HOW LIKELY A SHEEP IS TO BLEET

    FlockManager _agentFlock; // VARIABLE TO REFERENCE ORIGIN FLOCK
    public FlockManager AgentFlock { get { return _agentFlock; } } // PUBLIC GETTER FOR FLOCK

    Collider _agentCollider; // VARIABLE TO REFERENCE COLLIDER COMPONENT
    public Collider AgentCollider { get { return _agentCollider; } } // PUBLIC GETTER FOR AGENT COLLIDER

    List<Dog> _dogList = new List<Dog>(); // LIST OF ALL NEARBY DOGS
    public List<Dog> DogList { get { return _dogList; } } // PUBLIC GETTER FOR NEARBY DOGS


    // Start is called before the first frame update
    void Start()
    {
        _agentCollider = GetComponent<Collider>(); // SET REFERENCE TO COLLIDER ON THIS OBJECT
    }

    void Update()
    {
        if (Random.value < _bleetFactor) AudioManager.Instance.PlaySheepBleet(); // ALWAYS HAVE A CHANCE OF BLEETING 
    }

    public void ChangeAgentState(AgentState newState)
    {
        _agentState = newState; // SET AGENT STATE TO NEW STATE

        switch (_agentState)
        {
            case AgentState.Idle: // IN CASE AGENT IS IDLE...
                _bleetFactor = 0.001f; // DECREASE CHANCE OF BLEETING
                break;
            case AgentState.Scared: // IN CASE AGENT IS SCARED...
                _bleetFactor = 0.005f; // INCREASE CHANCE OF BLEETING
                break;
        }
    }


    public void Initialise(FlockManager flock)
    {
        _agentFlock = flock; // SET ORIGIN FLOCK FROM FLOCK SCRIPT
    }

    public void Move(Vector3 velocity)
    {
        if (_agentState == AgentState.Scared) // IF AGENT IS CURRENTLY SCARED...
        {
            transform.forward = velocity.normalized; // FACE IN MOVEMENT DIRECTION
            transform.position += Time.deltaTime * MoveSpeed * MoveModifier * velocity; // MOVE FORWARD IN MOVEMENT DIRECTION
        }
        else if (_agentState == AgentState.Idle) // IF AGENT IS CURRENTLY IDLE...
        {
            if (Random.Range(0, 500) < 1) MoveSpeed = Random.Range(1, 10) / 100; // RANDOMISE IDLE SPEED EVERY SO OFTEN

            transform.forward = velocity.normalized; // FACE IN MOVEMENT DIRECTION
            transform.position += Time.deltaTime * MoveSpeed * MoveModifier * velocity; // MOVE FORWARD IN MOVEMENT DIRECTION

        }
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 3);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Dog dog)) // IF IN RANGE OF DOG RADAR...
        {
            if (!_dogList.Contains(dog)) _dogList.Add(dog); // IF DOG WAS PREVIOUSLY UNKNOWN, ADD IT TO LIST

            ChangeAgentState(AgentState.Scared); // CHANGE CURRENT STATE TO SCARED
        }
        
        if (other.CompareTag("Gate")) // IF COLLIDED WITH GATE...
        {
            // ENTERED OUTSIDE GATE

            _agentFlock.PlayConfetti(transform.position);
            _agentFlock.RemoveAgentFromList(this); // REMOVE AGENT FROM FLOCK ORIGIN SCRIPT
            UIManager.Instance.IncrementCapturedSheep(); // INCREASE NUMBER OF CAPTURED SHEEP IN UI

            Destroy(gameObject); // DESTROY THIS OBJECT
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Dog dog)) // IF OUT OF RANGE OF ANY DOGS...
        {
            if (_dogList.Contains(dog)) _dogList.Remove(dog); // REMOVE DOG THAT'S OUT OF RANGE

            ChangeAgentState(AgentState.Idle); // CHANGE CURRENT STATE TO IDLE
        }
    }

}

public enum AgentState
{
    Idle, 
    Scared
}
