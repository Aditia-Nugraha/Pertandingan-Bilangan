using TMPro;
using UnityEngine;
using System.Collections;

public class GameplayMessageDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _messageTextPlayer1;
    [SerializeField] private TMP_Text _messageTextPlayer2;
    [SerializeField] private GameplayMessageAnimator _player1Animator;
    [SerializeField] private GameplayMessageAnimator _player2Animator;

    public void Show(GameplayMessage message)
    {
        gameObject.SetActive(true);
        switch (message)
        {
            case GameplayMessage.ReplaceCard:
                ShowPlayer1Message("Pilih 1 kartu untuk ditukar!", false);
                break;

            case GameplayMessage.NotEnoughEnergy:
                ShowPlayer1Message("Energy tidak cukup!");
                break;

            case GameplayMessage.Draw:
                ShowPlayer1Message("kartu ditambahkan!");
                break;

            case GameplayMessage.OpponentDraw:
                ShowPlayer2Message($"{PlayerProfile.Player2Name} menambahkan kartu!");
                break;

            case GameplayMessage.Heal:
                ShowPlayer1Message($"+50 HP ditambahkan!");
                break;

            case GameplayMessage.HPFull:
                ShowPlayer1Message($"HP sudah penuh!");
                break;

            case GameplayMessage.OpponentHeal:
                ShowPlayer2Message($"{PlayerProfile.Player2Name} menambah +50 HP!");
                break;

            case GameplayMessage.Player1ChoseCard:
                ShowPlayer1Message($"Kamu belum memilih kartu!");
                break;

            case GameplayMessage.Player2ChoseCard:
                ShowPlayer2Message($"{PlayerProfile.Player2Name} belum memilih kartu!");
                break;

            default:
                Hide();
                break;
        }
    }

    public void Hide()
    {
        HidePlayer1Message();
    }

    private void ShowPlayer1Message(string message, bool temporary = true)
    {
        _messageTextPlayer1.text = message;
        _messageTextPlayer1.alpha = 1f;

        if (temporary)
        {
            _player1Animator.Play();
        }
    }

    private void ShowPlayer2Message(string message)
    {
        _messageTextPlayer2.text = message;
        _player2Animator.Play();
    }

    private void HidePlayer1Message()
    {
        _player1Animator.PlayHide();
    }
}