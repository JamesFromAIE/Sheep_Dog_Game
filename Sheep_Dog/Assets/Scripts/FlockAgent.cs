using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FlockAgent : MonoBehaviour
{

    Flock _agentFlock;
    public Flock AgentFlock { get { return _agentFlock; } }

    Collider _agentCollider;
    public Collider AgentCollider { get { return _agentCollider; } }

    // Start is called before the first frame update
    void Start()
    {
        _agentCollider = GetComponent<Collider>();
    }

    public void Initialise(Flock flock)
    {
        _agentFlock = flock;
    }

    public void Move(Vector3 velocity)
    {
        transform.forward = velocity;
        transform.position += velocity * Time.deltaTime;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 3);
    }
}
