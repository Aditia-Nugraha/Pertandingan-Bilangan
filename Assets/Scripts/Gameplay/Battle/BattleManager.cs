using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] private PlayerContext _player1;
    [SerializeField] private PlayerContext _player2;

    [Header("Round")]
    [SerializeField] private RoundManager _roundManager;
    
    [Header("Result Panel")]
    [SerializeField] private ResultPanel _resultPanel;

    [Header("Message Display")]
    [SerializeField] private GameplayMessageDisplay _messageDisplay;

    private BattleResultData _lastBattleResult = new();
    public BattleResultData LastBattleResult => _lastBattleResult;

    public void StartBattle()
    {
        if (!_player1.HandManager.HasSelectedCard())
        {
            _messageDisplay.Show(GameplayMessage.Player1ChoseCard);
            return;
        }

        if (!_player2.HandManager.HasSelectedCard())
        {
            _messageDisplay.Show(GameplayMessage.Player2ChoseCard);
            return;
        }

        _player1.SelectedCardDisplay.Reveal();
        _player2.SelectedCardDisplay.Reveal();
        BattleResultData result = ResolveBattle();
        _resultPanel.Show();
    }

    private BattleResultData ResolveBattle()
    {
        CardData player1Card = _player1.HandManager.SelectedCard.Card;
        CardData player2Card = _player2.HandManager.SelectedCard.Card;

        _lastBattleResult = new BattleResultData();
        _lastBattleResult.Round = _roundManager.CurrentRound;
        _lastBattleResult.Player1Card = player1Card;
        _lastBattleResult.Player2Card = player2Card;

        if (player1Card.Value > player2Card.Value)
        {
            _lastBattleResult.Outcome = BattleOutcome.Win;
        }
        else if (player1Card.Value < player2Card.Value)
        {
            _lastBattleResult.Outcome = BattleOutcome.Lose;
        }
        else
        {
            _lastBattleResult.Outcome = BattleOutcome.Draw;
        }

        switch (_lastBattleResult.Outcome)
        {
            case BattleOutcome.Win:
                _lastBattleResult.Player1.PrimaryReward = PrimaryReward.Attack;
                _lastBattleResult.Player2.PrimaryReward = PrimaryReward.Health;

                _lastBattleResult.Player1.AttackReward = player1Card.Attack;
                _lastBattleResult.Player2.AttackReward = 0;

                _lastBattleResult.Player1.HpChange = 0;
                _lastBattleResult.Player2.HpChange = -player1Card.Attack;

                _lastBattleResult.Player1.EnergyChange = 5;
                _lastBattleResult.Player2.EnergyChange = 10;
                break;

            case BattleOutcome.Lose:
                _lastBattleResult.Player1.PrimaryReward = PrimaryReward.Health;
                _lastBattleResult.Player2.PrimaryReward = PrimaryReward.Attack;

                _lastBattleResult.Player1.AttackReward = 0;
                _lastBattleResult.Player2.AttackReward = player2Card.Attack;

                _lastBattleResult.Player1.HpChange = -player2Card.Attack;
                _lastBattleResult.Player2.HpChange = 0;

                _lastBattleResult.Player1.EnergyChange = 10;
                _lastBattleResult.Player2.EnergyChange = 5;
                break;

            case BattleOutcome.Draw:
                _lastBattleResult.Player1.PrimaryReward = PrimaryReward.None;
                _lastBattleResult.Player2.PrimaryReward = PrimaryReward.None;

                _lastBattleResult.Player1.AttackReward = 0;
                _lastBattleResult.Player2.AttackReward = 0;

                _lastBattleResult.Player1.HpChange = 0;
                _lastBattleResult.Player2.HpChange = 0;

                _lastBattleResult.Player1.EnergyChange = 5;
                _lastBattleResult.Player2.EnergyChange = 5;
                break;
        }
        return _lastBattleResult;
    }
}