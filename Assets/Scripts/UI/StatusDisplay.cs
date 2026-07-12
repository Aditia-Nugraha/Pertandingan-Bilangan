using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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

    [Header("Animation")]
    [SerializeField] private float _animationDuration = 0.5f;

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

    public void AnimateRefresh(int oldHP, int oldEnergy)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateRoutine(oldHP, oldEnergy));
    }

    private IEnumerator AnimateRoutine(int oldHP, int oldEnergy)
    {
        float elapsed = 0f;
        int targetHP = _player.Status.HP;
        int targetEnergy = _player.Status.Energy;

        while (elapsed < _animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _animationDuration);
            int currentHP = Mathf.RoundToInt(Mathf.Lerp(oldHP, targetHP, t));
            _hpText.text = currentHP.ToString();
            _hpFill.fillAmount = Mathf.Lerp((float)oldHP / PlayerProfile.MaxHP,
            (float)targetHP / PlayerProfile.MaxHP, t);

            int currentEnergy = Mathf.RoundToInt(Mathf.Lerp(oldEnergy, targetEnergy, t));
            _energyText.text = currentEnergy.ToString();
            _energyFill.fillAmount = Mathf.Lerp((float)oldEnergy / PlayerProfile.MaxEnergy,
            (float)targetEnergy / PlayerProfile.MaxEnergy, t);
            yield return null;
        }

        _hpText.text = targetHP.ToString();
        _hpFill.fillAmount = (float)targetHP / PlayerProfile.MaxHP;

        _energyText.text = targetEnergy.ToString();
        _energyFill.fillAmount = (float)targetEnergy / PlayerProfile.MaxEnergy;
    }
}