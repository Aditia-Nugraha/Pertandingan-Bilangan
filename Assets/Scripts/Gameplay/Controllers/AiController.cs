using UnityEngine;
using System.Collections;

public class AiController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HandManager _aiHandManager;
    [SerializeField] private HandManager _playerHandManager;
    [SerializeField] private HandDisplay _handDisplay;
    [SerializeField] private SelectedCardDisplay _selectedCardDisplay;
    [SerializeField] private PlayerContext _player2;

    [Header("Action Chance")]
    [SerializeField, Range(0f, 1f)] private float _drawChance = 0.5f;
    [SerializeField, Range(0f, 1f)] private float _healChance = 0.8f;

    [Header("Heal")]
    [SerializeField] private int _healThreshold = 900;

    private const int DrawCost = 5;
    private const int HealCost = 15;
    private const int HealAmount = 50;

    private void OnEnable()
    {
        _playerHandManager.OnCardSelected += HandlePlayerCardSelected;
    }

    private void OnDisable()
    {
        _playerHandManager.OnCardSelected -= HandlePlayerCardSelected;
    }

    public void HandlePlayerCardSelected()
    {
        if (_aiHandManager.HasSelectedCard())
        {
            return;
        }

        StartCoroutine(SelectCardDelay());
    }

    private IEnumerator SelectCardDelay()
    {
        float delay = Random.Range(1f, 2f);
        yield return new WaitForSeconds(delay);

        _aiHandManager.SelectRandomCard();
        _handDisplay.RefreshHand();
        _selectedCardDisplay.Refresh();
    }

    public void TryDraw()
    {
        if (NeedsEmergencyDraw())
        {
            EmergencyDraw();
        }
        else
        {
            NormalDraw();
        }

        _player2.HandDisplay.RefreshHand();
        _player2.StatusDisplay.Refresh();
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

    private void EmergencyDraw()
    {
        while (CanDraw())
        {
            _player2.Status.Energy -= DrawCost;
            _player2.HandManager.DrawOneCard();
        }
    }

    private void NormalDraw()
    {
        _player2.Status.Energy -= DrawCost;
        _player2.HandManager.DrawOneCard();
    }

    private void Heal()
    {
        _player2.Status.Energy -= HealCost;
        _player2.Status.HP += HealAmount;
        _player2.Status.HP = Mathf.Clamp(_player2.Status.HP, 0, PlayerProfile.MaxHP);
    }

    public bool TryHeal()
    {
        if (!CanHeal())
        {
            return false;
        }

        if (!ShouldHeal())
        {
            return false;
        }

        Heal();
        _player2.StatusDisplay.Refresh();
        return true;
    }
}