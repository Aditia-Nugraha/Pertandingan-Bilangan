using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleMenuController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField _playerNameInput;

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
        SceneManager.LoadScene("GameScene");
    }

    public void StartPlayerVsPlayer()
    {
        
    }
}