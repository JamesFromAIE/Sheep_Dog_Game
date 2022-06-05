using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance; // VARIABLE FOR SINGLETON

    private void Awake() => Instance = this; // SET SINGLETON TO THIS SCRIPT

    public int AgentCount; // VARIABLE TO CONTAIN SPAWN NUMBER

    public GameObject ControlsPage, SettingsPage, BackButton; // VARIABLES CONTAINING UI ELEMENTS FOR MENU PAGES

    public Button SoundButton, SFXButton, SoundOffButton, SFXOffButton;

    void Start()
    {
        // DISABLE ALL PAGES
        ControlsPage.SetActive(false);
        SettingsPage.SetActive(false);
        BackButton.SetActive(false);

        ObstacleManager.Instance.GetAndSetFirstLevelLayout();
        ObstacleManager.Instance.SpawnLevel();
        var obj = GameObject.FindGameObjectWithTag("Gate");
        DestroyImmediate(obj);
        FlockManager.Instance.SpawnFlock(); // SPAWN NEW FLOCK
        AddButtonListeners();
    }

    void AddButtonListeners()
    {
        SoundButton.onClick.AddListener(() => { ToggleSoundButton(true); AudioManager.Instance.SetMusicBool(false); });
        SoundOffButton.onClick.AddListener(() => { ToggleSoundButton(false); AudioManager.Instance.SetMusicBool(true); });
        SFXButton.onClick.AddListener(() => { ToggleSFXButton(true); AudioManager.Instance.SetSFXBool(false); });
        SFXOffButton.onClick.AddListener(() => { ToggleSFXButton(false); AudioManager.Instance.SetSFXBool(true); });

        AudioManager audInst = AudioManager.Instance;

        ToggleSoundButton(!audInst.IsMusicEnabled);
        ToggleSFXButton(!audInst.IsSFXEnabled);
    }
    
    void ToggleSoundButton(bool condition)
    {
        SoundOffButton.GetComponent<Image>().enabled = condition;
    }

    void ToggleSFXButton(bool condition)
    {
        SFXOffButton.GetComponent<Image>().enabled = condition;
    }

    public void ShowSettings()
    {
        // DISPLAY CREDITS ONLY
        ControlsPage.SetActive(false);
        SettingsPage.SetActive(true);
        BackButton.SetActive(true);
    }

    public void ShowControls()
    {
        // DISPLAY CONTROLS ONLY
        ControlsPage.SetActive(true);
        SettingsPage.SetActive(false);
        BackButton.SetActive(true);
    }

    public void CloseWindow()
    {
        // DISABLE ALL PAGES
        ControlsPage.SetActive(false);
        SettingsPage.SetActive(false);
        BackButton.SetActive(false);
    }

    public void PlayGame()
    {
        SoundButton.onClick.RemoveAllListeners();
        SoundOffButton.onClick.RemoveAllListeners();
        SFXButton.onClick.RemoveAllListeners();
        SFXOffButton.onClick.RemoveAllListeners();

        SceneManager.LoadScene(1); // LOAD PLAYING SCENE
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!!!"); // DEBUGGING LOG
        Application.Quit(); // CLOSE APPLICATION
    }
}
