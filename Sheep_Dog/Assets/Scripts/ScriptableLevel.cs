using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Levels",fileName = "ScriptableLevel")]
public class ScriptableLevel : ScriptableObject
{
    [SerializeField] GameObject _fences;
    public GameObject Fences { get { return _fences; } }

    [SerializeField] Transform _midPoint;
    public Transform MidPoint { get { return _midPoint; } }

    [SerializeField] List<MeshCollider> _walkablePlanes;
    public List<MeshCollider> WalkablePlanes { get { return _walkablePlanes; } }

    

}
