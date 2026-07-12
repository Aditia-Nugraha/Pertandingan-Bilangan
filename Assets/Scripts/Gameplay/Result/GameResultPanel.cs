using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameResultPanel : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject _container;
    [SerializeField] private GameObject _background;
    [SerializeField] private GameObject _panel;

    [Header("UI")]
    [SerializeField] private Image _resultPanel;
    [SerializeField] private Image _resultIcon;
    [SerializeField] private TMP_Text _playerNameText;
    [SerializeField] private TMP_Text _resultText;

    [Header("Sprite")]
    [SerializeField] private Sprite _winPanelSprite;
    [SerializeField] private Sprite _winIconSprite;
    [SerializeField] private Sprite _losePanelSprite;
    [SerializeField] private Sprite _loseIconSprite;

    [Header("Animation")]
    [SerializeField] private ScalePanelAnimator _panelAnimator;

    public void Show(MatchResult result)
    {
        _container.SetActive(true);
        _background.SetActive(true);
        _panel.SetActive(true);
        _playerNameText.text = PlayerProfile.Player1Name;

        switch (result)
        {
            case MatchResult.Win:
                _resultPanel.sprite = _winPanelSprite;
                _resultText.text = "Menang!";
                _resultIcon.sprite = _winIconSprite;
                break;

            case MatchResult.Lose:
                _resultPanel.sprite = _losePanelSprite;
                _resultText.text = "Kalah...";
                _resultIcon.sprite = _loseIconSprite;
                break;
        }

        _panelAnimator.PlayShow();
    }

    public void Hide()
    {
        _panel.SetActive(false);
        _container.SetActive(false);
    }

    public void Rematch()
    {
        _background.SetActive(false);
        _panelAnimator.PlayHide(RematchAfterHide);
    }

    private void RematchAfterHide()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Back()
    {
        SceneManager.LoadScene("BattleMenu");
    }
}