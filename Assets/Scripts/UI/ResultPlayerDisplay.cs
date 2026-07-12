using TMPro;
using UnityEngine;

public class ResultPlayerDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private CardDisplay _cardDisplay;
    [SerializeField] private TMP_Text _valueText;
    [SerializeField] private ResultCardFlipAnimator _cardFlipAnimator;
    [SerializeField] private ResultValueAnimator _valueAnimator;

    private CardData _card;

    public void SetPlayer(string playerName, CardData card)
    {
        _card = card;
        _nameText.text = playerName;
        _valueText.text = "";
        _cardFlipAnimator.ShowCardBack();
    }

    public void PlayFlip(System.Action onFinished = null)
    {
        _cardFlipAnimator.Play(_card, onFinished);
    }

    public void PlayValue(System.Action onFinished = null)
    {
        _valueAnimator.Play(_card.Value, onFinished);
    }
}