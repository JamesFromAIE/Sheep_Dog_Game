using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    private void Awake() => Instance = this;

    public int AgentCount;

    public GameObject ControlsPage, CreditsPage, BackButton;

    void Start()
    {
        ControlsPage.SetActive(false);
        CreditsPage.SetActive(false);
        BackButton.SetActive(false);

        Flock.Instance.SpawnNewFlock();
    }

    public void ShowCredits()
    {
        ControlsPage.SetActive(false);
        CreditsPage.SetActive(true);
        BackButton.SetActive(true);
    }

    public void ShowControls()
    {
        ControlsPage.SetActive(true);
        CreditsPage.SetActive(false);
        BackButton.SetActive(true);
    }

    public void CloseWindow()
    {
        ControlsPage.SetActive(false);
        CreditsPage.SetActive(false);
        BackButton.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!!!");
        Application.Quit();
    }
}
