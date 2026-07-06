using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ResultPanel : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject _panelRoot;
    [SerializeField] private GameResultPanel _gameResultPanel;

    [Header("Gameplay State")]
    [SerializeField] private GameplayStateManager _stateManager;

    [Header("Battle Manager")]
    [SerializeField] private BattleManager _battleManager;

    [Header("Round")]
    [SerializeField] private RoundManager _roundManager;
    [SerializeField] private RoundDisplay _roundDisplay;
    [SerializeField] private TMP_Text _roundText;
    
    [Header("Players")]
    [SerializeField] private PlayerContext _player1;
    [SerializeField] private PlayerContext _player2;

    [Header("Player Display")]
    [SerializeField] private ResultPlayerDisplay _player1Display;
    [SerializeField] private ResultPlayerDisplay _player2Display;

    [Header("Opponent Controller")]
    [SerializeField] private OpponentActionController _opponentActionController;

    [Header("Result")]
    [SerializeField] private TMP_Text _comparisonText;
    [SerializeField] private TMP_Text _resultText;

    [Header("Primary Reward")]
    [SerializeField] private Image _primaryRewardIcon;
    [SerializeField] private TMP_Text _primaryRewardText;

    [Header("Energy Reward")]
    [SerializeField] private Image _energyRewardIcon;
    [SerializeField] private TMP_Text _energyRewardText;

    [Header("Icon Sprite")]
    [SerializeField] private Sprite _attackIcon;
    [SerializeField] private Sprite _healthIcon;
    [SerializeField] private Sprite _energyIcon;

    public void Show()
    {
        Refresh();
        _stateManager.SetState(GameplayState.BattleResult);
        _panelRoot.SetActive(true);
    }

    public void Hide()
    {
        _panelRoot.SetActive(false);
    }

    private void Refresh()
    {
        BattleResultData result = _battleManager.LastBattleResult;
        _roundText.text = $"Ronde {result.Round}";

        _player1Display.SetPlayer(PlayerProfile.Player1Name, result.Player1Card);
        _player2Display.SetPlayer(PlayerProfile.Player2Name, result.Player2Card);

        switch (result.Outcome)
        {
            case BattleOutcome.Win:
                _comparisonText.text = ">";
                _resultText.text = "Menang";
                break;

            case BattleOutcome.Lose:
                _comparisonText.text = "<";
                _resultText.text = "Kalah";
                break;

            default:
                _comparisonText.text = "=";
                _resultText.text = "Seri";
                break;
        }

        switch (result.Player1.PrimaryReward)
        {
            case PrimaryReward.Attack:
                _primaryRewardIcon.enabled = true;
                _primaryRewardIcon.sprite = _attackIcon;
                _primaryRewardText.text = $"ATK +{result.Player1.AttackReward}";
                break;

            case PrimaryReward.Health:
                _primaryRewardIcon.enabled = true;
                _primaryRewardIcon.sprite = _healthIcon;
                _primaryRewardText.text = $"HP {result.Player1.HpChange}";
                break;

            default:
                _primaryRewardIcon.enabled = false;
                _primaryRewardText.text = "";
                break;
        }

        _energyRewardIcon.sprite = _energyIcon;
        _energyRewardText.text = $"Energy +{result.Player1.EnergyChange}";
    }

    public void Continue()
    {
        ApplyBattleResult();

        _player1.StatusDisplay.Refresh();
        _player2.StatusDisplay.Refresh();

        if (IsGameOver())
        {
            StartCoroutine(GameOverRoutine());
            return;
        }

        _player1.HandManager.RemoveSelectedCard();
        _player2.HandManager.RemoveSelectedCard();

        _player1.HandDisplay.RefreshHand();
        _player2.HandDisplay.RefreshHand();

        _player1.SelectedCardDisplay.Hide();
        _player2.SelectedCardDisplay.Hide();

        _player1.SelectedCardDisplay.Refresh();
        _player2.SelectedCardDisplay.Refresh();

        _opponentActionController.PlayTurn();

        _roundManager.NextRound();
        _roundDisplay.Refresh();

        _stateManager.SetState(GameplayState.Normal);

        Hide();
    }
        
    private void ApplyBattleResult()
    {
        BattleResultData result = _battleManager.LastBattleResult;

        _player1.Status.HP += result.Player1.HpChange;
        _player2.Status.HP += result.Player2.HpChange;

        _player1.Status.HP = Mathf.Clamp(_player1.Status.HP, 0, PlayerProfile.MaxHP);
        _player2.Status.HP = Mathf.Clamp(_player2.Status.HP, 0, PlayerProfile.MaxHP);

        _player1.Status.Energy += result.Player1.EnergyChange;
        _player2.Status.Energy += result.Player2.EnergyChange;

        _player1.Status.Energy = Mathf.Clamp(_player1.Status.Energy, 0, PlayerProfile.MaxEnergy);
        _player2.Status.Energy = Mathf.Clamp(_player2.Status.Energy, 0, PlayerProfile.MaxEnergy);
    }

    private bool IsGameOver()
    {
        return _player1.Status.HP <= 0 || _player2.Status.HP <= 0;
    }

    private MatchResult GetMatchResult()
    {
        if (_player2.Status.HP <= 0)
        {
            return MatchResult.Win;
        }

        return MatchResult.Lose;
    }

    private IEnumerator GameOverRoutine()
    {
        Hide();
        yield return new WaitForSeconds(1f);
        _gameResultPanel.Show(GetMatchResult());
    }
}