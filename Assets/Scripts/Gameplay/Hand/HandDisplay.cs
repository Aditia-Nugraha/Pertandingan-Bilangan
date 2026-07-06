using UnityEngine;

public class HandDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HandManager _handManager;
    [SerializeField] private CardDisplay[] _cardSlots;

    [Header("Display")]
    [SerializeField] private PlayerSide _playerSide;
    [SerializeField] private Sprite _closedCardSprite;

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
}