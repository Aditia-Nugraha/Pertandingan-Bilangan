using UnityEngine;

public class ReplaceController : MonoBehaviour
{
    [SerializeField] private PlayerContext _player;
    [SerializeField] private GameplayStateManager _stateManager;
    [SerializeField] private GameplayMessageDisplay _messageDisplay;

    [Header("Animation")]
    [SerializeField] private CardTransitionManager _transitionManager;
    [SerializeField] private CardDestroyManager _destroyManager;

    public bool IsReplacing()
    {
        return _stateManager.IsState(GameplayState.ReplaceCard);
    }

    public void Replace(int handIndex)
    {
        if (!IsReplacing())
        {
            return;
        }

        if (!_player.TemporaryCard.HasCard)
        {
            return;
        }

        AudioManager.Instance.PlaySfx(GameSfx.CardMove);
        _stateManager.SetState(GameplayState.Busy);

        CardData oldCard = _player.HandManager.Hand[handIndex];
        CardData newCard = _player.TemporaryCard.Card;

        RectTransform from = _player.SelectedCardDisplay.GetSlotTransform();
        RectTransform to = _player.HandDisplay.GetSlotTransform(handIndex);

        _player.HandDisplay.HideSlot(handIndex);
        _player.SelectedCardDisplay.HideImage();

        int completed = 0;

        void FinishOne()
        {
            completed++;

            if (completed >= 2)
            {
                FinishReplace(handIndex);
            }
        }

        _transitionManager.PlaySingle(
            newCard.CardSprite,
            from,
            to,
            FinishOne);

        _destroyManager.Play(
            oldCard.CardSprite,
            to,
            FinishOne);
    }

    private void FinishReplace(int handIndex)
    {
        _player.HandManager.ReplaceCard(handIndex, _player.TemporaryCard.Card);
        _player.TemporaryCard.Clear();
        _player.HandDisplay.RefreshHand();
        _player.SelectedCardDisplay.Refresh();
        _player.HandDisplay.ShowSlot(handIndex);
        _messageDisplay.Hide();
        _stateManager.SetState(GameplayState.Normal);
    }
}