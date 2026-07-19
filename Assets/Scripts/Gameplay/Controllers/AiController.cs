using UnityEngine;
using System.Collections;

public class AiController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerContext _player2;
    [SerializeField] private HandManager _aiHandManager;
    [SerializeField] private HandManager _playerHandManager;
    [SerializeField] private HandDisplay _aiHandDisplay;
    [SerializeField] private SelectedCardDisplay _selectedCardDisplay;

    [Header("Animation")]
    [SerializeField] private GameplayMessageDisplay _messageDisplay;
    [SerializeField] private CardTransitionAnimator _cardTransitionAnimator;
    [SerializeField] private CardTransitionManager _transitionManager;
    [SerializeField] private DealAnimationController _dealAnimationController;
    [SerializeField] private DrawAnimationService _drawAnimationService;
    [SerializeField] private Sprite _closedCardSprite;

    [Header("Action Chance")]
    [SerializeField, Range(0f, 1f)] private float _drawChance = 0.5f;
    [SerializeField, Range(0f, 1f)] private float _healChance = 0.8f;

    [Header("Heal")]
    [SerializeField] private int _healThreshold = 900;

    private const int DrawCost = 5;
    private const int HealCost = 15;
    private const int HealAmount = 50;

    private bool IsPlayerVsComputer => PlayerProfile.CurrentGameMode == GameMode.PlayerVsComputer;

    private void Awake()
    {
        if (!IsPlayerVsComputer)
        {
            enabled = false;
        }
    }

    private void OnEnable()
    {
        if (!IsPlayerVsComputer)
        {
            return;
        }

        _playerHandManager.OnCardSelected += HandlePlayerCardSelected;
    }

    private void OnDisable()
    {
        if (!IsPlayerVsComputer)
        {
            return;
        }

        _playerHandManager.OnCardSelected -= HandlePlayerCardSelected;
    }

    private void HandlePlayerCardSelected(int slotIndex)
    {
        if (_aiHandManager.HasSelectedCard())
        {
            return;
        }

        StartCoroutine(SelectCardDelay());
    }

    private IEnumerator SelectCardDelay()
    {
        float delay = Random.Range(0.5f, 1f);
        yield return new WaitForSeconds(delay);
        int slotIndex = _aiHandManager.GetRandomAvailableSlot();
        AudioManager.Instance.PlaySfx(GameSfx.CardMove);

        if (slotIndex < 0)
        {
            yield break;
        }

        CardData card = _aiHandManager.Hand[slotIndex];
        RectTransform from = _aiHandDisplay.GetSlotTransform(slotIndex);
        RectTransform to = _selectedCardDisplay.GetSlotTransform();

        _aiHandDisplay.HideSlot(slotIndex);
        _cardTransitionAnimator.Play(_closedCardSprite, from, to, () =>
        {
            _aiHandManager.SelectCard(slotIndex);
            _aiHandDisplay.RefreshHand();
            _selectedCardDisplay.Refresh();
        });
    }

    public IEnumerator TryDraw()
    {
        int oldHP = _player2.Status.HP;
        int oldEnergy = _player2.Status.Energy;

        if (NeedsEmergencyDraw())
        {
            yield return StartCoroutine(EmergencyDraw());
        }
        else
        {
            yield return StartCoroutine(NormalDraw());
        }

        _player2.StatusDisplay.AnimateRefresh(oldHP, oldEnergy);
    }

    private bool CanDraw()
    {
        return _player2.Status.Energy >= DrawCost && !_player2.HandManager.IsHandFull();
    }

    private bool CanHeal()
    {
        return _player2.Status.Energy >= HealCost && _player2.Status.HP < _healThreshold;
    }

    private bool ShouldDraw()
    {
        return Random.value < _drawChance;
    }

    private bool ShouldHeal()
    {
        return Random.value < _healChance;
    }

    public bool WantsToDraw()
    {
        return CanDraw() && ShouldDraw();
    }

    public bool NeedsEmergencyDraw()
    {
        return _player2.HandManager.Hand.Count < 2;
    }

    private IEnumerator EmergencyDraw()
    {
        while (CanDraw())
        {
            yield return StartCoroutine(NormalDraw());
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator NormalDraw()
    {
        AudioManager.Instance.PlaySfx(GameSfx.Message);
        _messageDisplay.Show(GameplayMessage.OpponentDraw);
        _player2.Status.Energy -= DrawCost;
        int slotIndex = _player2.HandManager.DrawOneCard();

        if (slotIndex < 0)
        {
            yield break;
        }

        yield return StartCoroutine(
            _drawAnimationService.PlayDraw(
                _player2,
                _transitionManager,
                slotIndex));
    }

    private void Heal()
    {
        AudioManager.Instance.PlaySfx(GameSfx.Message);
        _messageDisplay.Show(GameplayMessage.OpponentHeal);
        _player2.Status.Energy -= HealCost;
        _player2.Status.HP += HealAmount;
        _player2.Status.HP = Mathf.Clamp(_player2.Status.HP, 0, PlayerProfile.MaxHP);
    }

    public bool TryHeal()
    {
        int oldHP = _player2.Status.HP;
        int oldEnergy = _player2.Status.Energy;

        if (!CanHeal())
        {
            return false;
        }

        if (!ShouldHeal())
        {
            return false;
        }

        Heal();
        _player2.StatusDisplay.AnimateRefresh(oldHP, oldEnergy);
        return true;
    }
}