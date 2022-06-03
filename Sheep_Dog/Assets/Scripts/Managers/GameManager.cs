using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    [SerializeField] NavMeshSurface _surface;

    public static GameManager Instance; // VARIABLE FOR SINGLETON
    public GameState State; // VARIABLE TO CONTAIN CURRENT GAME STATE

    public int GameScore { get; private set; } = 0; // PUBLIC GETTER FOR CURRENT GAME SCORE
    [Range(1, 100)]
    public int AgentCount = 50; // VARIABLE FOR NUMBER OF AGENTS TO SPAWN
    int _startingCount; // VARIABLE TO HOLD ORIGINAL AGENT COUNT FROM SCENE START

    void Awake()
    {
        Instance = this; // SET SINGLETON TO THIS SCRIPT
        _startingCount = AgentCount; // SET STARTING COUNT TO AGENT COUNT
    }

    void Start()
    {
        UpdateGameState(GameState.GenerateLevel); // GENERATE NEW LEVEL
    }


    public void UpdateGameState(GameState newState)
    {
        State = newState; // CURRENT GAME STATE IS SET TO NEW STATE

        UIManager.Instance.UpdateUIState(State); // UPDATE STATE OF UI NO MATTER THE STATE

        switch (State)
        {
            case GameState.GenerateLevel: // IN CASE OF GENERATE LEVEL...

                SelectedDictionary.Instance.DeselectAll();
                ObstacleManager.Instance.GetNewLevelLayout();
                ObstacleManager.Instance.SpawnLevel();
                ObstacleManager.Instance.SpawnObstacles();
                FlockManager.Instance.SpawnFlock();
                DogManager.Instance.SpawnDogs();
                UIManager.Instance.InitialiseUI(); // INITIALISE VALUES IN UI
                //_surface.BuildNavMesh();

                UpdateGameState(GameState.Playing); // SET GAMESTATE TO PLAYING
                break;

            case GameState.Playing: // IN CASE OF PLAYING...

                Time.timeScale = 1f; // PLAY REGULAR TIME

                DogManager.Instance.EnableDogs(); // ENABLE DOGS IN CURRENT SCENE
                FlockManager.Instance.EnableAgents(); // ENABLE AGENTS IN CURRENT SCENE
                break;

            case GameState.Paused: // IN CASE PAUSED...

                Time.timeScale = 0f; // STOP TIME

                DogManager.Instance.DisableDogs(); // DISABLE DOGS IN CURRENT SCENE
                FlockManager.Instance.DisableAgents(); // DISABLE AGENTS IN CURRENT SCENE
                break;

            case GameState.Complete: // IN CASE OF VICTORY...

                Time.timeScale = 0f; // STOP TIME

                DogManager.Instance.DisableDogs(); // DISABLE DOGS IN CURRENT SCENE
                FlockManager.Instance.DisableAgents(); // DISABLE AGENTS IN CURRENT SCENE
                break;

            case GameState.Failure: // IN CASE OF FAILURE...

                Time.timeScale = 0f; // STOP TIME

                DogManager.Instance.DisableDogs(); // DISABLE DOGS IN CURRENT SCENE
                FlockManager.Instance.DisableAgents(); // DISABLE AGENTS IN CURRENT SCENE
                break;
        }
    }

    public void IncreaseGameScore(int value)
    {
        GameScore += value; // INCREASE GAME SCORE BY VALUE
    }

    public void ResetGameScore()
    {
        GameScore = 0; // SET GAME SCORE TO ZERO
    }

    public void IncreaseAgentCount(int value)
    {
        AgentCount += value; // INCREASE AGENT COUNT TO SPAWN BY VALUE
    }

    public void ResetAgentCount()
    {
        AgentCount = _startingCount; // RESET AGENT COUNT TO SPAWN BACK TO STARTING COUNT
    }



}

public enum GameState
{
    GenerateLevel=0,
    Playing = 1,
    Paused = 2,
    Complete = 3,
    Failure = 4,
}
