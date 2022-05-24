using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; // VARIABLE FOR SINGLETON
    public static InputManager _inputManager; // VARIABLE TO HOLD INPUT MANAGER

    void Awake()
    {
        Instance = this; // SET SINGLETON TO THIS SCRIPT
        _inputManager = InputManager.Instance; // GET REFERENCE FROM INPUT MANAGER SINGLETON
        _startingTimer = _maxTimer; // SET STARTING TIMER TO MAX TIMER
    }

    [Header("Playing UI")]
    [SerializeField] GameObject _allPlayingUI; // ALL UI FOR PLAYING
    [SerializeField] TextMeshProUGUI _countText, _timerText, _scoreText; // VARIABLES TO HOLD DYANMIC TEXT

    [SerializeField] int _maxTimer; // THE MAX TIMER FROM WHICH THE STARTING TIMER REFERS TO
    int _startingTimer; // THE STARTING TIME FROM WHICH THE RUNNING TIMER REFERS TO
    float _runningTimer; // VARIABLE TO HOLD TIMER WHICH TICKS DOWN DURING PLAY TIME
    

    [Header("Paused UI")]
    [SerializeField] GameObject _allPausedUI; // ALL UI FOR WHEN PAUSED

    [Header("Complete UI")]
    [SerializeField] GameObject _allCompleteUI; // ALL UI FOR VICTORY

    [Header("Failure UI")]
    [SerializeField] GameObject _allFailureUI; // ALL UI FOR FAILURE

    public int _sheepCount { get; private set; } // PUBLIC GETTER FOR NUMBER OF SHEEP CAUGHT
    public int _maxSheep { get; private set; } // PUBLIC GETTER FOR NUMBER OF SHEEP ON SCEEN START

    public void InitialiseUI()
    {
        _sheepCount = 0; // RESET SHEEP CAUGHT COUNT TO ZERO
        _maxSheep = GameManager.Instance.AgentCount; // SET MAX SHEEP TO THE AGENT COUNT IN THE GAME MANAGER
        _runningTimer = _startingTimer; // RUNNING TIME RESETS TO STARTING TIME
        UpdateUI(); // UPDATE UI TEXT VALUES
        
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape)) TogglePauseState(GameManager.Instance.State);

        _runningTimer -= Time.deltaTime; // RUNNING TIMER CONSTANTLY TICKS DOWN ON UPDATE
        DisplayTime(_runningTimer); // DISPLAY RUNNING TIME

        if (_runningTimer < 0) // IF RUNNING TIMER GOES BELOW ZERO...
        {
            _runningTimer = 0; // SET TIMER TO 0
            GameManager.Instance.UpdateGameState(GameState.Failure); // SET GAME STATE TO 'FAILURE'
        }
    }

    void TogglePauseState(GameState currentState)
    {
        // TOGGLE GAME STATES BETWEEN PAUSED AND PLAYING ACCORDINGLY
        if (currentState == GameState.Paused) GameManager.Instance.UpdateGameState(GameState.Playing);
        else if (currentState == GameState.Playing) GameManager.Instance.UpdateGameState(GameState.Paused);
    }

    public void UpdateUIState(GameState newState)
    {
        // UPDATE UI ACCORDING TO CURRENT GAME STATE
        _allPlayingUI.SetActive(newState == GameState.Playing);
        _allPausedUI.SetActive(newState == GameState.Paused);
        _allCompleteUI.SetActive(newState == GameState.Complete);
        _allFailureUI.SetActive(newState == GameState.Failure);
    }

    void UpdateUI()
    {
        _countText.text = _sheepCount + "/" + _maxSheep; // UPDATE COUNT TEXT TO DISPOLAY CURRENT AND TOTAL SHEEP
        _scoreText.text = "" + GameManager.Instance.GameScore; // UPDATE GAME SCORE ACCORDING TO LEVEL CLEARED IN GAME MANAGER
    }

    public void IncrementCapturedSheep()
    {
        _sheepCount++; // INCREMENT SHEEP COUNT BY ONE
        UpdateUI(); // UPDATE UI TEXT VALUES

        if (_sheepCount >= _maxSheep) GameManager.Instance.UpdateGameState(GameState.Complete); // IF ALL SHEEP ARE CAPTURED, SET GAME STATE TO COMPLETE
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1; // ADD ONTO TIME TO AVOID "0" APPEARING 

        float minutes = Mathf.FloorToInt(timeToDisplay / 60); // FLOOR TIME INTO MINUTES

        float seconds = Mathf.FloorToInt(timeToDisplay % 60); // MODULATE SECONDS FROM TIME

        _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // UPDATE TIMER TEXT WITH FORMATTED MINUTES AND SECONDS
    }

    public void NextLevel()
    {
        var gM = GameManager.Instance; // SIMPLIFY GAME MANAGER SINGLETON

        gM.IncreaseGameScore(1); // INCREASE GAME SCORE BY 1
        gM.IncreaseAgentCount(5); // INCREASE NUMBER OF AGENTS TO SPAWN BY 5

        if (gM.GameScore % 3 == 0) // EVERY 3 ROUNDS...
        {
            ObstacleManager.Instance.IncreaseObstacleCount(1); // INCREASE NUMBER OF OBSTACLES TO SPAWN BY 1
            DecreaseStartingTime(10); // DECREASE STARTING TIME BY 10 SECONDS
        }

        gM.UpdateGameState(GameState.GenerateLevel); // GENERATE NEW LEVEL
    }

    public void RestartLevel()
    {
        var gM = GameManager.Instance; // SIMPLIFY GAME MANAGER SINGLETON

        gM.ResetGameScore(); // RESET NUMBER OF CLEARED LEVELS
        gM.ResetAgentCount(); // RESET AGENT COUNT TO SPAWN

        ResetStartingTimer(); // RESET STARTING TIMER IN UI
        ObstacleManager.Instance.ResetObstacleCount(); // RESET NUMBER OF OBSTACLES TO SPAWN

        gM.UpdateGameState(GameState.GenerateLevel); // GENERATE NEW LEVEL
    }

    void ResetStartingTimer()
    {
        _startingTimer = _maxTimer; // SET STARTING TIMER TO ORIGINAL MAX TIMER
    }

    void DecreaseStartingTime(int value)
    {
        if (_startingTimer > 60) // IF STARTING TIME IS GREATER THAN SIXTY...
            _startingTimer -= value; // REDUCE STARTING TIME BY VALUE
    }

    public void Quit()
    {
        Time.timeScale = 1; // SET TIME BACK TO NORMAL PACE
        SceneManager.LoadScene(0); // LOAD MENU SCENE
    }

    private void OnEnable()
    {
        _inputManager.OnPauseGame += TogglePauseState; // SUBSCRIBE FUNCTION TO EVENT FROM INPUT MANAGER
    }

    private void OnDisable()
    {
        _inputManager.OnPauseGame -= TogglePauseState; // UNSUBSCRIBE FUNCTION FROM EVENT FROM INPUT MANAGER
    }
}
