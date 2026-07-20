using UnityEngine;
using UnityEngine.SceneManagement;

public class DisconnectPopup : MonoBehaviour
{
    [Header("Controller")]
    [SerializeField] private GameplaySyncController _gameplaySyncController;

    [Header("Panel")]
    [SerializeField] private GameObject _container;
    [SerializeField] private GameObject _background;
    [SerializeField] private GameObject _panel;

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
    }

    public void Hide()
    {
        _panel.SetActive(false);
        _background.SetActive(false);
        _container.SetActive(false);
    }

    public void BackToBattleMenu()
    {
        _gameplaySyncController.IgnoreNextDisconnect();
        LanDiscovery.Instance.StopListening();
        LanDiscovery.Instance.StopDiscovery();
        NetworkManager.Instance.Disconnect();
        NetworkSession.Role = PlayerRole.None;
        SceneManager.LoadScene("BattleMenu");
    }
}