using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Filter/Same Flock")]
public class SameFlockFilter : ContextFilter
{
    public override List<Transform> Filter(FlockAgent agent, List<Transform> original)
    {
        List<Transform> filtered = new List<Transform>(); // INITIALISE NEW LIST
        foreach (var item in original) // FOREACH NEIGHBOURING OBJECT 
        {
            FlockAgent itemAgent = item.GetComponent<FlockAgent>(); // GET AGENT COMPONENT
            
            // IF THIS OBJECT IS AN AGENT, BUT NOT THIS AGENT...
            if (itemAgent != null && itemAgent.AgentFlock == agent.AgentFlock)
            {
                filtered.Add(item); // ADD ITEM TO NEW LIST
            }
        }
        return filtered; // RETURN NEW LIST
    }
}

