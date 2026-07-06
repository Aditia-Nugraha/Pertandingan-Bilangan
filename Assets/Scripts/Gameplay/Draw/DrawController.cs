using UnityEngine;

public class DrawController : MonoBehaviour
{
    [SerializeField] private PlayerContext _player;
    [SerializeField] private GameplayStateManager _stateManager;
    [SerializeField] private SelectedCardDisplay _selectedCardDisplay;
    [SerializeField] private GameplayMessageDisplay _messageDisplay;

    private const int DrawCost = 5;

    public void OnDrawButton()
    {
        switch (_stateManager.CurrentState)
        {
            case GameplayState.Normal:
                Draw();
                break;

            case GameplayState.ReplaceCard:
                Discard();
                break;
        }
    }

    public void Draw()
    {
        if (_player.Status.Energy < DrawCost)
        {
            _messageDisplay.Show(GameplayMessage.NotEnoughEnergy);
            return;
        }

        if (_player.HandManager.IsHandFull())
        {
            if (_player.HandManager.HasSelectedCard())
            {
                _player.HandManager.RestoreSelectedCard();
                _player.HandDisplay.RefreshHand();
                _player.SelectedCardDisplay.Refresh();
            }

            _player.Status.Energy -= DrawCost;
            CardData drawnCard = _player.DeckManager.DrawOneCard();

            if (drawnCard == null)
            {
                return;
            }

            _player.TemporaryCard.SetCard(drawnCard);
            _selectedCardDisplay.Refresh();
            _player.StatusDisplay.Refresh();
            _stateManager.SetState(GameplayState.ReplaceCard);
            _messageDisplay.Show(GameplayMessage.ReplaceCard);

            return;
        }

        _player.Status.Energy -= DrawCost;
        _player.HandManager.DrawOneCard();
        _player.HandDisplay.RefreshHand();
        _player.StatusDisplay.Refresh();
        _messageDisplay.Show(GameplayMessage.Draw);
    }

    private void Discard()
    {
        if (!_player.TemporaryCard.HasCard)
        {
            return;
        }

        _player.TemporaryCard.Clear();
        _player.SelectedCardDisplay.Refresh();
        _messageDisplay.Hide();
        _stateManager.SetState(GameplayState.Normal);
    }
}
