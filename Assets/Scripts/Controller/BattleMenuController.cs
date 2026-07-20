using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BattleMenuController : MonoBehaviour
{
    [SerializeField] private TMP_InputField _playerNameInput;
    [SerializeField] private HandshakeController _handshakeController;
    [SerializeField] private MultiplayerLobbyPanel _lobbyPanel;
    [SerializeField] private float _hostTimeout = 3f;
    private Coroutine _hostRoutine;
    private bool _isJoining;

    private void OnEnable()
    {
        _handshakeController.HandshakeCompleted += HandleHandshakeCompleted;
        NetworkManager.Instance.ConnectionFailed += HandleConnectionFailed;
        LanDiscovery.Instance.HostFound += HandleHostFound;
    }

    private void OnDisable()
    {
        if (_handshakeController == null)
        {
            return;
        }

        _handshakeController.HandshakeCompleted -= HandleHandshakeCompleted;
        NetworkManager.Instance.ConnectionFailed -= HandleConnectionFailed;
        LanDiscovery.Instance.HostFound -= HandleHostFound;
    }

    private void HandleHandshakeCompleted()
    {
        if (NetworkSession.Role == PlayerRole.Client)
        {
            LanDiscovery.Instance.StopDiscovery();
        }

        LanDiscovery.Instance.StopListening();

        _lobbyPanel.ShowPlayerFound(
            PlayerProfile.Player2Name,
            NetworkSession.Role == PlayerRole.Host);

        AudioManager.Instance.PlaySfx(GameSfx.BattleWin);
    }

    private void HandleHostFound(string ip)
    {
        if (_isJoining)
        {
            return;
        }

        if (_hostRoutine != null)
        {
            StopCoroutine(_hostRoutine);
            _hostRoutine = null;
        }

        _isJoining = true;
        LanDiscovery.Instance.StopListening();
        NetworkManager.Instance.Join(ip);
    }

    private void HandleConnectionFailed()
    {
        NetworkSession.Role = PlayerRole.Host;
        NetworkManager.Instance.Host();
    }

    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void StartPlayerVsComputer()
    {
        string playerName = _playerNameInput.text.Trim();

        PlayerProfile.Player1Name =
            string.IsNullOrEmpty(playerName)
            ? "Player 1"
            : playerName;

        PlayerProfile.Player2Name = "Computer";
        PlayerProfile.CurrentGameMode = GameMode.PlayerVsComputer;
        PlayerProfile.LocalPlayerSide = PlayerSide.Player1;
        SceneManager.LoadScene("GameScene");
    }

    public void StartPlayerVsPlayer()
    {
        string playerName = _playerNameInput.text.Trim();

        PlayerProfile.Player1Name =
            string.IsNullOrEmpty(playerName)
            ? "Player 1"
            : playerName;

        PlayerProfile.CurrentGameMode = GameMode.PlayerVsPlayer;
        PlayerProfile.LocalPlayerSide = PlayerSide.Player1;

        _lobbyPanel.ShowSearching();
        _isJoining = false;
        NetworkSession.Role = PlayerRole.Client;
        LanDiscovery.Instance.StartListening();
        _hostRoutine = StartCoroutine(HostTimeoutRoutine());
    }

    private IEnumerator HostTimeoutRoutine()
    {
        yield return new WaitForSeconds(_hostTimeout);

        if (_isJoining)
        {
            yield break;
        }

        LanDiscovery.Instance.StopListening();
        NetworkSession.Role = PlayerRole.Host;
        NetworkManager.Instance.Host();
        LanDiscovery.Instance.StartDiscovery();
        Debug.Log("No Host Found -> Become Host");
    }

    public void HostGame()
    {
        NetworkSession.Role = PlayerRole.Host;
        NetworkManager.Instance.Host();
    }

    public void CancelSearching()
    {
        if (_hostRoutine != null)
        {
            StopCoroutine(_hostRoutine);
            _hostRoutine = null;
        }

        LanDiscovery.Instance.StopListening();
        LanDiscovery.Instance.StopDiscovery();
        NetworkManager.Instance.Disconnect();
        NetworkSession.Role = PlayerRole.None;
        _isJoining = false;
    }

    public void StartBattle()
    {
        if (NetworkSession.Role != PlayerRole.Host)
        {
            return;
        }

        NetworkManager.Instance.Send(NetworkCommand.StartGame);

        StartGame();
    }

    private void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}