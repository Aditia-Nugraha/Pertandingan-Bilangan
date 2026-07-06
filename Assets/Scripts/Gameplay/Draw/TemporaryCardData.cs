using UnityEngine;

[System.Serializable]
public class TemporaryCardData
{
    public CardData Card;
    public bool HasCard => Card != null;

    public void SetCard(CardData card)
    {
        Card = card;
    }

    public void Clear()
    {
        Card = null;
    }
}