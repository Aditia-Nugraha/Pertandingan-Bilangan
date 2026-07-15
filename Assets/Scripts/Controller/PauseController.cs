using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject _container;
    [SerializeField] private GameObject _background;
    [SerializeField] private GameObject _panel;
    [SerializeField] private GameObject _exitPanel;

    [Header("Animation")]
    [SerializeField] private ScalePanelAnimator _panelAnimator;

    private void Awake()
    {
        Hide();
    }

    public void Show()
    {
        _container.SetActive(true);
        _background.SetActive(true);
        _panel.SetActive(true);
        _panelAnimator.PlayShow();
        PauseGame();
    }

    public void Hide()
    {
        _panel.SetActive(false);
        _background.SetActive(false);
        _container.SetActive(false);
        _exitPanel.SetActive(false);
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    public void Resume()
    {
        AudioManager.Instance.PlaySfx(GameSfx.Discard);
        _background.SetActive(false);

        _panelAnimator.PlayHide(() =>
        {
            ResumeGame();
            Hide();
        });
    }

    public void Exit()
    {
        _panel.SetActive(false);
        _exitPanel.SetActive(true);
    }

    public void NoExit()
    {
        _panel.SetActive(true);
        _exitPanel.SetActive(false);
    }

    public void YesExit()
    {
        ResumeGame();
        SceneManager.LoadScene("BattleMenu");
    }
}