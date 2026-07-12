using UnityEngine;

public class HealController : MonoBehaviour
{
    [SerializeField] private PlayerContext _player;
    [SerializeField] private GameplayStateManager _stateManager;
    [SerializeField] private GameplayMessageDisplay _messageDisplay;

    private const int HealCost = 15;
    private const int HealAmount = 50;

    public void Heal()
    {
        if (_stateManager.CurrentState == GameplayState.Busy)
        {
            return;
        }

        int oldHP = _player.Status.HP;
        int oldEnergy = _player.Status.Energy;

        if (_player.Status.Energy < HealCost)
        {
            _messageDisplay.Show(GameplayMessage.NotEnoughEnergy);
            return;
        }

        if (_player.Status.HP >= PlayerProfile.MaxHP)
        {
            _messageDisplay.Show(GameplayMessage.HPFull);
            return;
        }

        _messageDisplay.Show(GameplayMessage.Heal);
        _player.Status.Energy -= HealCost;
        _player.Status.HP += HealAmount;
        _player.Status.HP = Mathf.Clamp(_player.Status.HP, 0, PlayerProfile.MaxHP);
        _player.StatusDisplay.AnimateRefresh(oldHP, oldEnergy);
    }
}