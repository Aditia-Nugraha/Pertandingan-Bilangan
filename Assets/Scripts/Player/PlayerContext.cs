using UnityEngine;

public class PlayerContext : MonoBehaviour
{
    [Header("Identity")]
    [SerializeField] private PlayerSide _playerSide;
    public PlayerSide PlayerSide => _playerSide;

    [Header("Status")]
    [SerializeField] private PlayerStatus _status = new();
    public PlayerStatus Status => _status;

    [Header("Gameplay")]
    [SerializeField] private DeckManager _deckManager;
    public DeckManager DeckManager => _deckManager;

    [SerializeField] private HandManager _handManager;
    public HandManager HandManager => _handManager;

    [SerializeField] private TemporaryCardData _temporaryCard = new();
    public TemporaryCardData TemporaryCard => _temporaryCard;

    [Header("Controller")]
    [SerializeField] private HumanController _humanController;
    public HumanController HumanController => _humanController;

    [Header("Display")]
    [SerializeField] private HandDisplay _handDisplay;
    public HandDisplay HandDisplay => _handDisplay;

    [SerializeField] private SelectedCardDisplay _selectedCardDisplay;
    public SelectedCardDisplay SelectedCardDisplay => _selectedCardDisplay;

    [SerializeField] private StatusDisplay _statusDisplay;
    public StatusDisplay StatusDisplay => _statusDisplay;

    [Header("Animation")]
    [SerializeField] private CardFlipManager _cardFlipManager;
    public CardFlipManager CardFlipManager => _cardFlipManager;
}