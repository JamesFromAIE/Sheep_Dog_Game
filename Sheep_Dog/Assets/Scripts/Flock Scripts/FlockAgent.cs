using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FlockAgent : MonoBehaviour
{
    public AgentState _agentState;
    
    public float MoveSpeed = 1;

    [SerializeField] float MoveModifier;
    Flock _agentFlock;
    float _bleetFactor;
    public Flock AgentFlock { get { return _agentFlock; } }

    Collider _agentCollider;
    public Collider AgentCollider { get { return _agentCollider; } }

    List<Dog> _dogList = new List<Dog>();
    public List<Dog> DogList { get { return _dogList; } }


    // Start is called before the first frame update
    void Start()
    {
        _agentCollider = GetComponent<Collider>();
    }

    void Update()
    {
        if (Random.value < _bleetFactor) AudioManager.Instance.PlaySheepBleet();
    }

    public void ChangeAgentState(AgentState newState)
    {
        _agentState = newState;

        switch (_agentState)
        {
            case AgentState.Idle:
                _bleetFactor = 0.001f;
                break;
            case AgentState.Scared:
                _bleetFactor = 0.005f;
                break;
        }
    }


    public void Initialise(Flock flock)
    {
        _agentFlock = flock;
    }

    public void Move(Vector3 velocity)
    {
        if (_agentState == AgentState.Scared)
        {
            if (velocity != Vector3.zero)
            {
                transform.forward = velocity.normalized;
                transform.position += Time.deltaTime * MoveSpeed * MoveModifier * velocity;
            }
                
        }
        else if (_agentState == AgentState.Idle)
        {
            if (Random.Range(0, 500) < 1)
            {
                MoveSpeed = Random.Range(1, 10) / 100;
            }
            if (velocity != Vector3.zero)
            {
                transform.forward = velocity.normalized;
                transform.position += Time.deltaTime * MoveSpeed * MoveModifier * velocity;
            }
        }
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 3);

        Gizmos.color = Color.blue;
        if (_agentState == AgentState.Idle) Gizmos.DrawWireSphere(transform.position, 1f);
    }

    public float GetClosestDogDistance()
    {
        float closestDog = 10;
        int count = 0;
        foreach (var dog in _dogList)
        {
            if (count == 0)
            {
                closestDog = Vector3.Distance(transform.position, dog.transform.position);
                continue;
            }

            var distance = Vector3.Distance(transform.position, dog.transform.position);
            if (distance < closestDog) closestDog = distance;
            
            count++;
        }
        return closestDog;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Dog dog))
        {
            if (!_dogList.Contains(dog)) _dogList.Add(dog);

            ChangeAgentState(AgentState.Scared);
        }
        
        if (other.CompareTag("Gate"))
        {
            // ENTERED OUTSIDE GATE

            _agentFlock.RemoveAgentFromList(this);
            UIManager.Instance.IncrementCapturedSheep();

            Destroy(gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Dog dog))
        {
            if (_dogList.Contains(dog)) _dogList.Remove(dog);

            ChangeAgentState(AgentState.Idle);
        }
    }
}

public enum AgentState
{
    Idle, 
    Scared
}
