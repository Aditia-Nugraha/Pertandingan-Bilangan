using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandReorderManager : MonoBehaviour
{
    [SerializeField] private HandReorderAnimator _animatorPrefab;

    private Coroutine _reorderCoroutine;

    public void Play(
        List<HandReorderData> snapshot,
        HandDisplay handDisplay,
        int removedSlotIndex,
        Action onFinished = null)
    {
        if (_reorderCoroutine != null)
        {
            StopCoroutine(_reorderCoroutine);
        }

        _reorderCoroutine = StartCoroutine(
            PlayRoutine(
                snapshot,
                handDisplay,
                removedSlotIndex,
                onFinished));
    }

    private IEnumerator PlayRoutine(
        List<HandReorderData> snapshot,
        HandDisplay handDisplay,
        int removedSlotIndex,
        Action onFinished)
    {
        int finishedCount = 0;
        int totalAnimations = 0;

        for (int i = removedSlotIndex + 1; i < snapshot.Count; i++)
        {
            if (snapshot[i].Card != null)
            {
                totalAnimations++;
            }
        }

        if (totalAnimations == 0)
        {
            _reorderCoroutine = null;
            onFinished?.Invoke();
            yield break;
        }

        for (int i = removedSlotIndex + 1; i < snapshot.Count; i++)
        {
            HandReorderData current = snapshot[i];

            if (current.Card == null)
            {
                continue;
            }

            handDisplay.HideSlot(current.SlotIndex);
            HandReorderAnimator animator =
                Instantiate(
                    _animatorPrefab,
                    transform);
            Sprite displaySprite = handDisplay.GetDisplaySprite(current.SlotIndex);
            animator.Play(
                displaySprite,
                handDisplay.GetSlotTransform(current.SlotIndex),
                handDisplay.GetSlotTransform(current.SlotIndex - 1),
                () =>
                {
                    finishedCount++;
                    Destroy(animator.gameObject);
                });
        }

        yield return new WaitUntil(() => finishedCount == totalAnimations);
        _reorderCoroutine = null;
        onFinished?.Invoke();
    }
}