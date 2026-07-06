using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerContext _player;

    [Header("UI")]
    [SerializeField] private TMP_Text _playerNameText;

    [SerializeField] private Image _hpFill;
    [SerializeField] private TMP_Text _hpText;

    [SerializeField] private Image _energyFill;
    [SerializeField] private TMP_Text _energyText;

    public void Refresh()
    {
        if (_player.PlayerSide == PlayerSide.Player1)
        {
            _playerNameText.text = PlayerProfile.Player1Name;

            _hpText.text = _player.Status.HP.ToString();
            _energyText.text = _player.Status.Energy.ToString();

            _hpFill.fillAmount = (float)_player.Status.HP / PlayerProfile.MaxHP;
            _energyFill.fillAmount = (float)_player.Status.Energy / PlayerProfile.MaxEnergy;
        }
        else
        {
            _playerNameText.text = PlayerProfile.Player2Name;

            _hpText.text = _player.Status.HP.ToString();
            _energyText.text = _player.Status.Energy.ToString();

            _hpFill.fillAmount = (float)_player.Status.HP / PlayerProfile.MaxHP;
            _energyFill.fillAmount = (float)_player.Status.Energy / PlayerProfile.MaxEnergy;
        }
    }
}