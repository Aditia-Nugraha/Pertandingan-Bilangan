using UnityEngine;
using UnityEngine.EventSystems;

public class CardSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private PlayerSide _playerSide;
    [SerializeField] private int _slotIndex;
    [SerializeField] private HumanController _controller;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_playerSide != PlayerProfile.CurrentViewingSide)
        {
            return;
        }

        _controller.SelectCard(_slotIndex);
    }
}