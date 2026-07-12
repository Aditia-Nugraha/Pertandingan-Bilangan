using UnityEngine;
using System;

public class CardFlipManager : MonoBehaviour
{
    [SerializeField] private CardFlipAnimator _flipAnimator;

    public void Play(CardData card, RectTransform target, Action onFinished = null)
    {
        _flipAnimator.Play(card, target, onFinished);
    }
}