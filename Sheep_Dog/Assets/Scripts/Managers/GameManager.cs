using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    [SerializeField] NavMeshSurface _surface;

    public static GameManager Instance;
    public GameState State;

    public int GameScore { get; private set; } = 0;
    [Range(1, 100)]
    public int AgentCount = 50;
    int _startingCount;

    void Awake()
    {
        Instance = this;
        _startingCount = AgentCount;
    }

    void Start()
    {
        UpdateGameState(GameState.GenerateLevel);
    }


    public void UpdateGameState(GameState newState)
    {
        State = newState;

        UIManager.Instance.UpdateUIState(State);

        switch (State)
        {
            case GameState.GenerateLevel:
                SelectedDictionary.Instance.DeselectAll();
                Flock.Instance.SpawnNewFlock();
                ObstacleManager.Instance.SpawnFenceState();
                ObstacleManager.Instance.SpawnObstacles();
                DogManager.Instance.SpawnDogs();
                DogPathfinding.Instance.SetNewUnWalkablesList();
                UIManager.Instance.InitialiseUI();
                //_surface.BuildNavMesh();

                UpdateGameState(GameState.Playing);
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
            case GameState.Complete:
                Time.timeScale = 0f;
                break;
            case GameState.Failure:
                Time.timeScale = 0f;
                break;
        }
    }

    public void IncreaseGameScore(int value)
    {
        GameScore += value;
    }

    public void ResetGameScore()
    {
        GameScore = 0;
    }

    public void IncreaseAgentCount(int value)
    {
        AgentCount += value;
    }

    public void ResetAgentCount()
    {
        AgentCount = _startingCount;
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
