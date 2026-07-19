using UnityEngine;

public class HumanController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HandManager _handManager;
    [SerializeField] private HandDisplay _handDisplay;
    [SerializeField] private SelectedCardDisplay _selectedCardDisplay;
    [SerializeField] private GameplayStateManager _stateManager;
    [SerializeField] private ReplaceController _replaceController;
    [SerializeField] private CardTransitionManager _transitionManager;
    [SerializeField] private GameplaySyncController _syncController;

    public void SelectCard(int slotIndex)
    {
        switch (_stateManager.CurrentState)
        {

            case GameplayState.Normal:
                PlaySelectAnimation(slotIndex);
                break;

            case GameplayState.ReplaceCard:
                _replaceController.Replace(slotIndex);
                break;

            case GameplayState.Busy:
                default:
                    return;
        }
    }

    private void SelectBattleCard(int slotIndex)
    {
        _handManager.SelectCard(slotIndex);
        _handDisplay.RefreshHand();
        _selectedCardDisplay.Refresh();
    }

    private void PlaySelectAnimation(int slotIndex)
    {
        if (_handManager.HasSelectedCard())
        {
            PlayReplaceAnimation(slotIndex);
            AudioManager.Instance.PlaySfx(GameSfx.CardMove);
            return;
        }

        CardData card = _handDisplay.GetCard(slotIndex);

        if (card == null)
        {
            return;
        }

        RectTransform from = _handDisplay.GetSlotTransform(slotIndex);
        RectTransform to = _selectedCardDisplay.GetSlotTransform();

        _handDisplay.HideSlot(slotIndex);
        _transitionManager.PlaySingle(card.CardSprite, from, to, () =>
        {
            SelectBattleCard(slotIndex);
        });
        AudioManager.Instance.PlaySfx(GameSfx.CardMove);
    }

    private void PlayReplaceAnimation(int slotIndex)
    {
        CardData newCard = _handDisplay.GetCard(slotIndex);

        if (newCard == null)
        {
            return;
        }

        CardData oldCard = _handManager.SelectedCard.Card;
        RectTransform oldFrom = _selectedCardDisplay.GetSlotTransform();
        RectTransform oldTo = _handDisplay.GetSlotTransform(_handManager.SelectedCard.OriginalSlotIndex);
        RectTransform newFrom = _handDisplay.GetSlotTransform(slotIndex);
        RectTransform newTo = _selectedCardDisplay.GetSlotTransform();
        
        _selectedCardDisplay.Clear();
        _handDisplay.HideSlot(slotIndex);
        _transitionManager.PlayReplace(
            oldCard.CardSprite,
            oldFrom,
            oldTo,
            newCard.CardSprite,
            newFrom,
            newTo,
            () =>
            {
                SelectBattleCard(slotIndex);
            });
    }

    public void PlayReturnAnimation(System.Action onFinished)
    {
        if (!_handManager.HasSelectedCard())
        {
            onFinished?.Invoke();
            return;
        }

        CardData selectedCard = _handManager.SelectedCard.Card;

        RectTransform from = _selectedCardDisplay.GetSlotTransform();
        RectTransform to = _handDisplay.GetSlotTransform(_handManager.SelectedCard.OriginalSlotIndex);

        _selectedCardDisplay.Clear();

        _transitionManager.PlayReturn(
            selectedCard.CardSprite,
            from,
            to,
            () =>
            {
                _handManager.RestoreSelectedCard();
                _handDisplay.RefreshHand();
                _selectedCardDisplay.Refresh();
                _syncController.SendReturnCard();
                onFinished?.Invoke();
            });
    }

    public void PlayDrawAnimation(int newSlotIndex, System.Action onFinished)
    {
        if (!_handManager.HasSelectedCard())
        {
            onFinished?.Invoke();
            return;
        }

        CardData selectedCard = _handManager.SelectedCard.Card;

        RectTransform selectedFrom = _selectedCardDisplay.GetSlotTransform();
        RectTransform selectedTo = _handDisplay.GetSlotTransform(_handManager.SelectedCard.OriginalSlotIndex);

        RectTransform drawFrom = _selectedCardDisplay.GetSlotTransform();
        RectTransform drawTo = _handDisplay.GetSlotTransform(newSlotIndex);

        _selectedCardDisplay.Clear();

        _transitionManager.PlayDraw(
            selectedCard.CardSprite,
            selectedFrom,
            selectedTo,

            _handDisplay.ClosedCardSprite,
            drawFrom,
            drawTo,

            () =>
            {
                _handManager.RestoreSelectedCard();
                _handDisplay.RefreshHand();
                _selectedCardDisplay.Refresh();
                onFinished?.Invoke();
            });
    }
}