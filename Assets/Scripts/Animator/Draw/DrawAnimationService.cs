using UnityEngine;
using System;
using System.Collections;

public class DrawAnimationService : MonoBehaviour
{
    [SerializeField] private DealAnimationController _dealAnimationController;

    public IEnumerator PlayDraw(
        PlayerContext player,
        CardTransitionManager transition,
        int slotIndex,
        Action onFinished = null)
    {
        player.HandDisplay.RefreshHand();
        player.HandDisplay.HideSlot(slotIndex);

        yield return StartCoroutine(
            _dealAnimationController.PlayDealAnimation(
                player,
                transition,
                slotIndex,
                null));

        onFinished?.Invoke();
    }
}