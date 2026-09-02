using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject _background;
    [SerializeField] private GameObject _panelRoot;

    [Header("Score")]
    [SerializeField] private TMP_Text _winValueText;
    [SerializeField] private TMP_Text _loseValueText;
    [SerializeField] private TMP_Text _totalScoreValueText;
    [SerializeField] private PersonalScoreManager _personalScoreManager;

    [Header("Animation")]
    [SerializeField] private ScalePanelAnimator _panelAnimator;

    public void ShowScore()
    {
        RefreshScore();

        gameObject.SetActive(true);
        _background.SetActive(true);
        _panelRoot.SetActive(true);
        _panelAnimator.PlayShow();
    }

    public void Close()
    {
        AudioManager.Instance.PlaySfx(GameSfx.Discard);

        _panelAnimator.PlayHide(() =>
        {
            _background.SetActive(false);
            gameObject.SetActive(false);
        });
    }

    private void RefreshScore()
    {
        _winValueText.text = _personalScoreManager.WinCount.ToString();
        _loseValueText.text = _personalScoreManager.LoseCount.ToString();
        _totalScoreValueText.text = _personalScoreManager.TotalScore.ToString();
    }
}