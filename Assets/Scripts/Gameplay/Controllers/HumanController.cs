using UnityEngine;

public class HumanController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HandManager _handManager;
    [SerializeField] private HandDisplay _handDisplay;
    [SerializeField] private SelectedCardDisplay _selectedCardDisplay;
    [SerializeField] private GameplayStateManager _stateManager;
    [SerializeField] private ReplaceController _replaceController;

    public void SelectCard(int slotIndex)
    {
        switch (_stateManager.CurrentState)
        {
            case GameplayState.Normal:
                SelectBattleCard(slotIndex);
                break;

            case GameplayState.ReplaceCard:
                _replaceController.Replace(slotIndex);
                break;

            default:
                return;
        }
    }

    private void SelectBattleCard(int slotIndex)
    {
        _handManager.SelectCard(slotIndex);
        _handDisplay.RefreshHand();
        _selectedCardDisplay.Refresh();
    }
}