using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public static InputManager _inputManager;

    void Awake()
    {
        Instance = this;
        _inputManager = InputManager.Instance;
        _startingTimer = _maxTimer;
    }

    [Header("Playing UI")]
    [SerializeField] GameObject _allPlayingUI;
    [SerializeField] TextMeshProUGUI _countText, _timerText, _scoreText;
    [SerializeField] int _maxTimer;
    int _startingTimer;
    float _runningTimer;
    

    [Header("Paused UI")]
    [SerializeField] GameObject _allPausedUI;

    [Header("Complete UI")]
    [SerializeField] GameObject _allCompleteUI;

    [Header("Failure UI")]
    [SerializeField] GameObject _allFailureUI;

    public int _sheepCount { get; private set; }
    public int _maxSheep { get; private set; }

    public void InitialiseUI()
    {
        _sheepCount = 0;
        _maxSheep = GameManager.Instance.AgentCount;
        _runningTimer = _startingTimer;
        UpdateUI();
        
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape)) TogglePauseState(GameManager.Instance.State);

        _runningTimer -= Time.deltaTime;
        DisplayTime(_runningTimer);

        if (_runningTimer < 0)
        {
            _runningTimer = 0;
            GameManager.Instance.UpdateGameState(GameState.Failure);
        }
    }

    void TogglePauseState(GameState currentState)
    {
        if (currentState == GameState.Paused) GameManager.Instance.UpdateGameState(GameState.Playing);
        else if (currentState == GameState.Playing) GameManager.Instance.UpdateGameState(GameState.Paused);
    }

    public void UpdateUIState(GameState newState)
    {
        _allPlayingUI.SetActive(newState == GameState.Playing);
        _allPausedUI.SetActive(newState == GameState.Paused);
        _allCompleteUI.SetActive(newState == GameState.Complete);
        _allFailureUI.SetActive(newState == GameState.Failure);
    }

    void UpdateUI()
    {
        _countText.text = _sheepCount + "/" + _maxSheep;
        _scoreText.text = "" + GameManager.Instance.GameScore;
    }

    public void IncrementCapturedSheep()
    {
        _sheepCount++;
        UpdateUI();

        if (_sheepCount >= _maxSheep) GameManager.Instance.UpdateGameState(GameState.Complete);
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);

        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void NextLevel()
    {
        GameManager.Instance.IncreaseGameScore(1);
        GameManager.Instance.IncreaseAgentCount(5);

        if (GameManager.Instance.GameScore % 3 == 0) // EVERY 3 ROUNDS...
        {
            ObstacleManager.Instance.IncreaseObstacleCount(1);
            DecreaseStartingTime(10);
        }

        GameManager.Instance.UpdateGameState(GameState.GenerateLevel);
    }

    public void RestartLevel()
    {
        GameManager.Instance.ResetGameScore();
        GameManager.Instance.ResetAgentCount();

        ResetStartingTimer();
        ObstacleManager.Instance.ResetObstacleCount();

        GameManager.Instance.UpdateGameState(GameState.GenerateLevel);
    }

    void ResetStartingTimer()
    {
        _startingTimer = _maxTimer;
    }

    void DecreaseStartingTime(int value)
    {
        if (_startingTimer > 60)
            _startingTimer -= value;
    }

    public void Quit()
    {
        Debug.Log("Back");
        SceneManager.LoadScene(0);
    }

    private void OnEnable()
    {
        _inputManager.OnPauseGame += TogglePauseState;
    }

    private void OnDisable()
    {
        _inputManager.OnPauseGame -= TogglePauseState;
    }
}
