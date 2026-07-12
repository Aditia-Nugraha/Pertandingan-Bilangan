using UnityEngine;
using System;

public class CardDestroyManager : MonoBehaviour
{
    [SerializeField] private CardDestroyAnimator _destroyAnimator;

    public void Play(Sprite sprite, RectTransform target, Action onFinished = null)
    {
        _destroyAnimator.Play(sprite, target, onFinished);
    }
}