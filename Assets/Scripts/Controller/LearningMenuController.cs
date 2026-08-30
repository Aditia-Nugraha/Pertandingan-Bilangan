using UnityEngine;
using UnityEngine.SceneManagement;

public class LearningMenuController : MonoBehaviour
{
    [Header("Popup")]
    [SerializeField] private GameObject _topicPanel;
    [SerializeField] private GameObject _fraction;
    [SerializeField] private GameObject _decimal;
    [SerializeField] private GameObject _percentage;
    [SerializeField] private GameObject _visual;

    public void OpenFraction()
    {
        CloseAllPopups();
        _topicPanel.SetActive(false);
        _fraction.SetActive(true);
    }

    public void OpenDecimal()
    {
        CloseAllPopups();
        _topicPanel.SetActive(false);
        _decimal.SetActive(true);
    }

    public void OpenPercentage()
    {
        CloseAllPopups();
        _topicPanel.SetActive(false);
        _percentage.SetActive(true);
    }

    public void OpenVisual()
    {
        CloseAllPopups();
        _topicPanel.SetActive(false);
        _visual.SetActive(true);
    }

    public void ClosePopup()
    {
        CloseAllPopups();
    }

    private void CloseAllPopups()
    {
        _fraction.SetActive(false);
        _decimal.SetActive(false);
        _percentage.SetActive(false);
        _visual.SetActive(false);
        _topicPanel.SetActive(true);
    }

    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }
}