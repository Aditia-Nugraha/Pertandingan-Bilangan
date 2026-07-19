using TMPro;
using UnityEngine;
using System.Collections;

public class GameplayMessageDisplay : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TMP_Text _bottomMessageText;
    [SerializeField] private TMP_Text _topMessageText;

    [Header("Animation")]
    [SerializeField] private GameplayMessageAnimator _bottomMessageAnimator;
    [SerializeField] private GameplayMessageAnimator _topMessageAnimator;

    private string OpponentName =>
    PlayerProfile.LocalPlayerSide == PlayerSide.Player1
        ? PlayerProfile.Player2Name
        : PlayerProfile.Player1Name;

    public void Show(GameplayMessage message)
    {
        gameObject.SetActive(true);
        switch (message)
        {
            case GameplayMessage.ReplaceCard:
                ShowBottomMessage("Pilih 1 kartu untuk ditukar!", false);
                break;

            case GameplayMessage.NotEnoughEnergy:
                ShowBottomMessage("Energy tidak cukup!");
                break;

            case GameplayMessage.Draw:
                ShowBottomMessage("kartu ditambahkan!");
                break;

            case GameplayMessage.OpponentDraw:
                ShowTopMessage($"{OpponentName} menambahkan kartu!");
                break;

            case GameplayMessage.Heal:
                ShowBottomMessage($"+50 HP ditambahkan!");
                break;

            case GameplayMessage.HPFull:
                ShowBottomMessage($"HP sudah penuh!");
                break;

            case GameplayMessage.OpponentHeal:
                ShowTopMessage($"{OpponentName} menambah +50 HP!");
                break;

            case GameplayMessage.Player1ChoseCard:
                ShowBottomMessage($"Kamu belum memilih kartu!");
                break;

            case GameplayMessage.Player2ChoseCard:
                ShowTopMessage($"{OpponentName} belum memilih kartu!");
                break;

            default:
                Hide();
                break;
        }
    }

    public void Hide()
    {
        HideBottomMessage();
    }

    private void ShowBottomMessage(string message, bool temporary = true)
    {
        _bottomMessageText.text = message;
        _bottomMessageText.alpha = 1f;

        if (temporary)
        {
            _bottomMessageAnimator.Play();
        }
    }

    private void ShowTopMessage(string message)
    {
        _topMessageText.text = message;
        _topMessageAnimator.Play();
    }

    private void HideBottomMessage()
    {
        _bottomMessageAnimator.PlayHide();
    }
}