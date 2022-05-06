using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using System;

public class Dog : MonoBehaviour
{
    public float MoveSpeed;
    [SerializeField] MeshRenderer _selectedCircle;
    public bool IsSelected { get; private set; } = false;
    public CancellationTokenSource TokenSource = null;
    public bool IsSitting { get; private set; }
    public DogStates State { get; private set; } = DogStates.Idle;


    #region Public Methods

    public void ChangeDogState(DogStates newState)
    {
        switch (newState)
        {
            case DogStates.Idle:
                IsSitting = false;
                break;
            case DogStates.Sitting:
                IsSitting = true;
                break;

            case DogStates.Moving:
                IsSitting = false;
                break;
        }
    }

    public async void MoveDogToPositionList(List<Vector3> posList)
    {
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
                Debug.Log("CAUGHT IT");
                TokenSource.Dispose();
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

    public void WorkerSelected(bool condition)
    {
        IsSelected = condition;
        _selectedCircle.enabled = condition;
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
            await Task.Yield();

            if (token.IsCancellationRequested)
            {
                //todo: cleanup
                
                token.ThrowIfCancellationRequested();
            }
        }
    }

    #endregion
}
