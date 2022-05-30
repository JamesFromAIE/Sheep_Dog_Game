using UnityEngine;
using UnityEngine.AI;

public class Dog : MonoBehaviour
{
    [SerializeField] NavMeshAgent _nMAgent; // NAVMESH AGENT COMPONENT
    [SerializeField] MeshRenderer _selectedCircle; // HIGHLIGHTED CIRCLE BENEATH DOG
    public bool IsSelected { get; private set; } = false; // BOOL FOR IF DOG IS CURRENTLY SELECTED
    public bool IsSitting { get; private set; } // BOOL FOR IF DOG IS CURRENTLY SITTING
    public DogStates State { get; private set; } = DogStates.Idle; // PUBLIC ENUM FOR DOG STATE

    [SerializeField] AudioSource _audSource; // RUNNING AUDIO SOURCE COMPONENT
    public AudioClip _selectSound; // RUNNING CLIP ASSIGNED BY MANAGER

    #region Public Methods

    public void MoveNVAgent(Vector3 destination)
    {
        if (IsSitting) // IF DOG IS SITTING...
        {
            _nMAgent.SetDestination(transform.position); // DONT MOVE!!!
            return;
        }

        // IF DOG IS TOO CLOSE TO NEW DESTINATION...
        if (Vector3.Distance(transform.position, destination) < 0.6f || GameManager.Instance.State != GameState.Playing)
        {
            _nMAgent.SetDestination(transform.position); // DONT MOVE!!!
            ChangeDogState(DogStates.Idle); // DOG IS NOW IDLE
        }
        else
        {
            _nMAgent.SetDestination(destination); // DOG MOVES IN THIS DIRECTION
            var lookDir = destination - transform.position; // GET DIRECTION TO LOOK TOWARD DESTINATION
            transform.forward = lookDir.FlattenLookDirection(); // LOOK IN FLATTENED DIRECTION
            ChangeDogState(DogStates.Moving); // DOG IS NOW MOVING
        }
        
    }
    public void ChangeDogState(DogStates newState)
    {
        switch (newState)
        {
            case DogStates.Idle: // IN CASE DOG IS IDLE...
                AudioManager.Instance.PlayDogRun(false); // DON'T PLAY RUNNING SOUND
                IsSitting = false; // DOG IS NOT SITTING
                break;
            case DogStates.Sitting: // IN CASE DOG IS SITTING...
                AudioManager.Instance.PlayDogRun(false); // DON'T PLAY RUNNING SOUND
                IsSitting = true; // DOG IS SITTING
                break;

            case DogStates.Moving: // IN CASE DOG IS MOVING...
                AudioManager.Instance.PlayDogRun(true); // PLAY RUNNING SOUND!!!
                IsSitting = false; // DOG IS NOT SITTING
                break;
        }
    }

    public void DogSelected(bool condition) // TOGGLE WHETHER DOG IS SELECTED OR NOT
    {
        IsSelected = condition; // TOGGLE DOG SELECTION
        _selectedCircle.enabled = condition; // TOGGLE SELECTED CIRCLE BENEATH DOG
        if (condition) _audSource.PlayClip(_selectSound); // PLAY BARK SOUND
    }

    #endregion

}
