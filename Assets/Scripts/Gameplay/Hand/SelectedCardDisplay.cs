using UnityEngine;

public class SelectedCardDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerContext _player;
    [SerializeField] private CardDisplay _cardDisplay;

    [Header("Display")]
    [SerializeField] private PlayerSide _playerSide;
    [SerializeField] private Sprite _closedCardSprite;
    private bool _isRevealed;

    private void Start()
    {
        Refresh();
    }

    private void OnEnable()
    {
        _player.HandManager.OnCardSelected += Refresh;
    }

    private void OnDisable()
    {
        _player.HandManager.OnCardSelected -= Refresh;
    }

    private bool IsCurrentViewer()
    {
        return _playerSide == PlayerProfile.CurrentViewingSide;
    }

    public void Refresh()
    {
        if (_player.TemporaryCard.HasCard)
        {
            _cardDisplay.SetCard(_player.TemporaryCard.Card);
            return;
        }
        
        if (!_player.HandManager.HasSelectedCard())
        {
            _cardDisplay.ClearCard();
            return;
        }

        if (_isRevealed || IsCurrentViewer())
        {
            _cardDisplay.SetCard(_player.HandManager.SelectedCard.Card);
        }
        else
        {
            _cardDisplay.SetSprite(_closedCardSprite);
        }
    }

    public void Reveal()
    {
        _isRevealed = true;
        Refresh();
    }

    public void Hide()
    {
        _isRevealed = false;
        Refresh();
    }

    public void Clear()
    {
        _cardDisplay.ClearCard();
    }
}