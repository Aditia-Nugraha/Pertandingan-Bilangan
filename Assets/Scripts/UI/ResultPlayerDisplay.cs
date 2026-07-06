using TMPro;
using UnityEngine;

public class ResultPlayerDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private CardDisplay _cardDisplay;
    [SerializeField] private TMP_Text _valueText;

    public void SetPlayer(string playerName, CardData card)
    {
        _nameText.text = playerName;
        _cardDisplay.SetCard(card);
        _valueText.text = card.Value.ToString("0.000");
    }
}