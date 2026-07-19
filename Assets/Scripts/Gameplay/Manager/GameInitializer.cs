using UnityEngine;

public class GameInitializer : MonoBehaviour
{

    [Header("Player 1")]
    [SerializeField] private PlayerContext _player1;
    [SerializeField] private DeckManager _player1Deck;
    [SerializeField] private HandManager _player1Hand;
    [SerializeField] private HandDisplay _player1Display;
    [SerializeField] private SelectedCardDisplay _player1SelectedCardDisplay;
    [SerializeField] private StatusDisplay _player1StatusDisplay;

    [Header("Player 2")]
    [SerializeField] private PlayerContext _player2;
    [SerializeField] private DeckManager _player2Deck;
    [SerializeField] private HandManager _player2Hand;
    [SerializeField] private HandDisplay _player2Display;
    [SerializeField] private SelectedCardDisplay _player2SelectedCardDisplay;
    [SerializeField] private StatusDisplay _player2StatusDisplay;

    [Header("Game")]
    [SerializeField] private RoundDisplay _roundDisplay;
    [SerializeField] private GameplayStateManager _stateManager;
    [SerializeField] private DealAnimationController _dealAnimation;

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        //Gameplay State
        _stateManager.SetState(GameplayState.Normal);
        
        // Player
        _player1.Status.Reset();
        _player2.Status.Reset();

        // Deck
        _player1Deck.InitializeDeck();
        _player2Deck.InitializeDeck();

        // Hand
        _player1Hand.DrawStartingHand();
        _player2Hand.DrawStartingHand();

        // Hand UI
        _player1Display.HideAllSlots();
        _player2Display.HideAllSlots();

        //Selected Card Display
        _player1SelectedCardDisplay.Hide();
        _player2SelectedCardDisplay.Hide();

        // Status UI
        _player1StatusDisplay.Refresh();
        _player2StatusDisplay.Refresh();

        // Round
        _roundDisplay.Refresh();

        //Opening Animation
        _dealAnimation.PlayOpeningAnimation();
    }
}