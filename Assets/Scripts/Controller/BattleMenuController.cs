using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleMenuController : MonoBehaviour
{
    [SerializeField] private TMP_InputField _playerNameInput;
    [SerializeField] private HandshakeController _handshakeController;
    [SerializeField] private MultiplayerLobbyPanel _lobbyPanel;
    [SerializeField] private string _hostIp = "127.0.0.1";

    private void OnEnable()
    {
        _handshakeController.HandshakeCompleted += HandleHandshakeCompleted;
        NetworkManager.Instance.ConnectionFailed += HandleConnectionFailed;
    }

    private void OnDisable()
    {
        if (_handshakeController == null)
        {
            return;
        }

        _handshakeController.HandshakeCompleted -= HandleHandshakeCompleted;
        NetworkManager.Instance.ConnectionFailed -= HandleConnectionFailed;
    }

    private void HandleHandshakeCompleted()
    {
        _lobbyPanel.ShowPlayerFound(
            PlayerProfile.Player2Name,
            NetworkSession.Role == PlayerRole.Host);
            AudioManager.Instance.PlaySfx(GameSfx.BattleWin);
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
        NetworkSession.Role = PlayerRole.Client;
        NetworkManager.Instance.Join(_hostIp);
    }

    public void AutoConnect()
    {
        NetworkSession.Role = PlayerRole.Client;
        NetworkManager.Instance.Join(_hostIp);
    }

    public void HostGame()
    {
        NetworkSession.Role = PlayerRole.Host;
        NetworkManager.Instance.Host();
    }

    public void JoinGame()
    {
        NetworkSession.Role = PlayerRole.Client;
        NetworkManager.Instance.Join(_hostIp);
    }

    public void CancelSearching()
    {
        NetworkSession.Role = PlayerRole.None;
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