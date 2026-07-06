using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private TMP_InputField _playerNameInput;

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadBattleMenu()
    {
        SceneManager.LoadScene("BattleMenu");
    }

    public void LoadPlayerVsComputer()
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
}