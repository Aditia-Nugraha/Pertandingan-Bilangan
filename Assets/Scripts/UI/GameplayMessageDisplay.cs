using TMPro;
using UnityEngine;
using System.Collections;

public class GameplayMessageDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _messageTextPlayer1;
    [SerializeField] private TMP_Text _messageTextPlayer2;
    [SerializeField] private float _temporaryDuration = 2f;
    private Coroutine _hideCoroutine;

    public void Show(GameplayMessage message)
    {
        if (_hideCoroutine != null)
        {
            StopCoroutine(_hideCoroutine);
            _hideCoroutine = null;
        }

        gameObject.SetActive(true);

        switch (message)
        {
            case GameplayMessage.ReplaceCard:
                _messageTextPlayer1.text = "Pilih 1 kartu untuk ditukar!";
                break;

            case GameplayMessage.NotEnoughEnergy:
                _messageTextPlayer1.text = "Energy tidak cukup!";
                _hideCoroutine = StartCoroutine(HideAfterDelay());
                break;

            case GameplayMessage.Draw:
                _messageTextPlayer1.text = "1 kartu ditambahkan!";
                _hideCoroutine = StartCoroutine(HideAfterDelay());
                break;

            case GameplayMessage.OpponentDraw:
                _messageTextPlayer2.text = $"{PlayerProfile.Player2Name} menambah 1 kartu!";
                _hideCoroutine = StartCoroutine(HideAfterDelay());
                break;

            case GameplayMessage.Heal:
                _messageTextPlayer1.text = $"+50 HP ditambahkan!";
                _hideCoroutine = StartCoroutine(HideAfterDelay());
                break;

            case GameplayMessage.HPFull:
                _messageTextPlayer1.text = $"HP sudah penuh!";
                _hideCoroutine = StartCoroutine(HideAfterDelay());
                break;

            case GameplayMessage.OpponentHeal:
                _messageTextPlayer2.text = $"{PlayerProfile.Player2Name} menambah +50 HP!";
                _hideCoroutine = StartCoroutine(HideAfterDelay());
                break;

            case GameplayMessage.Player1ChoseCard:
                _messageTextPlayer1.text = $"Kamu belum memilih kartu!";
                _hideCoroutine = StartCoroutine(HideAfterDelay());
                break;

            case GameplayMessage.Player2ChoseCard:
                _messageTextPlayer2.text = $"{PlayerProfile.Player2Name} belum memilih kartu!";
                _hideCoroutine = StartCoroutine(HideAfterDelay());
                break;

            default:
                Hide();
                break;
        }
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(_temporaryDuration);
        Hide();
    }

    public void Hide()
    {
        if (_hideCoroutine != null)
        {
            StopCoroutine(_hideCoroutine);
            _hideCoroutine = null;
        }

        _messageTextPlayer1.text = "";
        _messageTextPlayer2.text = "";

        gameObject.SetActive(false);
    }
}