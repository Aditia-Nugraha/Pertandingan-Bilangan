using UnityEngine;
using System;

public class CardTransitionManager : MonoBehaviour
{
    [Header("Animators")]
    [SerializeField] private CardTransitionAnimator _forwardAnimator;
    [SerializeField] private CardTransitionAnimator _backwardAnimator;

    public void PlaySingle(Sprite sprite, RectTransform from, RectTransform to, Action onFinished)
    {
        _forwardAnimator.Play(sprite, from, to, onFinished);
    }

    public void PlayReplace(
        Sprite oldSprite,
        RectTransform oldFrom,
        RectTransform oldTo,
        Sprite newSprite,
        RectTransform newFrom,
        RectTransform newTo,
        Action onFinished)
    {
        int completed = 0;

        void FinishOne()
        {
            completed++;

            if (completed >= 2)
            {
                onFinished?.Invoke();
            }
        }

        _backwardAnimator.Play(oldSprite, oldFrom, oldTo, FinishOne);
        _forwardAnimator.Play(newSprite, newFrom, newTo, FinishOne);
    }

    public void PlayDraw(
        Sprite selectedSprite,
        RectTransform selectedFrom,
        RectTransform selectedTo,
        Sprite drawSprite,
        RectTransform drawFrom,
        RectTransform drawTo,
        Action onFinished)
    {
        int completed = 0;

        void FinishOne()
        {
            completed++;

            if (completed >= 2)
            {
                onFinished?.Invoke();
            }
        }

        _backwardAnimator.Play(selectedSprite, selectedFrom, selectedTo, FinishOne);
        _forwardAnimator.Play(drawSprite, drawFrom, drawTo, FinishOne);
    }

    public void PlayTogether(
        Sprite sprite1,
        RectTransform from1,
        RectTransform to1,
        Sprite sprite2,
        RectTransform from2,
        RectTransform to2,
        Action onFinished)
    {
        int completed = 0;

        void FinishOne()
        {
            completed++;

            if (completed >= 2)
            {
                onFinished?.Invoke();
            }
        }

        _forwardAnimator.Play(sprite1, from1, to1, FinishOne);
        _backwardAnimator.Play(sprite2, from2, to2, FinishOne);
    }

    public void PlayReturn(
        Sprite sprite,
        RectTransform from,
        RectTransform to,
        Action onFinished)
    {
        _backwardAnimator.Play(sprite, from, to, onFinished);
    }
}