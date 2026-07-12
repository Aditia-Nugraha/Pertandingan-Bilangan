using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class BattleResultPanel : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject _container;
    [SerializeField] private GameObject _background;
    [SerializeField] private GameObject _panel;
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

    [Header("Animation")]
    [SerializeField] private BattleResultSequence _battleResultSequence;
    [SerializeField] private ContinueButtonAnimator _continueButtonAnimator;
    [SerializeField] private ScalePanelAnimator _panelAnimator;
    [SerializeField] private CardDestroyManager _player1DestroyManager;
    [SerializeField] private CardDestroyManager _player2DestroyManager;
    [SerializeField] private HandReorderManager _player1HandReorder;
    [SerializeField] private HandReorderManager _player2HandReorder;

    private const float DestroyDelay = 0.2f;

    public void Show()
    {
        _stateManager.SetState(GameplayState.BattleResult);
        _container.SetActive(true);
        _background.SetActive(true);
        _panel.SetActive(true);
        _battleResultSequence.Play();
        Refresh();
    }

    public void Hide()
    {
        _panel.SetActive(false);
        _container.SetActive(false);
    }

    public void HideResultText()
    {
        Color color = _roundText.color;
        color.a = 0f;
        _roundText.color = color;

        Color comparisonColor = _comparisonText.color;
        comparisonColor.a = 0f;
        _comparisonText.color = comparisonColor;

        Color resultColor = _resultText.color;
        resultColor.a = 0f;
        _resultText.color = resultColor;

        Color primaryIconColor = _primaryRewardIcon.color;
        primaryIconColor.a = 0f;
        _primaryRewardIcon.color = primaryIconColor;

        Color primaryTextColor = _primaryRewardText.color;
        primaryTextColor.a = 0f;
        _primaryRewardText.color = primaryTextColor;

        Color energyIconColor = _energyRewardIcon.color;
        energyIconColor.a = 0f;
        _energyRewardIcon.color = energyIconColor;

        Color energyTextColor = _energyRewardText.color;
        energyTextColor.a = 0f;
        _energyRewardText.color = energyTextColor;
    }

    private void Refresh()
    {
        HideResultText();
        _continueButtonAnimator.ResetState();
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
        _background.SetActive(false);
        _panelAnimator.PlayHide(ContinueAfterHide);
    }

    private void ContinueAfterHide()
    {
        StartCoroutine(ContinueRoutine());
    }

    private IEnumerator ContinueRoutine()
    {
        yield return new WaitForSeconds(DestroyDelay);

        bool player1Finished = false;
        bool player2Finished = false;

        CardData player1Card = _battleManager.LastBattleResult.Player1Card;
        CardData player2Card = _battleManager.LastBattleResult.Player2Card;

        _player1.SelectedCardDisplay.HideImage();
        _player2.SelectedCardDisplay.HideImage();

        _player1DestroyManager.Play(
            player1Card.CardSprite,
            _player1.SelectedCardDisplay.GetSlotTransform(),
            () =>
            {
                player1Finished = true;
            });

        _player2DestroyManager.Play(
            player2Card.CardSprite,
            _player2.SelectedCardDisplay.GetSlotTransform(),
            () =>
            {
                player2Finished = true;
            });

        yield return new WaitUntil(() =>
            player1Finished &&
            player2Finished);

        ContinueGameplay();
    }

    public void ContinueGameplay()
    {
        int oldPlayer1HP = _player1.Status.HP;
        int oldPlayer1Energy = _player1.Status.Energy;
        int oldPlayer2HP = _player2.Status.HP;
        int oldPlayer2Energy = _player2.Status.Energy;

        ApplyBattleResult();

        _player1.StatusDisplay.AnimateRefresh(oldPlayer1HP, oldPlayer1Energy);
        _player2.StatusDisplay.AnimateRefresh(oldPlayer2HP, oldPlayer2Energy);

        if (IsGameOver())
        {
            StartCoroutine(GameOverRoutine());
            return;
        }

        int player1RemovedIndex = _player1.HandManager.SelectedCard.OriginalSlotIndex;
        int player2RemovedIndex = _player2.HandManager.SelectedCard.OriginalSlotIndex;

        List<HandReorderData> player1Snapshot = _player1.HandDisplay.CreateSnapshot();
        List<HandReorderData> player2Snapshot = _player2.HandDisplay.CreateSnapshot();

        StartCoroutine(
        PlayHandReorderRoutine(
            player1Snapshot,
            player2Snapshot,
            player1RemovedIndex,
            player2RemovedIndex));
    }

    private IEnumerator PlayHandReorderRoutine(
        List<HandReorderData> player1Snapshot,
        List<HandReorderData> player2Snapshot,
        int player1RemovedIndex,
        int player2RemovedIndex)
    {
        bool player1Finished = false;
        bool player2Finished = false;

        _player1HandReorder.Play(
            player1Snapshot,
            _player1.HandDisplay,
            player1RemovedIndex,
            () =>
            {
                player1Finished = true;
            });

        _player2HandReorder.Play(
            player2Snapshot,
            _player2.HandDisplay,
            player2RemovedIndex,
            () =>
            {
                player2Finished = true;
            });

        yield return new WaitUntil(() =>
            player1Finished &&
            player2Finished);

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