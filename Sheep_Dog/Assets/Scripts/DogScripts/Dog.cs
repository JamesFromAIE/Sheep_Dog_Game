using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using System;
using UnityEngine.AI;

public class Dog : MonoBehaviour
{
    [SerializeField] NavMeshAgent NMAgent;
    public float MoveSpeed;
    [SerializeField] MeshRenderer _selectedCircle;
    public bool IsSelected { get; private set; } = false;
    public CancellationTokenSource TokenSource = null;
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

        if (Vector3.Distance(transform.position, destination) < 0.2f)
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

    public async void MoveDogToPositionList(List<Vector3> posList)
    {
        PreviousPath = posList;

        TokenSource = new CancellationTokenSource();
        var token = TokenSource.Token;
        //IsMoving = true;
        ChangeDogState(DogStates.Moving);

        foreach (Vector3 pos in posList)
        {
            bool isCancelled = false;
            try
            {
                await MoveDogToVector3(pos, token);
            }
            catch (OperationCanceledException ex)
            {
                //Debug.Log("CAUGHT IT");
                TokenSource?.Dispose();
                TokenSource = null;
                ChangeDogState(DogStates.Idle);
            }
            finally
            {
                if (TokenSource == null) isCancelled = true;

            }
            if (isCancelled) break;
        }

        //TokenSource.Dispose();
        TokenSource = null;
        ChangeDogState(DogStates.Idle);
    }

    public void DogSelected(bool condition)
    {
        IsSelected = condition;
        _selectedCircle.enabled = condition;
        if (condition) _audSource.PlayClip(_selectSound);
    }

    public async void StallFunction(int time)
    {
        await Task.Delay(time);
    }

    #endregion

    #region Private Methods
    async Task MoveDogToVector3(Vector3 destination, CancellationToken token)
    {
        var end = Time.time + (1 / MoveSpeed);
        while (Time.time < end)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, MoveSpeed * Time.deltaTime);
            transform.forward = Vector3.SmoothDamp(transform.forward, destination - transform.position, ref _currentVelocity, _turnTime);
            //transform.forward = destination - transform.position;
            await Task.Yield();

            if (token.IsCancellationRequested)
            {
                //todo: cleanup
                
                token.ThrowIfCancellationRequested();
            }
        }
    }

    void OnDestroy()
    {
        TokenSource?.Dispose();
        TokenSource = null;
    }



    #endregion
}
