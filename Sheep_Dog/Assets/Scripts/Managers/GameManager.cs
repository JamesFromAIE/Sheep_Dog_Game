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
        UpdateGameState(GameState.Playing);
    }


    public void UpdateGameState(GameState newState)
    {
        State = newState;

        UIManager.Instance.UpdateUIState(State);

        switch (State)
        {
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
    Playing = 0,
    Paused = 1,
    Complete = 2,
    Failure = 3,
}
