using UnityEngine;

public class RoundManager : MonoBehaviour
{
    private int _currentRound = 1;
    public int CurrentRound => _currentRound;

    public void ResetRound()
    {
        _currentRound = 1;
    }

    public void NextRound()
    {
        _currentRound++;
    }
}