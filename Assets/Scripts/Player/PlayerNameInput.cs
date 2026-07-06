using TMPro;
using UnityEngine;

public class PlayerNameInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField _playerNameInput;
    private const string DefaultPlayerName = "Player 1";

    private void Start()
    {
        _playerNameInput.text = PlayerProfile.Player1Name;
    }

    public void SavePlayerName()
    {
        string playerName = _playerNameInput.text.Trim();
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = DefaultPlayerName;
        }
        PlayerProfile.Player1Name = playerName;
        _playerNameInput.text = playerName;
    }
}