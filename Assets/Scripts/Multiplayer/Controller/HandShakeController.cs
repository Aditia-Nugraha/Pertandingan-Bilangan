using UnityEngine;
using UnityEngine.SceneManagement;

public class HandshakeController : MonoBehaviour
{
    public event System.Action HandshakeCompleted;

    private void OnEnable()
    {
        NetworkManager.Instance.Connected += HandleConnected;
        NetworkManager.Instance.PacketReceived += HandlePacketReceived;
    }

    private void OnDisable()
    {
        if (NetworkManager.Instance == null)
        {
            return;
        }

        NetworkManager.Instance.Connected -= HandleConnected;
        NetworkManager.Instance.PacketReceived -= HandlePacketReceived;
    }

    private void HandleConnected()
    {
        if (NetworkSession.Role != PlayerRole.Client)
        {
            return;
        }

        NetworkManager.Instance.Send(
            NetworkCommand.Hello,
            PlayerProfile.Player1Name);
    }

    private void HandlePacketReceived(NetworkPacket packet)
    {
        switch (packet.Command)
        {
            case NetworkCommand.Hello:
                HandleHello(packet);
                break;

            case NetworkCommand.HelloResponse:
                HandleHelloResponse(packet);
                break;

            case NetworkCommand.StartGame:
                HandleStartGame();
                break;
        }
    }

    private void HandleHello(NetworkPacket packet)
    {
        if (NetworkSession.Role != PlayerRole.Host)
        {
            return;
        }

        PlayerProfile.Player2Name = packet.Data;
        NetworkManager.Instance.Send(
            NetworkCommand.HelloResponse,
            PlayerProfile.Player1Name);

        HandshakeCompleted?.Invoke();
    }

    private void HandleHelloResponse(NetworkPacket packet)
    {
        if (NetworkSession.Role != PlayerRole.Client)
        {
            return;
        }

        PlayerProfile.Player2Name = packet.Data;
        HandshakeCompleted?.Invoke();
    }

    private void HandleStartGame()
    {
        if (NetworkSession.Role != PlayerRole.Client)
        {
            return;
        }

        SceneManager.LoadScene("GameScene");
    }
}