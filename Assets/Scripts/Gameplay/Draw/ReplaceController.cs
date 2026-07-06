using UnityEngine;

public class ReplaceController : MonoBehaviour
{
    [SerializeField] private PlayerContext _player;
    [SerializeField] private GameplayStateManager _stateManager;
    [SerializeField] private GameplayMessageDisplay _messageDisplay;

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

        _player.HandManager.ReplaceCard(handIndex, _player.TemporaryCard.Card);
        _player.TemporaryCard.Clear();
        _player.HandDisplay.RefreshHand();
        _player.SelectedCardDisplay.Refresh();
        _messageDisplay.Hide();
        _stateManager.SetState(GameplayState.Normal);
    }
}