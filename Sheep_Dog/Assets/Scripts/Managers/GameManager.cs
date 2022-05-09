using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState State;

    void Awake()
    {
        Instance = this;
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
                Flock.Instance.SpawnNewFlock();
                ObstacleManager.Instance.SpawnObstacles();
                DogManager.Instance.SpawnDogs();
                UIManager.Instance.InitialiseUI();

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



}

public enum GameState
{
    GenerateLevel=0,
    Playing = 1,
    Paused = 2,
    Complete = 3,
    Failure = 4,
}
