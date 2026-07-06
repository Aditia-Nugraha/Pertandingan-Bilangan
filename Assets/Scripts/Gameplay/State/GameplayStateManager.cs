using System;
using UnityEngine;

public class GameplayStateManager : MonoBehaviour
{
    public GameplayState CurrentState { get; private set; } = GameplayState.Normal;
    public event Action<GameplayState> OnStateChanged;

    public void SetState(GameplayState newState)
    {
        if (CurrentState == newState)
        {
            return;
        }

        CurrentState = newState;
        OnStateChanged?.Invoke(CurrentState);
    }

    public bool IsState(GameplayState state)
    {
        return CurrentState == state;
    }
}