using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance; // VARIABLE FOR SINGLETON

    private void Awake() => Instance = this; // SET SINGLETON TO THIS SCRIPT

    public int AgentCount; // VARIABLE TO CONTAIN SPAWN NUMBER

    public GameObject ControlsPage, CreditsPage, BackButton; // VARIABLES CONTAINING UI ELEMENTS FOR MENU PAGES

    void Start()
    {
        // DISABLE ALL PAGES
        ControlsPage.SetActive(false);
        CreditsPage.SetActive(false);
        BackButton.SetActive(false);

        ObstacleManager.Instance.GetAndSetFirstLevelLayout();
        ObstacleManager.Instance.SpawnLevel();
        FlockManager.Instance.SpawnFlock(); // SPAWN NEW FLOCK
    }

    public void ShowCredits()
    {
        // DISPLAY CREDITS ONLY
        ControlsPage.SetActive(false);
        CreditsPage.SetActive(true);
        BackButton.SetActive(true);
    }

    public void ShowControls()
    {
        // DISPLAY CONTROLS ONLY
        ControlsPage.SetActive(true);
        CreditsPage.SetActive(false);
        BackButton.SetActive(true);
    }

    public void CloseWindow()
    {
        // DISABLE ALL PAGES
        ControlsPage.SetActive(false);
        CreditsPage.SetActive(false);
        BackButton.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1); // LOAD PLAYING SCENE
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!!!"); // DEBUGGING LOG
        Application.Quit(); // CLOSE APPLICATION
    }
}
