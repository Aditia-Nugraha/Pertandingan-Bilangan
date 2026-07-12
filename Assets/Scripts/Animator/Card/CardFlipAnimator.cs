using UnityEngine;
using System;
using System.Collections;

public class CardFlipAnimator : MonoBehaviour
{
    [SerializeField] private CardDisplay _cardDisplay;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Sprite _closedCardSprite;
    [SerializeField] private float _duration = 0.2f;
    private Vector3 _defaultScale;

    private void Awake()
    {
        _defaultScale = _rectTransform.localScale;
        _cardDisplay.ClearCard();
    }

    public void Play(CardData card, RectTransform target, Action onFinished = null)
    {
        StopAllCoroutines();
        StartCoroutine(FlipRoutine(card, target, onFinished));
    }

    private IEnumerator FlipRoutine(CardData card, RectTransform target, Action onFinished)
    {
        _rectTransform.position = target.position;
        _rectTransform.localScale = _defaultScale;
        _cardDisplay.SetSprite(_closedCardSprite);
        _cardDisplay.CardImage.enabled = true;
        float halfDuration = _duration * 0.5f;
        Vector3 startScale = _rectTransform.localScale;
        float elapsed = 0f;

        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / halfDuration);
            _rectTransform.localScale = new Vector3(
                Mathf.Lerp(startScale.x, 0f, t),
                startScale.y,
                startScale.z);
            yield return null;
        }

        _cardDisplay.SetCard(card);
        elapsed = 0f;

        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / halfDuration);
            _rectTransform.localScale = new Vector3(
                Mathf.Lerp(0f, startScale.x, t),
                startScale.y,
                startScale.z);
            yield return null;
        }

        _cardDisplay.ClearCard();
        _rectTransform.localScale = startScale;
        onFinished?.Invoke();
    }
}