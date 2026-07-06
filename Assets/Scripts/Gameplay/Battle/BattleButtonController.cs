using UnityEngine;
using UnityEngine.UI;

public class BattleButtonController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameplayStateManager _stateManager;
    [SerializeField] private Button _button;

    private void Start()
    {
        Refresh();
    }

    private void OnEnable()
    {
        _stateManager.OnStateChanged += HandleStateChanged;
    }

    private void OnDisable()
    {
        _stateManager.OnStateChanged -= HandleStateChanged;
    }

    private void HandleStateChanged(GameplayState state)
    {
        Refresh();
    }

    private void Refresh()
    {
        _button.interactable = _stateManager.IsState(GameplayState.Normal);
    }
}