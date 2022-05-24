using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class Dog : MonoBehaviour
{
    [SerializeField] NavMeshAgent NMAgent;
    public float MoveSpeed;
    [SerializeField] MeshRenderer _selectedCircle;
    public bool IsSelected { get; private set; } = false;
    public bool IsSitting { get; private set; }
    public DogStates State { get; private set; } = DogStates.Idle;

    [SerializeField] AudioSource _audSource;
    public AudioClip _selectSound;

    public List<Vector3> PreviousPath { get; private set; } = new List<Vector3>();
    Vector3 _currentVelocity;
    [SerializeField] float _turnTime;

    #region Public Methods

    public void MoveNVAgent(Vector3 destination)
    {
        if (IsSitting)
        {
            NMAgent.SetDestination(transform.position);
            return;
        }

        if (Vector3.Distance(transform.position, destination) < 0.2f || GameManager.Instance.State != GameState.Playing)
        {
            NMAgent.SetDestination(transform.position);
            ChangeDogState(DogStates.Idle);
            
        }
        else
        {
            NMAgent.SetDestination(destination);
            var lookDir = destination - transform.position;
            transform.forward = lookDir.FlattenLookDirection();
            ChangeDogState(DogStates.Moving);
        }
        
    }
    public void ChangeDogState(DogStates newState)
    {
        switch (newState)
        {
            case DogStates.Idle:
                AudioManager.Instance.PlayDogRun(false);
                IsSitting = false;

                //if (UnityEngine.Random.Range(0, 500) < 10) DogManager.Instance.GetAndSetRandomDestination(this);

                break;
            case DogStates.Sitting:
                AudioManager.Instance.PlayDogRun(false);
                IsSitting = true;
                break;

            case DogStates.Moving:
                AudioManager.Instance.PlayDogRun(true);
                IsSitting = false;
                break;
        }
    }

    public void DogSelected(bool condition)
    {
        IsSelected = condition;
        _selectedCircle.enabled = condition;
        if (condition) _audSource.PlayClip(_selectSound);
    }

    #endregion

    #region Private Methods
    #endregion
}
