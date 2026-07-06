using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CardDatabase _cardDatabase;
    private List<CardData> _deckCards = new();
    public int DeckCount => _deckCards.Count;

    public void InitializeDeck()
    {
        _deckCards.Clear();
        _deckCards.AddRange(_cardDatabase.Cards);
        ShuffleDeck();
    }

    private void CreateDeck()
    {
        _deckCards.AddRange(_cardDatabase.Cards);
        ShuffleDeck();
    }

    private void ShuffleDeck()
    {
        for (int i = _deckCards.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (_deckCards[i], _deckCards[randomIndex]) = (_deckCards[randomIndex], _deckCards[i]);
        }
    }

    public CardData DrawCard()
    {
        if (_deckCards.Count == 0)
        {
            CreateDeck();
        }

        CardData drawnCard = _deckCards[0];
        _deckCards.RemoveAt(0);
        return drawnCard;
    }

    public CardData DrawOneCard()
    {
        return DrawCard();
    }
}