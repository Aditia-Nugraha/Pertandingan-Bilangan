using TMPro;
using UnityEngine;

public class RoundDisplay : MonoBehaviour
{
    [SerializeField] private RoundManager _roundManager;
    [SerializeField] private TMP_Text _roundText;

    public void Refresh()
    {
        _roundText.text = $"Ronde {_roundManager.CurrentRound}";
    }
}