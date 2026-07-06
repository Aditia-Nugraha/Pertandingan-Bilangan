using System.Collections.Generic;
using UnityEngine;
using System;

public class HandManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DeckManager _deckManager;
    private readonly List<CardData> _handCards = new();
    public IReadOnlyList<CardData> Hand => _handCards;
    public const int MaxHandSize = 5;

    [SerializeField] private SelectedCardData _selectedCard = new SelectedCardData();
    public SelectedCardData SelectedCard => _selectedCard;
    public event Action OnCardSelected;

    public void DrawStartingHand()
    {
        while (_handCards.Count < MaxHandSize)
        {
            CardData card = _deckManager.DrawCard();
            if (card == null)
            {
                break;
            }
            _handCards.Add(card);
        }
    }

    public bool HasSelectedCard()
    {
        return _selectedCard.HasCard;
    }

    public void SelectCard(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _handCards.Count)
        {
            return;
        }
        if (_selectedCard.OriginalSlotIndex == slotIndex)
        {
            return;
        }
        CardData card = _handCards[slotIndex];
        if (card == null)
        {
            return;
        }
        RestoreSelectedCard();
        _selectedCard.Card = card;
        _selectedCard.OriginalSlotIndex = slotIndex;
        _handCards[slotIndex] = null;
        OnCardSelected?.Invoke();
    }

    public void RestoreSelectedCard()
    {
        if (!_selectedCard.HasCard)
        {
            return;
        }
        _handCards[_selectedCard.OriginalSlotIndex] = _selectedCard.Card;
        _selectedCard.Clear();
    }

    public void SelectRandomCard()
    {
        List<int> availableIndexes = new();
        for (int i = 0; i < _handCards.Count; i++)
        {
            if (_handCards[i] != null)
            {
                availableIndexes.Add(i);
            }
        }
        if (availableIndexes.Count == 0)
        {
            return;
        }
        int randomIndex = UnityEngine.Random.Range(0, availableIndexes.Count);
        SelectCard(availableIndexes[randomIndex]);
    }

    public void RemoveSelectedCard()
    {
        if (!HasSelectedCard())
        {
            return;
        }

        _handCards.RemoveAt(_selectedCard.OriginalSlotIndex);

        _selectedCard.Clear();
}

    public bool IsHandFull()
    {
        return Hand.Count >= MaxHandSize;
    }

    public void DrawOneCard()
    {
        CardData card = _deckManager.DrawCard();

        if (card != null)
        {
            _handCards.Add(card);
        }
    }

    public void ReplaceCard(int index, CardData newCard)
    {
        if (index < 0 || index >= _handCards.Count)
        {
            return;
        }

        if (_handCards[index] == null)
        {
            return;
        }
        _handCards[index] = newCard;
    }
}