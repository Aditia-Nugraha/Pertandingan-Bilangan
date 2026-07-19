using UnityEngine;
using System.Collections;

public class OpponentActionController : MonoBehaviour
{
    [SerializeField] private AiController _aiController;
    [SerializeField] private GameplaySyncController _gameplaySyncController;

    public void PlayTurn()
    {
        switch (PlayerProfile.CurrentGameMode)
        {
            case GameMode.PlayerVsComputer:
                StartCoroutine(TurnRoutine());
                break;
        }
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

        yield return StartCoroutine(_aiController.TryDraw());
    }

    private IEnumerator EmergencyDrawRoutine()
    {
        if (!_aiController.NeedsEmergencyDraw())
        {
            yield break;
        }

        yield return StartCoroutine(_aiController.TryDraw());
    }
}