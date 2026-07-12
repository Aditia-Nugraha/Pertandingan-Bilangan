using UnityEngine;
using System.Collections.Generic;

public class HandDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HandManager _handManager;
    [SerializeField] private CardDisplay[] _cardSlots;

    [Header("Display")]
    [SerializeField] private PlayerSide _playerSide;
    [SerializeField] private Sprite _closedCardSprite;
    public Sprite ClosedCardSprite => _closedCardSprite;
    public int CardCount => _handManager.Hand.Count;

    private bool IsCurrentViewer()
    {
        return _playerSide == PlayerProfile.CurrentViewingSide;
    }

    public void RefreshHand()
    {
        bool showFront = IsCurrentViewer();

        for (int i = 0; i < _cardSlots.Length; i++)
        {
            if (i >= _handManager.Hand.Count)
            {
                _cardSlots[i].ClearCard();
                continue;
            }

            CardData card = _handManager.Hand[i];

            if (card == null)
            {
                _cardSlots[i].ClearCard();
                continue;
            }

            if (showFront)
            {
                _cardSlots[i].SetCard(card);
            }
            else
            {
                _cardSlots[i].SetSprite(_closedCardSprite);
            }
        }
    }

    public void HideSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _cardSlots.Length)
        {
            return;
        }

        _cardSlots[slotIndex].HideImage();
    }

    public void ShowSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _cardSlots.Length)
        {
            return;
        }

        _cardSlots[slotIndex].ShowImage();
    }

    public RectTransform GetSlotTransform(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _cardSlots.Length)
        {
            return null;
        }

        return _cardSlots[slotIndex].GetComponent<RectTransform>();
    }

    public CardDisplay GetCardDisplay(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _cardSlots.Length)
        {
            return null;
        }

        return _cardSlots[slotIndex];
    }

    public CardData GetCard(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _handManager.Hand.Count)
        {
            return null;
        }

        return _handManager.Hand[slotIndex];
    }

    public Sprite GetDisplaySprite(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _handManager.Hand.Count)
        {
            return null;
        }

        CardData card = _handManager.Hand[slotIndex];

        if (card == null)
        {
            return null;
        }

        if (IsCurrentViewer())
        {
            return card.CardSprite;
        }

        return _closedCardSprite;
    }

    public void HideAllSlots()
    {
        foreach (CardDisplay slot in _cardSlots)
        {
            slot.ClearCard();
        }
    }

    public List<HandReorderData> CreateSnapshot()
    {
        List<HandReorderData> snapshot = new();

        for (int i = 0; i < _handManager.Hand.Count; i++)
        {
            snapshot.Add(new HandReorderData
            {
                SlotIndex = i,
                Card = _handManager.Hand[i]
            });
        }

        return snapshot;
    }
}