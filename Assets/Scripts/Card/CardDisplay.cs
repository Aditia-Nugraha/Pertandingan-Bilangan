using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] private Image _cardImage;
    private CardData _cardData;
    public CardData CardData => _cardData;
    public Image CardImage => _cardImage;

    public void SetCard(CardData cardData)
    {
        _cardData = cardData;
        _cardImage.enabled = true;
        _cardImage.sprite = cardData.CardSprite;
    }

    public void SetSprite(Sprite sprite)
    {
        _cardData = null;
        _cardImage.enabled = true;
        _cardImage.sprite = sprite;
    }

    public void ClearCard()
    {
        _cardData = null;
        _cardImage.enabled = false;
    }

    public void HideImage()
    {
        _cardImage.enabled = false;
    }

    public void ShowImage()
    {
        _cardImage.enabled = true;
    }
}