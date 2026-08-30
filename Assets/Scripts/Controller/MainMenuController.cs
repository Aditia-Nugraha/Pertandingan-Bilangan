using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Popup")]
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _howToPlayPanel;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _aboutPanel;

    public void Play()
    {
        SceneManager.LoadScene("BattleMenu");
    }

    public void OpenHowToPlay()
    {
        CloseAllPopups();
        _mainMenuPanel.SetActive(false);
        _howToPlayPanel.SetActive(true);
    }

    public void Learn()
    {
        SceneManager.LoadScene("LearningMenu");
    }

    public void OpenSettings()
    {
        CloseAllPopups();
        _mainMenuPanel.SetActive(false);
        _settingsPanel.SetActive(true);
    }

    public void OpenAbout()
    {
        CloseAllPopups();
        _mainMenuPanel.SetActive(false);
        _aboutPanel.SetActive(true);
    }

    public void ClosePopup()
    {
        CloseAllPopups();
    }

    private void CloseAllPopups()
    {
        _howToPlayPanel.SetActive(false);
        _settingsPanel.SetActive(false);
        _aboutPanel.SetActive(false);
        _mainMenuPanel.SetActive(true);
    }

    public void Exit()
    {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
            Application.Quit();
    #endif
    }
}