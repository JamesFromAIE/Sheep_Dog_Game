using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    void Awake()
    {
        Instance = this;
    }

    [Header("Playing UI")]
    [SerializeField] GameObject _allPlayingUI;
    [SerializeField] TextMeshProUGUI _countText;
    [SerializeField] TextMeshProUGUI _timerText;
    [SerializeField] int _startingTimer;
    float _runningTimer;
    

    [Header("Paused UI")]
    [SerializeField] GameObject _allPausedUI;

    [Header("Complete UI")]
    [SerializeField] GameObject _allCompleteUI;

    [Header("Failure UI")]
    [SerializeField] GameObject _allFailureUI;


    void Start()
    {
        UpdateUI();
        _runningTimer = _startingTimer;
    }

    public int _sheepCount { get; private set; }
    public int _maxSheep;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePauseState(GameManager.Instance.State);

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
    }

    public void IncrementCapturedSheep()
    {
        _sheepCount++;
        UpdateUI();
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);

        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(0);
    }
}
