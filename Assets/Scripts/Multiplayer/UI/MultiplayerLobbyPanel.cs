using TMPro;
using UnityEngine;

public class MultiplayerLobbyPanel : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject _background;
    [SerializeField] private GameObject _loadingPanel;
    [SerializeField] private GameObject _foundPanel;

    [Header("Action")]
    [SerializeField] private GameObject _hostAction;
    [SerializeField] private GameObject _clientAction;
    [SerializeField] private BattleMenuController _battleMenuController;

    [Header("UI")]
    [SerializeField] private TMP_Text _playerNameText;

    [Header("Animation")]
    [SerializeField] private ScalePanelAnimator _panelAnimator;
    [SerializeField] private RectTransform _loadingIcon;
    [SerializeField] private float _rotationSpeed = 180f;

    private bool _isSearching;

    private void Update()
    {
        if (!_isSearching)
        {
            return;
        }

        _loadingIcon.Rotate(0f, 0f, -_rotationSpeed * Time.unscaledDeltaTime);
    }

    public void ShowSearching()
    {
        gameObject.SetActive(true);
        _background.SetActive(true);
        _loadingPanel.SetActive(true);
        _foundPanel.SetActive(false);
        _isSearching = true;
        _panelAnimator.PlayShow();
    }

    public void ShowPlayerFound(string playerName, bool isHost)
    {
        _isSearching = false;
        _loadingPanel.SetActive(false);
        _foundPanel.SetActive(true);
        _playerNameText.text = playerName;
        _hostAction.SetActive(isHost);
        _clientAction.SetActive(!isHost);
    }

    public void CloseSearching()
    {
        AudioManager.Instance.PlaySfx(GameSfx.Discard);

        _panelAnimator.PlayHide(() =>
        {
            _background.SetActive(false);
            gameObject.SetActive(false);
            _battleMenuController.CancelSearching();
        });
    }
}