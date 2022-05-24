using System.Collections.Generic;
using UnityEngine;

public abstract class ContextFilter : ScriptableObject
{
    public abstract List<Transform> Filter(FlockAgent agent, List<Transform> original); // ABSTRACT CLASS FOR FILTERS TAKING IN CONTEXT
}
