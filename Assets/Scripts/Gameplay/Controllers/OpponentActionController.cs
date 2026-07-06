using UnityEngine;
using System.Collections;

public class OpponentActionController : MonoBehaviour
{
    [SerializeField] private AiController _aiController;
    [SerializeField] private GameplayMessageDisplay _messageDisplay;

    public void PlayTurn()
    {
        StartCoroutine(TurnRoutine());
    }

    private IEnumerator TurnRoutine()
    {
        float delay = Random.Range(1f, 2f);
        yield return new WaitForSeconds(delay);

        if (_aiController.NeedsEmergencyDraw())
        {
            yield return EmergencyDrawRoutine();
            yield break;
        }

        if (_aiController.TryHeal())
        {
            _messageDisplay.Show(GameplayMessage.OpponentHeal);
            yield break;
        }

        yield return NormalDrawRoutine();
    }

    private IEnumerator NormalDrawRoutine()
    {
        if (_aiController.NeedsEmergencyDraw())
        {
            yield break;
        }

        if (!_aiController.WantsToDraw())
        {
            yield break;
        }

        _messageDisplay.Show(GameplayMessage.OpponentDraw);
        _aiController.TryDraw();
    }

    private IEnumerator EmergencyDrawRoutine()
    {
        if (!_aiController.NeedsEmergencyDraw())
        {
            yield break;
        }

        _messageDisplay.Show(GameplayMessage.OpponentDraw);
        _aiController.TryDraw();
    }
}